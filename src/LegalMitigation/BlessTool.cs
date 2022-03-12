using LegalMitigation.Types;
using LegalMitigation.Utils;
using System.Diagnostics;

namespace LegalMitigation
{
    internal class BlessTool
    {
        #region Delegate(s)

        public delegate void OnCheckingDrivesCallback();

        public delegate void OnDrivesDetectedCallback(IEnumerable<DriveInfo> drives);

        public delegate void OnSearchingSourceCallback(string path);

        public delegate int OnSelectWinSourceCallback(IEnumerable<WinSource> sources);

        #endregion

        #region Events

        /// <summary>
        /// A callback event that fires when drives are being checked on the system.
        /// </summary>
        public event OnCheckingDrivesCallback OnCheckingDrives;

        /// <summary>
        /// A callback event that fires when one or more drives were detected.
        /// </summary>
        public event OnDrivesDetectedCallback OnDrivesDetected;

        /// <summary>
        /// A callback event that fires when a Windows installation source is being looked up.
        /// </summary>
        public event OnSearchingSourceCallback OnSearchingSource;

        /// <summary>
        /// A callback event that fires when a WinSource was selected (conveniently, from a user's choice)
        /// </summary>
        public event OnSelectWinSourceCallback OnSelectWinSource;

        #endregion

        public BlessToolError Resolve(W9xNETApp[] apps)
        {
            Debug.Assert(OnCheckingDrives != null && OnDrivesDetected != null && OnSearchingSource != null && OnSelectWinSource != null, "Callback handlers were not properly set.");
            OnCheckingDrives.Invoke();

            // Filter the CD/DVD drives
            var cdRoms = DriveInfo.GetDrives().Where(x => x.DriveType == DriveType.CDRom);
            OnDrivesDetected.Invoke(cdRoms);

            if (cdRoms.Count() == 0)
            {
                return BlessToolError.DrivesNotFound;
            }

            // Sources found.
            // We could have more than one source...
            var sources = new List<WinSource>();

            // Detect the sources
            foreach (var drive in cdRoms)
            {
                if (!drive.IsReady)
                {
                    continue;
                }

                var current = WinSource.DetectSourceByPath(drive.RootDirectory.FullName);
                if (current.Type != WinSourceType.Unknown)
                {
                    sources.Add(current);
                }
            }

            if (sources.Count == 0)
            {
                return BlessToolError.SourcesNotFound;
            }

            int selectedSource = sources.Count > 1 ? -1 : 0;
            if (selectedSource == -1)
            {
                selectedSource = OnSelectWinSource.Invoke(sources);
            }

            if (selectedSource < 0 || selectedSource >= sources.Count)
            {
                return BlessToolError.OperationCancelled;
            }

            foreach (W9xNETApp i in apps)
            {
                var source = sources.ElementAt(selectedSource);
                var pathToCab = Path.Combine(source.SourcePath, i.GetCabFile(source.Type));

                if (!File.Exists(pathToCab))
                {
                    throw new FileNotFoundException($"The file \"{pathToCab}\" could not be found!");
                }

                i.LoadResourcesFromExternalSource(pathToCab, source.Type);
            }

            return BlessToolError.Success;
        }
    }
}