using UnityEngine;
using UnityEngine.UI;

public class PlusCoin : MonoBehaviour
{
    [SerializeField] private Controller _ctr;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PlusMoney);
    }

    private void PlusMoney()
    {
        _ctr.AddCoins(1000);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(PlusMoney);

    }
}
