using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static W9xNET.Interop.Win32.Cabinet;

namespace W9xNET.Cabinet
{
    public class CabExtractor : IDisposable
    {
        #region Fields and Properties

        /// Very important!
        /// Do not try to call directly to these methods, instead use the delegates. if you use them directly it may cause application crashes, corruption and data loss.
        /// Using fields to save the delegate so that the delegate won't be garbage collected  !
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application until it is guaranteed that they will never be called.
        private readonly FdiMemAllocDelegate _fdiAllocMemHandler;
        private readonly FdiMemFreeDelegate _fdiFreeMemHandler;
        private readonly FdiFileOpenDelegate _fdiOpenStreamHandler;
        private readonly FdiFileReadDelegate _fdiReadStreamHandler;
        private readonly FdiFileWriteDelegate _fdiWriteStreamHandler;
        private readonly FdiFileCloseDelegate _fdiCloseStreamHandler;
        private readonly FdiFileSeekDelegate _fdiSeekStreamHandler;

        private ArchiveFile? _currentFileToDecompress;
        readonly List<string> _fileNames = new();
        private readonly CabError _erf;
        private const int CpuTypeUnknown = -1;
        private readonly byte[] _inputData;
        private bool _disposed;
        /// <summary>
        /// 
        /// </summary>
        private readonly List<string> _subDirectoryToIgnore = new List<string>();
        /// <summary>
        /// Path to the folder where the files will be extracted to
        /// </summary>
        private readonly string _extractionFolderPath;
        /// <summary>
        /// The name of the folder where the files will be extracted to
        /// </summary>
        public readonly string ExtractedFolderName;

        #endregion

        #region Constructor(s)

        public CabExtractor(string cabFilePath, string extractedFolderName, IEnumerable<string> subDirectoryToUnpack)
            : this(cabFilePath, extractedFolderName)
        {
            ExtractedFolderName = extractedFolderName;

            if (subDirectoryToUnpack != null)
                _subDirectoryToIgnore.AddRange(subDirectoryToUnpack);
        }

        public CabExtractor(string cabFilePath, string extractedFolderName)
        {
            ExtractedFolderName = extractedFolderName;

            var cabBytes =
               File.ReadAllBytes(cabFilePath);
            _inputData = cabBytes;
            var cabFileLocation = Path.GetDirectoryName(cabFilePath) ?? "";
            _extractionFolderPath = Path.Combine(cabFileLocation, ExtractedFolderName);
            _erf = new CabError();
            FdiContext = IntPtr.Zero;

            _fdiAllocMemHandler = MemAlloc;
            _fdiFreeMemHandler = MemFree;
            _fdiOpenStreamHandler = InputFileOpen;
            _fdiReadStreamHandler = FileRead;
            _fdiWriteStreamHandler = FileWrite;
            _fdiCloseStreamHandler = InputFileClose;
            _fdiSeekStreamHandler = FileSeek;

            FdiContext = FdiCreate(_fdiAllocMemHandler, _fdiFreeMemHandler, _fdiOpenStreamHandler, _fdiReadStreamHandler, _fdiWriteStreamHandler, _fdiCloseStreamHandler, _fdiSeekStreamHandler, _erf);
        }

        #endregion

        public bool ExtractCabFiles()
        {
            if (!FdiIterate())
            {
                throw new Exception("Failed to iterate cab files");
            }

            foreach (var file in _fileNames)
            {
                ExtractFile(file);
            }
            return true;
        }

        public void ExtractFile(string fileName)
        {
            _currentFileToDecompress = new ArchiveFile { Name = fileName };
            FdiCopy();
            CreateAllRelevantDirectories(fileName);
            if (_currentFileToDecompress.Data != null)
            {
                File.WriteAllBytes(Path.Combine(_extractionFolderPath, _currentFileToDecompress.Name), _currentFileToDecompress.Data);
            }
        }

        private void CreateAllRelevantDirectories(string filePath)
        {
            if (!Directory.Exists(_extractionFolderPath))
            {
                Directory.CreateDirectory(_extractionFolderPath);
            }
            var fullPathToFile = Path.GetDirectoryName(filePath);
            if (fullPathToFile != null &&
                !Directory.Exists(Path.Combine(_extractionFolderPath, fullPathToFile)))
            {
                Directory.CreateDirectory(Path.Combine(_extractionFolderPath, fullPathToFile));
            }
        }

        private static string GetFileName(FdiNotification notification)
        {
            var encoding = ((int)notification.attribs & 128) != 0 ? Encoding.UTF8 : Encoding.Default;
            int length = 0;
            while (Marshal.ReadByte(notification.psz1, length) != 0)
                checked { ++length; }
            var numArray = new byte[length];
            Marshal.Copy(notification.psz1, numArray, 0, length);
            string path = encoding.GetString(numArray);
            if (Path.IsPathRooted(path))
                path = path.Replace(String.Concat(Path.VolumeSeparatorChar), "");
            return path;
        }
        private IntPtr ExtractCallback(FdiNotificationType fdint, FdiNotification fdin)
        {
            switch (fdint)
            {
                case FdiNotificationType.CopyFile:
                    return CopyFiles(fdin);
                case FdiNotificationType.CloseFileInfo:
                    return OutputFileClose(fdin);
                default:
                    return IntPtr.Zero;
            }
        }

        private IntPtr IterateCallback(FdiNotificationType fdint, FdiNotification fdin)
        {
            switch (fdint)
            {
                case FdiNotificationType.CopyFile:
                    return OutputFileOpen(fdin);
                default:
                    return IntPtr.Zero;
            }
        }

        private IntPtr InputFileOpen(string fileName, int oflag, int pmode)
        {
            var stream = new MemoryStream(_inputData);
            GCHandle gch = GCHandle.Alloc(stream);
            return (IntPtr)gch;
        }

        private int InputFileClose(IntPtr hf)
        {
            var stream = StreamFromHandle(hf);
            stream.Close();
            ((GCHandle)(hf)).Free();
            return 0;
        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private IntPtr CopyFiles(FdiNotification fdin)
        {
            var fileName = GetFileName(fdin);
            var extractFile = _currentFileToDecompress.Name == fileName ? _currentFileToDecompress : null;
            if (extractFile != null)
            {
                var stream = new MemoryStream();
                GCHandle gch = GCHandle.Alloc(stream);
                extractFile.Handle = (IntPtr)gch;
                return extractFile.Handle;
            }

            //Do not extract this file
            return IntPtr.Zero;
        }
        private IntPtr OutputFileOpen(FdiNotification fdin)
        {
            var extractFile = new ArchiveFile { Name = GetFileName(fdin) };
            if (ShouldIgnoreFile(extractFile))
            {
                //ignore this file.
                return IntPtr.Zero;
            }
            var stream = new MemoryStream();
            GCHandle gch = GCHandle.Alloc(stream);
            extractFile.Handle = (IntPtr)gch;

            AddToListOfFiles(extractFile);

            //return IntPtr.Zero so that the iteration will keep on going
            return IntPtr.Zero;
        }

        private bool ShouldIgnoreFile(ArchiveFile extractFile)
        {
            var rootFolder = GetFileRootFolder(extractFile.Name);
            return _subDirectoryToIgnore.Any(dir => dir.Equals(rootFolder, StringComparison.InvariantCultureIgnoreCase));
        }

        private string GetFileRootFolder(string path)
        {
            try
            {
                return path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void AddToListOfFiles(ArchiveFile extractFile)
        {
            if (!_fileNames.Any(file => file.Equals(extractFile.Name)))
            {
                _fileNames.Add(extractFile.Name);
            }
        }

        private IntPtr OutputFileClose(FdiNotification fdin)
        {
            Debug.Assert(_currentFileToDecompress != null, "_currentFileToDecompress is null");

            var extractFile = _currentFileToDecompress.Handle == fdin.hf ? _currentFileToDecompress : null;
            var stream = StreamFromHandle(fdin.hf);

            if (extractFile != null)
            {
                extractFile.Found = true;
                extractFile.Length = (int)stream.Length;

                if (stream.Length > 0)
                {
                    extractFile.Data = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(extractFile.Data, 0, (int)stream.Length);
                }
            }

            stream.Close();
            return IntPtr.Zero;
        }

        private static IntPtr FdiCreate(
            FdiMemAllocDelegate fnMemAlloc,
            FdiMemFreeDelegate fnMemFree,
            FdiFileOpenDelegate fnFileOpen,
            FdiFileReadDelegate fnFileRead,
            FdiFileWriteDelegate fnFileWrite,
            FdiFileCloseDelegate fnFileClose,
            FdiFileSeekDelegate fnFileSeek,
            CabError erf)
        {
            return Interop.Win32.Cabinet.FdiCreate(fnMemAlloc, fnMemFree, fnFileOpen, fnFileRead, fnFileWrite,
                             fnFileClose, fnFileSeek, CpuTypeUnknown, erf);
        }

        private static int FileRead(IntPtr hf, byte[] buffer, int cb)
        {
            var stream = StreamFromHandle(hf);
            return stream.Read(buffer, 0, cb);
        }

        private static int FileWrite(IntPtr hf, byte[] buffer, int cb)
        {
            var stream = StreamFromHandle(hf);
            stream.Write(buffer, 0, cb);
            return cb;
        }

        private static Stream StreamFromHandle(IntPtr hf)
        {
            return (Stream)((GCHandle)hf).Target!;
        }

        private IntPtr MemAlloc(int cb)
        {
            return Marshal.AllocHGlobal(cb);
        }

        private void MemFree(IntPtr mem)
        {
            Marshal.FreeHGlobal(mem);
        }

        private int FileSeek(IntPtr hf, int dist, int seektype)
        {
            var stream = StreamFromHandle(hf);
            return (int)stream.Seek(dist, (SeekOrigin)seektype);
        }

        private bool FdiCopy()
        {
            try
            {
                return Interop.Win32.Cabinet.FdiCopy(FdiContext, "<notused>", "<notused>", 0, ExtractCallback, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool FdiIterate()
        {
            return Interop.Win32.Cabinet.FdiCopy(FdiContext, "<notused>", "<notused>", 0, IterateCallback, IntPtr.Zero, IntPtr.Zero);
        }

        private IntPtr FdiContext { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_disposed)
                {
                    if (FdiContext != IntPtr.Zero)
                    {
                        Interop.Win32.Cabinet.FdiDestroy(FdiContext);
                        FdiContext = IntPtr.Zero;
                    }
                    _disposed = true;
                }
            }
        }
    }
}