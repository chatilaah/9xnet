using System.Collections;

namespace W9xNET.Explorer.Controls
{
    internal class TaskbarWindowEntryCollection : IList<TaskbarWindowEntry>, ICollection<TaskbarWindowEntry>, IEnumerable
    {
        private readonly List<TaskbarWindowEntry> _items = new();

        public TaskbarWindowEntry this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(TaskbarWindowEntry item) => _items.Add(item);

        public void Add(Icon icon, string text) => _items.Add(new TaskbarWindowEntry { Icon = icon, Text = text });

        public void Clear() => _items.Clear();

        public bool Contains(TaskbarWindowEntry item) => _items.Contains(item);

        public void CopyTo(TaskbarWindowEntry[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<TaskbarWindowEntry> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(TaskbarWindowEntry item) => _items.IndexOf(item);

        public void Insert(int index, TaskbarWindowEntry item) => _items.Insert(index, item);

        public bool Remove(TaskbarWindowEntry item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}