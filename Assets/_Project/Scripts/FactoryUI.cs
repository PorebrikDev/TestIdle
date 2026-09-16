using UnityEngine;

public class FactoryUI : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private MachineCard _cardPrefab;
    [SerializeField] private Transform _container;

    private MachineCard[] _cards;

    private void Start()
    {
        int count = _controller.MachineCount;
        _cards = new MachineCard[count];

        for (int i = 0; i < count; i++)
        {
            var card = Instantiate(_cardPrefab, _container);
            card.Bind(_controller.GetMachine(i), i, OnOpen, OnUpgrade, OnBoost);
            _cards[i] = card;
        }
    }

    private void OnOpen(int id)
    {
        _controller.TryUnlock(id);
        RefreshAll();
    }

    private void OnUpgrade(int id)
    {
        _controller.TryUpgrade(id);
        RefreshAll();
    }

    private void OnBoost(int id)
    {
       
        RefreshAll();
    }

    private void RefreshAll()
    {
        foreach (var card in _cards) card.Refresh();
    }
}