using LegalMitigation.Types;
using System.IO;

namespace LegalMitigation.Utils
{
    public class WinSource
    {
        #region Properties

        /// <summary>
        /// Path to the sources folder of the Windows installation.
        /// Unlike later versions of Windows, the installation files are typically located in win9x (win95 or win98) directory from the root level.
        /// </summary>
        public readonly string SourcePath;

        /// <summary>
        /// The type of the installation source.
        /// </summary>
        public readonly WinSourceType Type;

        protected virtual string[] InstallationFiles { get; }

        protected virtual string RelativeDirectory { get; }

        #endregion

        public WinSource(string sourcePath, WinSourceType type)
        {
            SourcePath = sourcePath;
            Type = type;
        }

        private WinSource CreateInstanceWithPath(string path)
        {
            switch (Type)
            {
                case WinSourceType.Win95:
                    return new WinSource95(path);
                case WinSourceType.Win98:
                    return new WinSource98(path);
                case WinSourceType.WinMe:
                    return new WinSourceMe(path);
            }

            return new WinSourceNull(path);
        }

        internal static WinSource DetectSourceByPath(string path)
        {
            WinSource[] winSources = new WinSource[] {
                new WinSource95(),
                new WinSource98(),
                new WinSourceMe()
            };

            foreach (var source in winSources)
            {
                var dir = new DirectoryInfo(path);
                var isOk = false;
                //List<FileInfo> files = new List<FileInfo>();

                foreach (var filename in source.InstallationFiles)
                {
                    if (!File.Exists(Path.Combine(dir.FullName, filename)))
                    {
                        continue;
                    }

                    //TODO: Not the most efficient way to search, but hey, it gets the job done for now.
                    var found = dir.GetFiles(filename, SearchOption.AllDirectories);
                    if (!(isOk = found.Length > 0)) break;
                }

                if (isOk)
                {
                    return source.CreateInstanceWithPath(Path.Combine(path, source.RelativeDirectory));
                }
            }

            return new WinSourceNull();
        }
    }
}