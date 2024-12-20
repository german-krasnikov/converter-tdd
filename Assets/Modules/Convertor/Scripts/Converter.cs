using System;

namespace Modules.Converter
{
    /**
       Конвертер представляет собой преобразователь ресурсов, который берет ресурсы
       из зоны погрузки (справа) и через несколько секунд преобразовывает его в
       ресурсы другого типа (слева).

       Конвертер работает автоматически. Когда заканчивается цикл переработки
       ресурсов, то конвертер берет следующую партию и начинает цикл по новой, пока
       можно брать ресурсы из зоны загрузки или пока есть место для ресурсов выгрузки.

       Также конвертер можно выключать. Если конвертер во время работы был
       выключен, то он возвращает обратно ресурсы в зону загрузки. Если в это время
       были добавлены еще ресурсы, то при переполнении возвращаемые ресурсы
       «сгорают».

       Характеристики конвертера:
       - Зона погрузки: вместимость бревен
       - Зона выгрузки: вместимость досок
       - Кол-во ресурсов, которое берется с зоны погрузки
       - Кол-во ресурсов, которое поставляется в зону выгрузки
       - Время преобразования ресурсов
       - Состояние: вкл/выкл
     */
    public sealed class Converter
    {
        public event Action<ItemType, int> OnSourceAdded;
        public event Action<ItemType, int> OnSourceRemoved;
        public event Action<ItemType, int> OnTargetRemoved;
        public event Action<ConvertReceipt> OnConverted;
        public event Action<ConvertReceipt> OnStartConverting;
        public event Action<ConvertReceipt> OnStopConverting;
        public event Action<bool> OnEnableChanged;

        private Storage _targetStorage;
        private Storage _sourceStorage;
        private bool _enabled;
        private ConvertReceipt _receipt;
        private CouldDown _couldDown;
        private int _convertingCount;

        public int ConvertingCount => _convertingCount;
        public bool IsEnabled => _enabled;
        public int MaxSize => _targetStorage.MaxSize;
        public bool IsInProgress => _enabled && ConvertingCount != 0;
        public ConvertReceipt Receipt => _receipt;
        public float CouldDownTime => _couldDown.Time;

        public Converter(int maxSize, ConvertReceipt receipt)
        {
            _targetStorage = new Storage(maxSize);
            _sourceStorage = new Storage(maxSize);
            _receipt = receipt;
            _couldDown = new CouldDown(receipt.Time);
            _couldDown.OnComplete += CouldDown;
        }

        internal Converter(int maxSize, ConvertReceipt receipt, Storage sourceStorage, Storage targetStorage, int convertingCount)
            : this(maxSize, receipt)
        {
            _sourceStorage = sourceStorage;
            _targetStorage = targetStorage;
            _convertingCount = convertingCount;
        }

        public void Update(float deltaTime)
        {
            if (!IsInProgress) return;
            _couldDown.Tick(deltaTime);
        }

        public void SetEnabled(bool enabled)
        {
            if (_enabled == enabled) return;
            _enabled = enabled;
            OnEnableChanged?.Invoke(enabled);

            if (enabled)
            {
                CheckStartConverting();
            }
            else
            {
                Stop();
            }
        }

        public int AddSourceItem(ItemType itemType, int addCount)
        {
            if (itemType != _receipt.SourceType) return addCount;
            var returnCount = _sourceStorage.AddItem(itemType, addCount);
            var added = addCount - returnCount;

            var wasInProgress = IsInProgress;
            CheckStartConverting();

            if (returnCount > 0 && IsInProgress && !wasInProgress)
            {
                returnCount = _sourceStorage.AddItem(itemType, returnCount);
                added = addCount - returnCount;
            }

            if (returnCount != addCount)
            {
                OnSourceAdded?.Invoke(itemType, added);
            }

            return returnCount;
        }

        public int GetSourceItemCount()
        {
            return _sourceStorage.GetItemCount(_receipt.SourceType);
        }

        public int GetSourceItemCount(ItemType itemType)
        {
            return _sourceStorage.GetItemCount(itemType);
        }

        public int GetTargetItemCount(ItemType targetType)
        {
            return _targetStorage.GetItemCount(targetType);
        }

        public int GetTargetItemCount()
        {
            return _targetStorage.GetItemCount(_receipt.TargetType);
        }

        public bool RemoveSourceItem(ItemType itemType, int count)
        {
            if (itemType != _receipt.SourceType) return false;
            var result = _sourceStorage.RemoveItem(itemType, count);

            if (result)
            {
                OnSourceRemoved?.Invoke(itemType, count);
            }

            return result;
        }
        
        public bool RemoveSourceItem(int count) => RemoveSourceItem(_receipt.SourceType, count);

        public bool RemoveTargetItem(ItemType itemType, int count)
        {
            if (itemType != _receipt.TargetType) return false;
            var result = _targetStorage.RemoveItem(itemType, count);

            if (result)
            {
                OnTargetRemoved?.Invoke(itemType, count);
            }

            CheckStartConverting();
            return result;
        }
        
        public bool RemoveTargetItem(int count) => RemoveTargetItem(_receipt.TargetType, count);

        internal bool Convert()
        {
            if (!CanConvert()) return false;

            _targetStorage.AddItem(_receipt.TargetType, _receipt.TargetCount);
            _convertingCount = 0;
            OnConverted?.Invoke(_receipt);
            CheckStartConverting();
            return true;
        }

        internal void Stop()
        {
            _couldDown.Reset();

            if (ConvertingCount > 0)
            {
                _sourceStorage.AddItem(_receipt.SourceType, _convertingCount);
                _convertingCount = 0;
                OnStopConverting?.Invoke(_receipt);
            }
        }

        internal void CheckStartConverting()
        {
            if (NeedStartConverting())
            {
                _convertingCount = _receipt.SourceCount;
                _sourceStorage.RemoveItem(_receipt.SourceType, _receipt.SourceCount);
                OnStartConverting?.Invoke(_receipt);
            }
        }

        internal bool NeedStartConverting() => IsEnabled && !IsInProgress && CanConvertNext();

        internal bool CanConvertNext()
        {
            if (!IsEnabled) return false;
            if (IsInProgress) return false;
            if (GetSourceItemCount() < _receipt.SourceCount) return false;
            if (GetTargetItemCount() + _receipt.TargetCount > MaxSize) return false;
            return true;
        }

        internal bool CanConvert()
        {
            if (!IsEnabled) return false;
            if (_convertingCount != _receipt.SourceCount) return false;
            if (GetTargetItemCount() + _receipt.TargetCount > MaxSize) return false;
            return true;
        }

        private void CouldDown()
        {
            Convert();
        }
    }
}