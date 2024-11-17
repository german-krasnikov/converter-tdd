using System;

namespace Homework
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
        public event Action<ItemType, int> OnAdded;
        public event Action<ItemType, int> OnRemoved;
        public event Action<ItemType> Converted;
        public event Action<bool> OnEnableChanged;

        private Storage _convertStorage;
        private Storage _sourceStorage;
        private float _couldDown;
        private bool _enabled;
        private ConvertReceipt _receipt;

        public bool IsEnabled => _enabled;
        public float Time => _receipt.Time;

        public Converter(int maxSize, ConvertReceipt receipt)
        {
            _convertStorage = new Storage(maxSize);
            _sourceStorage = new Storage(maxSize);
            _receipt = receipt;
        }

        public void Update(float deltaTime)
        {
        }

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
            OnEnableChanged?.Invoke(enabled);
        }

        public int AddSourceItem(ItemType itemType, int addCount)
        {
            if (itemType != _receipt.SourceType) return addCount;
            return _sourceStorage.AddItem(itemType, addCount);
        }

        public int GetSourceItemCount(ItemType itemType)
        {
            return _sourceStorage.GetItemCount(itemType);
        }
    }
}