using LegalMitigation.Types;

namespace LegalMitigation.Interfaces
{
    internal interface IAssetLoader
    {
        /// <summary>
        /// Loads the resources from the embedded resources of the library/application or from an external source.
        /// </summary>
        /// <param name="filename">(Optional) The legal way of loading the resources when the application is distributed to the end-user.</param>
        string Init(string filename, WinSourceType sourceType = WinSourceType.Win95);
    }
}