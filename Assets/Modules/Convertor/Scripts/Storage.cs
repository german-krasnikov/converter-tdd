using System;
using System.Collections.Generic;

namespace Modules.Converter
{
    public class Storage
    {
        public int MaxSize => _maxSize;

        private Dictionary<ItemType, int> _items = new();
        private int _maxSize;

        public Storage(int maxSize)
        {
            _maxSize = maxSize;
        }

        public int AddItem(ItemType item, int count)
        {
            if (count < 0) throw new ArgumentException();
            var addedCount = Math.Min(count, _maxSize - GetItemCount(item) - Count());
            _items.Add(item, addedCount);
            return count - addedCount;
        }

        public int GetItemCount(ItemType item)
        {
            _items.TryGetValue(item, out var result);
            return result;
        }

        public bool RemoveItem(ItemType item, int removeCount)
        {
            if (removeCount < 0) throw new ArgumentException();
            var count = GetItemCount(item);
            if (removeCount > count) return false;
            _items[item] -= removeCount;
            return true;
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