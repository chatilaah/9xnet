using LegalMitigation;

namespace W9xNET.Explorer.Types
{
    internal enum ControlPanelItemType
    {
        //AccessibilityOptions,
        //AddNewHardware,
        AddOrRemovePrograms,
        DateOrTime,
        Display,
        //Fonts,
        //Keyboard,
        //Mouse,
        //Multimedia,
        //Network,
        //Passwords,
        //Power,
        //Printers,
        //RegionalSettings,
        Sounds,
        System
    }

    /// <summary>
    /// Extension class for the ControlPanelItemType enumeration
    /// </summary>
    internal static class ControlPanelItemTypeExtensions
    {
        /// <summary>
        /// Retrieves the appropriate instance of the specified ControlPanelItemType.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An instance to a form</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static W9xNETApp GetCpl(this ControlPanelItemType item)
        {
            //switch (item)
            //{
            //    case ControlPanelItemType.AddOrRemovePrograms:
            //        return new W9xNET.AppwizCpl.Program();
            //    case ControlPanelItemType.System:
            //        return new W9xNET.SysdmCpl.Program();
            //    case ControlPanelItemType.Display:
            //        return new W9xNET.DeskCpl.Program();
            //    case ControlPanelItemType.DateOrTime:
            //        return new W9xNET.TimedateCpl.Program();
            //    case ControlPanelItemType.Sounds:
            //        return new W9xNET.MmsysCpl.Program();
            //}

            throw new NotImplementedException($"The feature \"{item.ToString()}\" was not implemented yet!");
        }
    }
}