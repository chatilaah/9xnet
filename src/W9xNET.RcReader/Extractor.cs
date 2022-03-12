using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using W9xNET.RcFileReader.Extensions;
using W9xNET.RcReader.Data;
using W9xNET.RcReader.Enums;
using W9xNET.RcReader.Icons;
using static W9xNET.Interop.Win32.Kernel32;
using static W9xNET.Interop.Win32.User32;
using BYTE = System.Byte;
using WORD = System.UInt16;

namespace W9xNET.RcReader
{
    /// <summary>
    /// Get icon resources (RT_DIALOG) from an executable module (either a .dll or an .exe file).
    /// </summary>
    public class Extractor : IDisposable
    {
        #region Public Propreties
        private string _fileName;
        /// <summary>
        /// A fully quallified name of the executable module.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            private set { _fileName = value; }
        }

        private IntPtr _moduleHandle;
        /// <summary>
        /// Gets the module handle.
        /// </summary>
        public IntPtr ModuleHandle
        {
            get { return _moduleHandle; }
            private set { _moduleHandle = value; }
        }

        private List<ResourceName> _groupIconList;
        private List<ResourceName> _dialogNamesList;
        private List<ResourceName> _menuList;
        private List<ResourceName> _bmpList;

        /// <summary>
        /// Gets a list of icons resource names RT_DIALOG
        /// </summary>
        public List<ResourceName> DialogNamesList
        {
            get => _dialogNamesList;
            private set => _dialogNamesList = value;
        }

        /// <summary>
        /// Gets a list of icons resource names RT_GROUP_ICON
        /// </summary>
        public List<ResourceName> GroupIconList
        {
            get => _groupIconList;
            private set => _groupIconList = value;
        }

        /// <summary>
        /// Gets a list of menues resource names RT_MENU
        /// </summary>
        public List<ResourceName> MenuList
        {
            get => _menuList;
            private set => _menuList = value;
        }

        /// <summary>
        /// Gets a list of menues resource names RT_BITMAP
        /// </summary>
        public List<ResourceName> BmpList
        {
            get => _bmpList;
            private set => _bmpList = value;
        }

        /// <summary>
        /// Gets number of RT_DIALOG found in the executable module.
        /// </summary>
        public int DialogCount => this.DialogNamesList.Count;

        /// <summary>
        /// Gets number of RT_GROUP_ICON found in the executable module.
        /// </summary>
        public int IconCount => this.GroupIconList.Count;

        /// <summary>
        /// Gets number of RT_MENU found in the executable module.
        /// </summary>
        public int MenuCount => this.MenuList.Count;

        /// <summary>
        /// Gets number of RT_BITMAP found in the executable module.
        /// </summary>
        public int BmpCount => this.BmpList.Count;

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the RT_GROUP_ICON cache.
        /// </summary>
        private Dictionary<int, Icon> IconCache { get; set; }

        /// <summary>
        /// Gets or sets the RT_BITMAP cache.
        /// </summary>
        private Dictionary<int, Bitmap> BmpCache { get; set; }

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new IconExtractor and loads the executable module into the address space of the calling process.
        /// The executable module can be a .dll or an .exe file.
        /// The specified module can cause other modules to be mapped into the address space.
        /// </summary>
        /// <param name="fileName">The name of the executable module (either a .dll or an .exe file). The file name can contain environment variables (like %SystemRoot%).</param>
        public Extractor(string fileName)
        {
            LoadLibrary(fileName);
        }

        /// <summary>
        /// Destructs the IconExtractor object.
        /// </summary>
        ~Extractor()
        {
            Dispose();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function maps a specified executable module into the address space of the calling process.
        /// The executable module can be a .dll or an .exe file.
        /// The specified module can cause other modules to be mapped into the address space.
        /// </summary>
        /// <param name="fileName">The name of the executable module (either a .dll or an .exe file). The file name can contain environment variables (like %SystemRoot%).</param>
        private void LoadLibrary(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            this.FileName = Environment.ExpandEnvironmentVariables(fileName);
            //Load the executable module into memory using LoadLibraryEx API.
            this.ModuleHandle = Interop.Win32.Kernel32.LoadLibraryEx(Environment.ExpandEnvironmentVariables(this.FileName), IntPtr.Zero, LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE);
            if (this.ModuleHandle == IntPtr.Zero)
            {
                int errorNum = Marshal.GetLastWin32Error();
                switch ((GetLastErrorResult)errorNum)
                {
                    case GetLastErrorResult.ERROR_FILE_NOT_FOUND:
                        throw new FileNotFoundException("File not found.", this.FileName);
                    case GetLastErrorResult.ERROR_BAD_EXE_FORMAT:
                        throw new ArgumentException("The file '" + this.FileName + "' is not a valid win32 executable or dll.");
                    default:
                        throw new Win32Exception(errorNum);
                }
            }

            this.DialogNamesList = new List<ResourceName>();
            this.GroupIconList = new List<ResourceName>();
            this.BmpList = new List<ResourceName>();
            this.MenuList = new List<ResourceName>();
            this.IconCache = new Dictionary<int, Icon>();
            this.BmpCache = new Dictionary<int, Bitmap>();

            //Enumurate the resource names of RT_DIALOG by calling EnumResourcesCallBack function for each resource of that type.
            Interop.Win32.Kernel32.EnumResourceNames(this.ModuleHandle, ResourceTypes.RT_GROUP_ICON, EnumResourcesCallBack, IntPtr.Zero);
            Interop.Win32.Kernel32.EnumResourceNames(this.ModuleHandle, ResourceTypes.RT_DIALOG, EnumResourcesCallBack, IntPtr.Zero);
            Interop.Win32.Kernel32.EnumResourceNames(this.ModuleHandle, ResourceTypes.RT_BITMAP, EnumResourcesCallBack, IntPtr.Zero);
            Interop.Win32.Kernel32.EnumResourceNames(this.ModuleHandle, ResourceTypes.RT_MENU, EnumResourcesCallBack, IntPtr.Zero);
        }

        /// <summary>
        /// The callback function that is being called for each resource (RT_GROUP_ICON, RT_ICON) in the executable module.
        /// The function stores the resource name of type RT_GROUP_ICON into the GroupIconsList and 
        /// stores the resource name of type RT_ICON into the IconsList.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="lpszType">Specifies the type of the resource being enumurated (RT_GROUP_ICON, RT_ICON).</param>
        /// <param name="lpszName">Specifies the name of the resource being enumurated. For more ifnormation, see the Remarks section.</param>
        /// <param name="lParam">Specifies the application defined parameter passed to the EnumResourceNames function.</param>
        /// <returns>This callback function return true to continue enumuration.</returns>
        /// <remarks>
        /// If the high bit of lpszName is not set (=0), lpszName specifies the integer identifier of the givin resource.
        /// Otherwise, it is a pointer to a null terminated string.
        /// If the first character of the string is a pound sign (#), the remaining characters represent a decimal number that specifies the integer identifier of the resource. For example, the string "#258" represents the identifier 258.
        /// #define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0)
        /// </remarks>
        private bool EnumResourcesCallBack(IntPtr hModule, ResourceTypes lpszType, IntPtr lpszName, IntPtr lParam)
        {
            switch (lpszType)
            {
                case ResourceTypes.RT_DIALOG:
                    this.DialogNamesList.Add(new ResourceName(lpszName));
                    break;
                case ResourceTypes.RT_GROUP_ICON:
                    this.GroupIconList.Add(new ResourceName(lpszName));
                    break;
                case ResourceTypes.RT_MENU:
                    this.MenuList.Add(new ResourceName(lpszName));
                    break;
                case ResourceTypes.RT_BITMAP:
                    this.BmpList.Add(new ResourceName(lpszName));
                    break;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Gets a System.Drawing.Bitmap that represents RT_BITMAP at the givin index from the executable module.
        /// </summary>
        /// <param name="index">The index of the RT_BITMAP in the executable module.</param>
        /// <returns>Returns System.Drawing.Bitmap.</returns>
        private Bitmap GetBmpFromLib(int index)
        {
            byte[] rc = GetResourceData(this.ModuleHandle, this.BmpList[index], ResourceTypes.RT_BITMAP);

            List<byte> b = new();
            //TODO: HACK, adding the header of the BMP manually...
            b.AddRange(new byte[] { 66, 77, 246, 0, 0, 0, 0, 0, 0, 0, 118, 0, 0, 0 });
            b.AddRange(rc);

            // Convert the resouce into an .bmp file image.
            using MemoryStream inputStream = new(b.ToArray());
            return new Bitmap(inputStream);
        }

        /// <summary>
        /// Gets a System.Drawing.Icon that represents RT_GROUP_ICON at the givin index from the executable module.
        /// </summary>
        /// <param name="index">The index of the RT_GROUP_ICON in the executable module.</param>
        /// <returns>Returns System.Drawing.Icon.</returns>
        private Icon GetIconFromLib(int index)
        {
            byte[] rc = GetResourceData(this.ModuleHandle, this.GroupIconList[index], ResourceTypes.RT_GROUP_ICON);

            // Convert the resouce into an .ico file image.
            using MemoryStream inputStream = new(rc);
            using MemoryStream destStream = new();

            // Read the GroupIconDir header.
            GroupIconDir grpDir = Utility.ReadStructure<GroupIconDir>(inputStream);

            int numEntries = grpDir.Count;
            int iconImageOffset = IconInfo.SizeOfIconDir + numEntries * IconInfo.SizeOfIconDirEntry;

            // Write the IconDir header.
            Utility.WriteStructure<IconDir>(destStream, grpDir.ToIconDir());
            for (int i = 0; i < numEntries; i++)
            {
                // Read the GroupIconDirEntry.
                GroupIconDirEntry grpEntry = Utility.ReadStructure<GroupIconDirEntry>(inputStream);

                // Write the IconDirEntry.
                destStream.Seek(IconInfo.SizeOfIconDir + i * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
                Utility.WriteStructure<IconDirEntry>(destStream, grpEntry.ToIconDirEntry(iconImageOffset));

                // Get the icon image raw data and write it to the stream.
                byte[] imgBuf = GetResourceData(this.ModuleHandle, grpEntry.ID, ResourceTypes.RT_ICON);
                destStream.Seek(iconImageOffset, SeekOrigin.Begin);
                destStream.Write(imgBuf, 0, imgBuf.Length);

                // Append the iconImageOffset.
                iconImageOffset += imgBuf.Length;
            }

            destStream.Seek(0, SeekOrigin.Begin);
            return new Icon(destStream);
        }

        private List<ExMenuItem> GetMenuFromLib(int index)
        {
            byte[] rc = GetResourceData(this.ModuleHandle, this.MenuList[index], ResourceTypes.RT_MENU);

            using MemoryStream inputStream = new(rc);
            BinaryReader br = new(inputStream, Encoding.ASCII);

            // Read the menu parameters
            var header = Utility.ReadStructure<MENUEX_TEMPLATE_HEADER>(inputStream);

            List<ExMenuItem> exitems = new();

            const long MF_POPUP = 0x00000010L; // from WinUser.h
            const long MF_END = 0x00000080L; // from WinUser.h

            if (header.wVersion == 1)
            {
                if (header.wVersion != 1 || header.wOffset < 4)
                {
                    // should return here.
                    Debugger.Break();
                }

                inputStream.Seek(4 + header.wOffset, SeekOrigin.Begin);
                inputStream.ReadDwordAlignment();

                Stack<bool> flag_stack = new();
                flag_stack.Push(true);

                short wDepth = 0;
                MENUEX_TEMPLATE_ITEM_HEADER item_header;

                inputStream.ReadDwordAlignment();

                while (inputStream.Position < inputStream.Length)
                {
                    item_header = Utility.ReadStructure<MENUEX_TEMPLATE_ITEM_HEADER>(inputStream);

                    ExMenuItem exitem = new();

                    if (!inputStream.ReadSz(ref exitem.Text)) break;

                    if ((item_header.bResInfo & 0x01) == 1)
                    {
                        flag_stack.Push(!((item_header.bResInfo & 0x80) == 0));

                        inputStream.ReadDwordAlignment();
                        var c = Utility.ReadStructure<System.Int32>(inputStream);

                        exitem.DwType = item_header.dwType;
                        exitem.DwState = item_header.dwState;
                        exitem.MenuId = item_header.menuId;
                        exitem.bResInfo = item_header.bResInfo;
                        exitem.wDepth = wDepth++;
                        exitems.Add(exitem);
                    }
                    else
                    {
                        exitem.DwHelpId = 0;
                        exitem.DwType = item_header.dwType;
                        exitem.DwState = item_header.dwState;
                        exitem.MenuId = item_header.menuId;
                        exitem.bResInfo = item_header.bResInfo;
                        exitem.wDepth = wDepth;
                        exitems.Add(exitem);

                        if ((item_header.bResInfo & 0x80) == 0x80)
                        {
                            --wDepth;
                            while (flag_stack.Count > 0 && flag_stack.Peek())
                            {
                                flag_stack.Pop();
                                if (wDepth == 0)
                                    break;
                                --wDepth;
                            }
                            if (flag_stack.Count == 0)
                                break;
                        }
                    }

                    inputStream.ReadDwordAlignment();
                }
            }
            else
            {
                inputStream.Position = 0;

                MENUHEADER hd2 = Utility.ReadStructure<MENUHEADER>(inputStream);
                if (hd2.wVersion != 0 || hd2.cbHeaderSize != 0)
                {
                    return exitems;
                }

                //List<MenuItem> items = new(); // not used on the 9xNET environment

                Stack<bool> flag_stack = new();
                flag_stack.Push(true);

                short wDepth = 0, fItemFlags = 0;
                while (inputStream.Position < inputStream.Length)
                {
                    fItemFlags = inputStream.PeekWord();

                    ExMenuItem item = new();
                    
                    var cf = fItemFlags & MF_POPUP;
                    if (cf == MF_POPUP)
                    {
                        flag_stack.Push(!((fItemFlags & MF_END) == 0));

                        POPUPMENUITEMHEAD head = Utility.ReadStructure<POPUPMENUITEMHEAD>(inputStream);
                        inputStream.ReadSz(ref item.Text);

                        //item.fItemFlags = fItemFlags; // Not used on the 9xNET environment
                        item.MenuId = 0;
                        item.wDepth = wDepth++;
                        exitems.Add(item);
                    }
                    else
                    {
                        NORMALMENUITEMHEAD head = Utility.ReadStructure<NORMALMENUITEMHEAD>(inputStream);
                        inputStream.ReadSz(ref item.Text);

                        //item.fItemFlags = fItemFlags; // Not used on the 9xNET environment
                        item.MenuId = head.wMenuID;
                        item.wDepth = wDepth;
                        exitems.Add(item);

                        cf = fItemFlags & MF_END;
                        if (cf == MF_END)
                        {
                            --wDepth;
                            while (flag_stack.Count > 0 && flag_stack.Peek())
                            {
                                flag_stack.Pop();
                                if (wDepth == 0)
                                    break;
                                --wDepth;
                            }
                            if (flag_stack.Count == 0)
                                break;
                        }
                    }
                }
            }

            return exitems;
        }

        /// <summary>
        /// Gets a W9xNET.RcReader.Data.DialogInfo that represents RT_DIALOG at the givin index from the executable module.
        /// </summary>
        /// <param name="index">The index of the RT_DIALOG in the executable module.</param>
        /// <returns>Returns W9xNET.RcReader.Data.DialogInfo.</returns>
        private DialogInfo GetDialogFromLib(int index)
        {
            byte[] rc = GetResourceData(this.ModuleHandle, this.DialogNamesList[index], ResourceTypes.RT_DIALOG);

            using MemoryStream inputStream = new(rc);
            BinaryReader br = new(inputStream, Encoding.Unicode);

            // Read the dialog layout parameters
            var dlgTemplate = Utility.ReadStructure<DLGTEMPLATE>(inputStream);

            var x2 = (WindowStyleFlags)dlgTemplate.Style;

            // Read the menu ID
            WORD hMenu = 0;
            if (br.PeekChar() != 0 || br.PeekChar() == 0xFFFF)
            {
                unsafe
                {
                    inputStream.Position += sizeof(WORD);
                }


                hMenu = br.ReadUInt16();
            }

            inputStream.Position = (hMenu == 0) ?
                0x16 : 0x18;

            // Read the dialog caption
            var dialogTitle = br.GetString();

            // Read the dialog font information
            var fontSize = br.ReadInt16();
            var fontFamily = br.GetString();

            //
            // Grab controls
            //
            var controls = new List<DialogControlElement>();

            for (int i = 0; i < dlgTemplate.Cdit; i++)
            {
                inputStream.ReadDwordAlignment();

                var control = Utility.ReadStructure<DLGITEMTEMPLATE>(inputStream);

                object clazz = br.ReadObject();
                object title = br.ReadObject();

                BYTE b = br.ReadByte();
                if (b > 0)
                {
                    throw new NotImplementedException();
                }

                controls.Add(new DialogControlElement(
                    control.Style,
                    title, clazz,
                    control.X,
                    control.Y,
                    control.Cx,
                    control.Cy,
                    control.ID
                ));
            }

            return new DialogInfo(
                dlgTemplate.Style,
                dlgTemplate.X,
                dlgTemplate.Y,
                dlgTemplate.Cx,
                dlgTemplate.Cy,
                hMenu,
                dialogTitle,
                fontFamily,
                fontSize,
                controls,
                rc);
        }

        /// <summary>
        /// Extracts the raw data of the resource from the module.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="resrouceName">The name of the resource.</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>The resource raw data.</returns>
        private static byte[] GetResourceData(IntPtr hModule, ResourceName resourceName, ResourceTypes resourceType)
        {
            //Find the resource in the module.
            IntPtr hResInfo = IntPtr.Zero;
            try { hResInfo = Interop.Win32.Kernel32.FindResource(hModule, resourceName.Value, resourceType); }
            finally { resourceName.Dispose(); }
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Load the resource.
            IntPtr hResData = Interop.Win32.Kernel32.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Lock the resource to read data.
            IntPtr hGlobal = Interop.Win32.Kernel32.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Get the resource size.
            int resSize = Interop.Win32.Kernel32.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }
            //Allocate the requested size.
            byte[] buf = new byte[resSize];
            //Copy the resource data into our buffer.
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }

        /// <summary>
        /// Extracts the raw data of the resource from the module.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="resrouceName">The identifier of the resource.</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>The resource raw data.</returns>
        private static byte[] GetResourceData(IntPtr hModule, int resourceId, ResourceTypes resourceType)
        {
            //Find the resource in the module.
            IntPtr hResInfo = Interop.Win32.Kernel32.FindResource(hModule, (IntPtr)resourceId, resourceType);
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Load the resource.
            IntPtr hResData = Interop.Win32.Kernel32.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Lock the resource to read data.
            IntPtr hGlobal = Interop.Win32.Kernel32.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Get the resource size.
            int resSize = Interop.Win32.Kernel32.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }
            //Allocate the requested size.
            byte[] buf = new byte[resSize];
            //Copy the resource data into our buffer.
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a System.Drawing.Bitmap that represents RT_BITMAP at the givin index.
        /// </summary>
        /// <param name="index">The index of the RT_BITMAP in the executable module.</param>
        /// <returns>Returns System.Drawing.Bitmap.</returns>
        public Bitmap GetBmpAt(int index)
        {
            if (index < 0 || index >= this.BmpCount)
            {
                if (this.IconCount > 0)
                    throw new ArgumentOutOfRangeException("index", index, "Index should be in the range (0-" + this.BmpCount.ToString() + ").");
                else
                    throw new ArgumentOutOfRangeException("index", index, "No bitmaps in the list.");
            }

            if (!this.BmpCache.ContainsKey(index))
                this.BmpCache[index] = GetBmpFromLib(index);

            return this.BmpCache[index];
        }

        /// <summary>
        /// Gets a System.Drawing.Icon that represents RT_GROUP_ICON at the givin index.
        /// </summary>
        /// <param name="index">The index of the RT_GROUP_ICON in the executable module.</param>
        /// <returns>Returns System.Drawing.Icon.</returns>
        public Icon GetIconAt(int index)
        {
            if (index < 0 || index >= this.IconCount)
            {
                if (this.IconCount > 0)
                    throw new ArgumentOutOfRangeException("index", index, "Index should be in the range (0-" + this.IconCount.ToString() + ").");
                else
                    throw new ArgumentOutOfRangeException("index", index, "No icons in the list.");
            }

            if (!this.IconCache.ContainsKey(index))
                this.IconCache[index] = GetIconFromLib(index);

            return this.IconCache[index];
        }

        /// <summary>
        /// Gets a System.Drawing.Icon that represents RT_GROUP_ICON at the givin index.
        /// </summary>
        /// <param name="index">The index of the RT_GROUP_ICON in the executable module.</param>
        /// <returns>Returns System.Drawing.Icon.</returns>
        public DialogInfo GetDialogAt(int index)
        {
            if (index < 0 || index >= this.DialogCount)
            {
                if (this.DialogCount > 0)
                    throw new ArgumentOutOfRangeException("index", index, "Index should be in the range (0-" + this.DialogCount.ToString() + ").");
                else
                    throw new ArgumentOutOfRangeException("index", index, "No dialogs in the list.");
            }

            return GetDialogFromLib(index);
        }

        public List<ExMenuItem> GetMenuAt(int index)
        {
            if (index < 0 || index >= this.MenuCount)
            {
                if (this.MenuCount > 0)
                    throw new ArgumentOutOfRangeException("index", index, "Index should be in the range (0-" + this.MenuCount.ToString() + ").");
                else
                    throw new ArgumentOutOfRangeException("index", index, "No menues in the list.");
            }

            return GetMenuFromLib(index);
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases the resources of that object.
        /// </summary>
        public void Dispose()
        {
            if (this.ModuleHandle != IntPtr.Zero)
            {
                try { Interop.Win32.Kernel32.FreeLibrary(this.ModuleHandle); }
                catch { }
                this.ModuleHandle = IntPtr.Zero;
            }
            if (this.DialogNamesList != null)
                this.DialogNamesList.Clear();
        }
        #endregion
    }
}