using DWORD = System.Int32;
using WORD = System.Int16;

namespace W9xNET.RcReader.Data
{
    public class DialogControlElement
    {
        public DialogControlElement(DWORD style, object caption, object clazz, short x, short y, short width, short height, WORD id)
        {
            Style = style;
            Caption = caption;
            Class = clazz;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            ID = id;
        }

        protected virtual double Spacing => 1.6;

        public readonly DWORD Style;
        public readonly object Caption;
        public readonly object Class;
        protected readonly short _y;
        protected readonly short _x;
        protected readonly short _width;
        protected readonly short _height;
        public readonly WORD ID;

        public int X => (int)(_x * Spacing);

        public int Y => (int)(_y * Spacing);

        public int Width => (int)(_width * Spacing);

        public int Height => (int)(_height * Spacing);

        public T GetCaption<T>() => (T)Caption;

        public T GetStyle<T>() => (T)System.Enum.ToObject(typeof(T), Style);

        public string IDToPredefClass()
        {
            if (Class.GetType() == typeof(System.String))
            {
                return Class.ToString() ?? String.Empty;
            }

            switch (Convert.ToUInt16(Class))
            {
                case 0x0080:
                    return "BUTTON";
                case 0x0081:
                    return "EDIT";
                case 0x0082:
                    return "STATIC";
                case 0x0083:
                    return "LISTBOX";
                case 0x0084:
                    return "SCROLLBAR";
                case 0x0085:
                    return "COMBOBOX";
            }

            return String.Empty;
        }
    }
}