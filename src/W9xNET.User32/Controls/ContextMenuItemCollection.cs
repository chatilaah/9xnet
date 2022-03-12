using System.Collections;

namespace W9xNET.User32.Controls
{
    public class ContextMenuItemCollection : IList<ContextMenuItem>, ICollection<ContextMenuItem>, IEnumerable
    {
        private readonly List<ContextMenuItem> _items = new();

        public ContextMenuItem this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(ContextMenuItem item) => _items.Add(item);

        public void Add(string text, int id = 0) => _items.Add(new ContextMenuItem { Text = text, Name = $"{id}" });

        public void Add(Icon icon, string text) => _items.Add(new ContextMenuItem
        {
            Text = text,
            Icon = icon
        });

        public void AddSeparator() => _items.Add(new SeparatorMenuItem());

        public void Clear() => _items.Clear();

        public bool Contains(ContextMenuItem item) => _items.Contains(item);

        public void CopyTo(ContextMenuItem[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<ContextMenuItem> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(ContextMenuItem item) => _items.IndexOf(item);

        public void Insert(int index, ContextMenuItem item) => _items.Insert(index, item);

        public bool Remove(ContextMenuItem item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}