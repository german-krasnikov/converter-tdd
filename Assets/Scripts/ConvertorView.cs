using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConvertorView : MonoBehaviour
{
    public event Action OnAddSourceWoodButtonClick;
    public event Action OnAddSourcePlankButtonClick;
    public event Action OnRemoveTargetWoodButtonClick;
    public event Action OnRemoveTargetPlankButtonClick;
    public event Action OnStartButtonClick;
    public event Action OnStopButtonClick;

    [SerializeField]
    private Button _addSourceWoodButton;
    [SerializeField]
    private Button _addSourcePlankButton;
    [SerializeField]
    private Button _removeTargetWoodButton;
    [SerializeField]
    private Button _removeTargetPlankButton;
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private Button _stopButton;
    [SerializeField]
    private TMP_Text _stateText;
    [SerializeField]
    private TMP_Text _sizeText;
    [SerializeField]
    private TMP_Text _receptText;
    [SerializeField]
    private TMP_Text _sourceCountText;
    [SerializeField]
    private TMP_Text _targetCountText;
    [SerializeField]
    private TMP_Text _couldDownText;
    [SerializeField]
    private Slider _progess;

    public void SetStateText(string text) => _stateText.text = text;
    public void SetSizeText(string text) => _sizeText.text = text;
    public void SetReceptText(string text) => _receptText.text = text;
    public void SetSourceCountText(string text) => _sourceCountText.text = text;
    public void SetTargetCountText(string text) => _targetCountText.text = text;
    public void SetCouldDownText(string text) => _couldDownText.text = text;

    public void SetProgress(float value, float max)
    {
        _progess.maxValue = max;
        _progess.value = value;
    }

    private void OnEnable()
    {
        _addSourceWoodButton.onClick.AddListener(AddSourceWoodButtonClickHandler);
        _addSourcePlankButton.onClick.AddListener(AddSourcePlankButtonClickHandler);
        _removeTargetWoodButton.onClick.AddListener(RemoveTargetWoodButtonClickHandler);
        _removeTargetPlankButton.onClick.AddListener(RemoveTargetPlankButtonClickHandler);
        _startButton.onClick.AddListener(StartButtonClickHandler);
        _stopButton.onClick.AddListener(StopButtonClickHandler);
    }

    private void OnDisable()
    {
        _addSourceWoodButton.onClick.RemoveListener(AddSourceWoodButtonClickHandler);
        _addSourcePlankButton.onClick.RemoveListener(AddSourcePlankButtonClickHandler);
        _removeTargetWoodButton.onClick.RemoveListener(RemoveTargetWoodButtonClickHandler);
        _removeTargetPlankButton.onClick.RemoveListener(RemoveTargetPlankButtonClickHandler);
        _startButton.onClick.RemoveListener(StartButtonClickHandler);
        _stopButton.onClick.RemoveListener(StopButtonClickHandler);
    }

    private void StopButtonClickHandler()
    {
        OnStopButtonClick?.Invoke();
    }

    private void StartButtonClickHandler()
    {
        OnStartButtonClick?.Invoke();
    }

    private void RemoveTargetPlankButtonClickHandler()
    {
        OnRemoveTargetPlankButtonClick?.Invoke();
    }

    private void RemoveTargetWoodButtonClickHandler()
    {
        OnRemoveTargetWoodButtonClick?.Invoke();
    }

    private void AddSourcePlankButtonClickHandler()
    {
        OnAddSourcePlankButtonClick?.Invoke();
    }

    private void AddSourceWoodButtonClickHandler()
    {
        OnAddSourceWoodButtonClick?.Invoke();
    }
}