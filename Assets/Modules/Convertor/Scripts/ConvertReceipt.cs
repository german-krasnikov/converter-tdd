namespace Modules.Converter
{
    public class ConvertReceipt
    {
        private ItemType _sourceType;
        private ItemType _targetType;
        private int _sourceCount;
        private int _targetCount;
        private float _time;

        public ItemType SourceType => _sourceType;
        public ItemType TargetType => _targetType;
        public int SourceCount => _sourceCount;
        public int TargetCount => _targetCount;
        public float Time => _time;

        public ConvertReceipt(ItemType sourceType, ItemType targetType, int sourceCount, int targetCount, float time)
        {
            _sourceType = sourceType;
            _targetType = targetType;
            _sourceCount = sourceCount;
            _targetCount = targetCount;
            _time = time;
        }
    }
}