using Modules.Converter;
using UnityEngine;

public class ConvertorPresenter : MonoBehaviour
{
    private ConvertorView _view;
    private Converter _convertor;

    private void Awake()
    {
        var receipt = new ConvertReceipt(ItemType.Wood, ItemType.Plank, 5, 1, 2);
        _convertor = new Converter(5, receipt);
        _view = GetComponent<ConvertorView>();
        _convertor.SetEnabled(true);
    }

    private void OnEnable()
    {
        _convertor.OnConverted += OnConvertedHandler;
        _convertor.OnSourceAdded += OnSourceAddedHandler;
        _convertor.OnTargetRemoved += OnTargetRemovedHandler;
        _convertor.OnStartConverting += OnStartConvertingHandler;
        _convertor.OnStopConverting += OnStopConvertingHandler;
        _view.OnStartButtonClick += OnStartButtonClickHandler;
        _view.OnStopButtonClick += OnStopButtonClickHandler;
        _view.OnAddSourcePlankButtonClick += OnAddSourcePlankButtonClickHandler;
        _view.OnAddSourceWoodButtonClick += OnAddSourceWoodButtonClickHandler;
        _view.OnRemoveTargetPlankButtonClick += OnRemoveTargetPlankButtonClickHandler;
        _view.OnRemoveTargetWoodButtonClick += OnRemoveTargetWoodButtonClickHandler;
        Invalidate();
    }

    private void OnDisable()
    {
        _convertor.OnConverted -= OnConvertedHandler;
        _convertor.OnSourceAdded -= OnSourceAddedHandler;
        _convertor.OnTargetRemoved -= OnTargetRemovedHandler;
        _convertor.OnStartConverting -= OnStartConvertingHandler;
        _convertor.OnStopConverting -= OnStopConvertingHandler;
        _view.OnStartButtonClick -= OnStartButtonClickHandler;
        _view.OnStopButtonClick -= OnStopButtonClickHandler;
        _view.OnAddSourcePlankButtonClick -= OnAddSourcePlankButtonClickHandler;
        _view.OnAddSourceWoodButtonClick -= OnAddSourceWoodButtonClickHandler;
        _view.OnRemoveTargetPlankButtonClick -= OnRemoveTargetPlankButtonClickHandler;
        _view.OnRemoveTargetWoodButtonClick -= OnRemoveTargetWoodButtonClickHandler;
    }

    private void Update()
    {
        _convertor.Update(Time.deltaTime);
        InvalidateCouldDown();
        InvalidateProgress();
    }

    private void Invalidate()
    {
        InvalidateReceipt();
        InvalidateSize();
        InvalidateState();
        InvalidateSourceCount();
        InvalidateTargetCount();
        InvalidateCouldDown();
        InvalidateProgress();
    }

    private void InvalidateReceipt()
    {
        var receipt = _convertor.Receipt;
        _view.SetReceiptText($"{receipt.SourceType}: {receipt.SourceCount} -> {receipt.TargetType}: {receipt.TargetCount}; Time: {receipt.Time}");
    }

    private void InvalidateSize()
    {
        _view.SetSizeText($"{_convertor.MaxSize}");
    }

    private void InvalidateState()
    {
        _view.SetStateText(_convertor.IsEnabled ? "Enabled" : "Disabled");
    }

    private void InvalidateSourceCount()
    {
        _view.SetSourceCountText($"{_convertor.GetSourceItemCount()}");
    }

    private void InvalidateTargetCount()
    {
        _view.SetTargetCountText($"{_convertor.GetTargetItemCount()}");
    }

    private void InvalidateCouldDown()
    {
        _view.SetCouldDownText(_convertor.CouldDownTime.ToString("0.0") + " s");
    }

    private void InvalidateProgress()
    {
        _view.SetProgress(_convertor.CouldDownTime, _convertor.Receipt.Time);
    }

    private void OnSourceAddedHandler(ItemType item, int count)
    {
        Debug.Log($"{nameof(OnSourceAddedHandler)} {item}: {count}");
        Invalidate();
    }

    private void OnTargetRemovedHandler(ItemType item, int count)
    {
        Debug.Log($"{nameof(OnTargetRemovedHandler)} {item}: {count}");
        Invalidate();
    }

    private void OnStartConvertingHandler(ConvertReceipt receipt)
    {
        Debug.Log($"{nameof(OnStartConvertingHandler)} {receipt}");
        Invalidate();
    }

    private void OnStopConvertingHandler(ConvertReceipt receipt)
    {
        Debug.Log($"{nameof(OnStopConvertingHandler)} {receipt}");
        Invalidate();
    }

    private void OnConvertedHandler(ConvertReceipt receipt)
    {
        Debug.Log($"{nameof(OnConvertedHandler)} {receipt}");
        Invalidate();
    }

    private void OnStartButtonClickHandler()
    {
        Debug.Log($"{nameof(OnStartButtonClickHandler)}");
        _convertor.SetEnabled(true);
    }

    private void OnStopButtonClickHandler()
    {
        Debug.Log($"{nameof(OnStopButtonClickHandler)}");
        _convertor.SetEnabled(false);
    }

    private void OnAddSourcePlankButtonClickHandler()
    {
        Debug.Log($"{nameof(OnAddSourcePlankButtonClickHandler)}");
        _convertor.AddSourceItem(ItemType.Plank, 1);
    }

    private void OnAddSourceWoodButtonClickHandler()
    {
        Debug.Log($"{nameof(OnAddSourceWoodButtonClickHandler)}");
        _convertor.AddSourceItem(ItemType.Wood, 1);
    }

    private void OnRemoveTargetPlankButtonClickHandler()
    {
        Debug.Log($"{nameof(OnRemoveTargetPlankButtonClickHandler)}");
        _convertor.RemoveTargetItem(ItemType.Plank, 1);
    }

    private void OnRemoveTargetWoodButtonClickHandler()
    {
        Debug.Log($"{nameof(OnRemoveTargetWoodButtonClickHandler)}");
        _convertor.RemoveTargetItem(ItemType.Wood, 1);
    }
}