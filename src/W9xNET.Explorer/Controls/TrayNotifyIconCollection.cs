using System.Collections;

namespace W9xNET.Explorer.Controls
{
    internal class TrayNotifyIconCollection : IList<SysTrayIcon>, ICollection<SysTrayIcon>, IEnumerable
    {
        private readonly List<SysTrayIcon> _items = new();

        public SysTrayIcon this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(SysTrayIcon item) => _items.Add(item);

        public void Add(Icon icon, string text) => _items.Add(new SysTrayIcon { Icon = icon, Text = text });

        public void Clear() => _items.Clear();

        public bool Contains(SysTrayIcon item) => _items.Contains(item);

        public void CopyTo(SysTrayIcon[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<SysTrayIcon> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(SysTrayIcon item) => _items.IndexOf(item);

        public void Insert(int index, SysTrayIcon item) => _items.Insert(index, item);

        public bool Remove(SysTrayIcon item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}