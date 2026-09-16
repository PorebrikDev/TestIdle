using TMPro;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _cpsText;
    [SerializeField] private TMP_Text _valutaText;

    private void Start()
    {
        _controller.OnStateChanged += Refresh;
        InvokeRepeating(nameof(Refresh), 0f, 1f);   
    }

    private void OnDestroy()
    {
        _controller.OnStateChanged -= Refresh;
    }

    private void Refresh()
    {
        _coinsText.text = $"{_controller.Coins:F0}";
        _cpsText.text = $"{_controller.TotalCps:F1}/сек";
        _valutaText.text = $"{_controller.Valuta:F0}";
    }
}