using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Modules.Converter.Tests")]  
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
        public event Action<ItemType, int> OnConvertedRemoved;
        public event Action<ItemType, int> OnConverted;
        public event Action<bool> OnEnableChanged;

        private Storage _convertedStorage;
        private Storage _sourceStorage;
        private float _couldDown;
        private bool _enabled;
        private ConvertReceipt _receipt;

        public bool IsEnabled => _enabled;
        public float Time => _receipt.Time;

        public Converter(int maxSize, ConvertReceipt receipt)
        {
            _convertedStorage = new Storage(maxSize);
            _sourceStorage = new Storage(maxSize);
            _receipt = receipt;
        }

        public void Update(float deltaTime)
        {
        }

        internal bool Convert()
        {
            return true;
        }

        public void SetEnabled(bool enabled)
        {
            if (_enabled == enabled) return;
            _enabled = enabled;
            OnEnableChanged?.Invoke(enabled);
        }

        public int AddSourceItem(ItemType itemType, int addCount)
        {
            if (itemType != _receipt.SourceType) return addCount;
            var returnCount = _sourceStorage.AddItem(itemType, addCount);

            if (returnCount != addCount)
            {
                OnSourceAdded?.Invoke(itemType, returnCount);
            }

            return returnCount;
        }

        public int GetSourceItemCount(ItemType itemType)
        {
            return _sourceStorage.GetItemCount(itemType);
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
    }
}