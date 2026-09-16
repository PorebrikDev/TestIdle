using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineCard : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _upgraidButton;
    [SerializeField] private Button _boostButton;

    [SerializeField] private TMP_Text _nameCard;
    [SerializeField] private TMP_Text _openCost;
    [SerializeField] private TMP_Text _UpgraidCost;
    [SerializeField] private TMP_Text _prefomanceMachineText;
    [SerializeField] private TMP_Text _boostTimeer;
    [SerializeField] private Image _icon;

    private Machine _machine;
    private int _id;
    private System.Action<int> _onOpen;
    private System.Action<int> _onUpgrade;
    private System.Action<int> _onBoost;

    public void Bind(Machine machine, int id, System.Action<int> onOpen, System.Action<int> onUpgrade, System.Action<int> onBoost)
    {
        _machine = machine;
        _id = id;
        _onOpen = onOpen;
        _onUpgrade = onUpgrade;
        _onBoost = onBoost;

        _openButton.onClick.AddListener(OnOpenClicked);
        _upgraidButton.onClick.AddListener(OnUpgradeClicked);
        _boostButton.onClick.AddListener(OnBoostClicked);

        Refresh();
    }

    private void OnOpenClicked() => _onOpen?.Invoke(_id);
    private void OnUpgradeClicked() => _onUpgrade?.Invoke(_id);
    private void OnBoostClicked() => _onBoost?.Invoke(_id);

    public void Refresh()
    {
        var m = _machine;

        _nameCard.text = m.GetDisplayName();

        if (!m.IsLocked)
        {
            _prefomanceMachineText.text = $"{m.CPS:F1}/сек";
            _UpgraidCost.text = $"{m.UpgradeCost:F0}";
            _openCost.text = "";
            _icon.sprite = m.Icon;

            _openButton.gameObject.SetActive(false);
            _upgraidButton.gameObject.SetActive(true);
            _boostButton.gameObject.SetActive(true);
        }
        else
        {
            // закрытая
            _openCost.text = $"{m.UnlockCost:F0}";
            _prefomanceMachineText.text = "";
            _UpgraidCost.text = "";
            _icon.sprite = m.Defolt;

            _openButton.gameObject.SetActive(true);
            _upgraidButton.gameObject.SetActive(false);
            _boostButton.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _openButton.onClick.RemoveListener(OnOpenClicked);
        _upgraidButton.onClick.RemoveListener(OnUpgradeClicked);
        _boostButton.onClick.RemoveListener(OnBoostClicked);
    }
}