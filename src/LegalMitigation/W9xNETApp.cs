using LegalMitigation.Types;
using System.Reflection;
using W9xNET.Cabinet;

namespace LegalMitigation
{
    public abstract class W9xNETApp
    {
        #region Private Properties

        private bool IsInitResources => !string.IsNullOrEmpty(DstPath);

        private readonly Dictionary<WinSourceType, string> CFMap;
        private string DstPath = "";

        private const string CompanyName = "Ahmad N. Chatila";

        #endregion

        #region Public Properties

        /// <summary>
        /// The targetted resource's file simple name.
        /// The file could be a DLL or EXE.
        /// </summary>
        public readonly string KeyFile;

        #endregion

        #region Protected Properties

        /// <summary>
        /// The Win32 RC object.
        /// </summary>
        public RcObject? RC { get; private set; }

        protected string GetFilenameFromRoot(string filename) => Path.Combine(DstPath, filename);

        /// <summary>
        /// The instances that are loaded upon startup.
        /// </summary>
        protected static Dictionary<string, W9xNETApp> m_instances = new();

        #endregion

        #region Constructor(s)

        public W9xNETApp(string key, Dictionary<WinSourceType, string> cfMap)
        {
            CFMap = cfMap;
            KeyFile = key;
        }

        #endregion

        /// <summary>
        /// Gets the .CAB file that contains the required file.
        /// </summary>
        /// <param name="winSourceType"></param>
        /// <returns></returns>
        public string GetCabFile(WinSourceType winSourceType) => CFMap[winSourceType];

        /// <summary>
        /// A rundll-related implementation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public abstract void OnInvoke(dynamic sender, params string[] args);

        /// <summary>
        /// Loads resources for the asset object.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="sourceType"></param>
        /// <returns>The key file to be used to extract resources from.</returns>
        public string LoadResourcesFromExternalSource(string filename, WinSourceType sourceType)
        {
            if (IsInitResources)
            {
                throw new System.Exception("Already loaded resources from an external source.");
            }

            var lmDll = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                CompanyName,
                LegalMitigation.Properties.Resources.AppName,
                Path.GetFileName(Assembly.GetExecutingAssembly().Location)
                );

            DstPath = $"{lmDll}.{sourceType.ToStringAlt()}";

            RC ??= new RcObject(GetFilenameFromRoot(KeyFile));

            if (!Directory.Exists(DstPath))
                Directory.CreateDirectory(DstPath);

            var keyFileAsPath = Path.Combine(DstPath, KeyFile);
            if (!File.Exists(keyFileAsPath))
            {
                using var cabFile = new CabExtractor(filename, DstPath);
                cabFile.ExtractFile(KeyFile);
            }

            m_instances[KeyFile] = this;
            return keyFileAsPath;
        }

        /// <summary>
        /// Display icon of the application
        /// </summary>
        public abstract Icon? DisplayIcon { get; }

        /// <summary>
        /// Display name of the application
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Entry point to the window
        /// </summary>
        public abstract Form? FormEntry { get; }
    }
}