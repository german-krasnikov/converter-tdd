namespace Modules.Converter
{
    public class ItemUnit
    {
        private ItemType _type;
        private int _count;

        public ItemType Type => _type;
        public int Count => _count;

        public ItemUnit(ItemType type, int count)
        {
            _type = type;
            _count = count;
        }
    }
}