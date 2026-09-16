using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IapUI : MonoBehaviour
{
    [SerializeField] private IapService _iapService;
    [SerializeField] private Controller _controller;
    [SerializeField] private Button _buyButton;

    [SerializeField] private GameObject _window;
    [SerializeField] private TMP_Text _windowText;

    private WaitForSeconds _waitTime = new WaitForSeconds(2f);

    private void Awake()
    {
        _buyButton.onClick.AddListener(OnBuyClicked);
        _iapService.OnPurchaseSucceeded += OnSuccess;
        _iapService.OnPurchaseFailed += OnFail;
        _window.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(OnBuyClicked);
        _iapService.OnPurchaseSucceeded -= OnSuccess;
        _iapService.OnPurchaseFailed -= OnFail;
    }

    private void OnBuyClicked() => _iapService.BuyCoins();

    private void OnSuccess()
    {
        _controller.AddCoins(10000);
        ShowWindow("Куплено! +10000 монет");
    }

    private void OnFail(string reason)
    {
        ShowWindow($"Ошибка: {reason}");
    }

    public void ShowWindow(string message)
    {
        _windowText.text = message;
        _window.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HidePopupAfter());
    }

    private IEnumerator HidePopupAfter()
    {
        yield return _waitTime;
        _window.SetActive(false);
    }

}