using System.Collections.Generic;

namespace Homework
{
    public class Storage
    {
        private Dictionary<ItemType, int> _items = new();
        private int _maxSize;

        public Storage(int maxSize)
        {
            _maxSize = maxSize;
        }

        public void AddItem(ItemType item, int count)
        {
            _items.Add(item, count);
        }

        public int GetItemCount(ItemType item)
        {
            return _items[item];
        }

        public int Count()
        {
            var result = 0;

            foreach (var item in _items)
            {
                result += item.Value;
            }

            return result;
        }
    }
}