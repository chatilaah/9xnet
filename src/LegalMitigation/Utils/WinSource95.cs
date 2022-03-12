namespace LegalMitigation.Utils
{
    /// <summary>
    /// Microsoft(R) Windows(R) 95 sources
    /// </summary>
    internal sealed class WinSource95 : WinSource
    {
        public WinSource95(string path = "") : base(path, Types.WinSourceType.Win95)
        {
            // do nothing.
        }

        protected override string RelativeDirectory => "WIN95";

        /// <summary>
        /// The expected Windows 95 installation files.
        /// </summary>
        protected override string[] InstallationFiles => new string[]
        {
            $"{RelativeDirectory}\\FORMAT.COM",
            $"{RelativeDirectory}\\MINI.CAB",
            $"{RelativeDirectory}\\MSINFO.INF",
            $"{RelativeDirectory}\\OEMSETUP.BIN",
            $"{RelativeDirectory}\\OEMSETUP.EXE",
            $"{RelativeDirectory}\\PRECOPY1.CAB",
            $"{RelativeDirectory}\\PRECOPY2.CAB",
            $"{RelativeDirectory}\\SETUP25I.EXE",
            $"{RelativeDirectory}\\SETUP32.EXE",
            $"{RelativeDirectory}\\SUHELPER.BIN",
            $"{RelativeDirectory}\\SWINST4.EXE",
            $"{RelativeDirectory}\\deltemp.com",
            $"{RelativeDirectory}\\dossetup.bin",
            $"{RelativeDirectory}\\extract.exe",
            $"{RelativeDirectory}\\save32.com",
            $"{RelativeDirectory}\\scandisk.exe",
            $"{RelativeDirectory}\\scandisk.pif",
            $"{RelativeDirectory}\\scanprog.exe",
            $"{RelativeDirectory}\\setup.exe",
            $"{RelativeDirectory}\\smartdrv.exe",
            $"{RelativeDirectory}\\wb16off.exe",
            $"{RelativeDirectory}\\win95_02.cab",
            $"{RelativeDirectory}\\win95_03.cab",
            $"{RelativeDirectory}\\win95_04.cab",
            $"{RelativeDirectory}\\win95_05.cab",
            $"{RelativeDirectory}\\win95_06.cab",
            $"{RelativeDirectory}\\win95_07.cab",
            $"{RelativeDirectory}\\win95_08.cab",
            $"{RelativeDirectory}\\win95_09.cab",
            $"{RelativeDirectory}\\win95_10.cab",
            $"{RelativeDirectory}\\win95_11.cab",
            $"{RelativeDirectory}\\win95_12.cab",
            $"{RelativeDirectory}\\win95_13.cab",
            $"{RelativeDirectory}\\win95_14.cab",
            $"{RelativeDirectory}\\win95_15.cab",
            $"{RelativeDirectory}\\win95_16.cab",
            $"{RelativeDirectory}\\win95_17.cab",
            $"{RelativeDirectory}\\win95_18.cab",
            $"{RelativeDirectory}\\win95_19.cab",
            $"{RelativeDirectory}\\win95_20.cab",
            $"{RelativeDirectory}\\win95_21.cab",
            $"{RelativeDirectory}\\win95_22.cab",
            $"{RelativeDirectory}\\win95_23.cab",
            $"{RelativeDirectory}\\win95_24.cab",
            $"{RelativeDirectory}\\win95_25.cab",
            $"{RelativeDirectory}\\win95_26.cab",
            $"{RelativeDirectory}\\win95_27.cab",
            $"{RelativeDirectory}\\win95_28.cab",
            $"{RelativeDirectory}\\winsetup.bin",
            $"{RelativeDirectory}\\xmsmmgr.exe"
        };
    }
}