using System.Drawing;
using System.IO;

namespace W9xNET.Shell32.Utils
{
    public static class DriveTypeExt
    {
        public static Icon ToIcon(this DriveType driveType)
        {
            switch (driveType)
            {
                case DriveType.Unknown:
                case DriveType.NoRootDirectory:
                    return Program.Instance.RC!.IconAt(0);
                case DriveType.Removable:
                    return Program.Instance.RC!.IconAt(7);
                case DriveType.Fixed:
                    return Program.Instance.RC!.IconAt(8);
                case DriveType.Network:
                    return Program.Instance.RC!.IconAt(9);
                case DriveType.CDRom:
                    return Program.Instance.RC!.IconAt(11);
                case DriveType.Ram:
                    return Program.Instance.RC!.IconAt(12);
            }

            return null;
        }
    }
}