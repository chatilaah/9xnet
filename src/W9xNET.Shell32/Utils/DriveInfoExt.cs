using System.IO;

namespace W9xNET.User32.Utils
{
    public static class DriveInfoExt
    {
        /// <summary>
        /// Gets the classical naming scheme of a device.
        /// E.g. C:\ will be represented with (C:) instead.
        /// </summary>
        /// <param name="di"></param>
        /// <returns></returns>
        public static string ClassicalName(this DriveInfo di) => $"({di.Name.Substring(0, 2)})";
    }
}
