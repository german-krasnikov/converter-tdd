namespace Modules.Converter
{
    public class ConvertReceipt
    {
        private ItemType _sourceType;
        private ItemType _convertType;
        private int _sourceCount;
        private int _convertCount;
        private float _time;

        public ItemType SourceType => _sourceType;
        public ItemType ConvertType => _convertType;
        public int SourceCount => _sourceCount;
        public int ConvertCount => _convertCount;
        public float Time => _time;

        public ConvertReceipt(ItemType sourceType, ItemType convertType, int sourceCount, int convertCount, float time)
        {
            _sourceType = sourceType;
            _convertType = convertType;
            _sourceCount = sourceCount;
            _convertCount = convertCount;
            _time = time;
        }
    }
}