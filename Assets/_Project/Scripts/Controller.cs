using System;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private DataMashine[] _configs;

    private Machine[] _machines;
    private SaveService _saveService;
    private double _coins;
    private double _valuta;

    public event Action OnStateChanged;

    public double Coins => _coins;
    public double Valuta => _valuta;
    public int MachineCount => _machines.Length;

    private void Awake()
    {
        Analytics.Init();                 
        Analytics.Track("game_started");

        _saveService = new SaveService();

        _machines = new Machine[_configs.Length];
        for (int i = 0; i < _configs.Length; i++)
            _machines[i] = new Machine(_configs[i]);

        var save = _saveService.Load();
        ApplySave(save);
    }

    public Machine GetMachine(int id) => _machines[id];

    public bool TryUpgrade(int id)
    {
        var m = _machines[id];
        if (m.IsLocked) return false;

        double cost = m.UpgradeCost;
        if (_coins < cost) return false;

        _coins -= cost;
        m.Upgrade();
        OnStateChanged?.Invoke();

        Analytics.Track("machine_upgraded", new() { { "id", id }, { "level", m.Lvl } });
        Save();

        return true;
    }

    public bool TryUnlock(int id)
    {
        var m = _machines[id];
        if (!m.IsLocked) return false;

        double cost = m.UnlockCost;
        if (_coins < cost) return false;

        _coins -= cost;
        m.Unlock();
        OnStateChanged?.Invoke();

        Analytics.Track("machine_unlocked", new() { { "id", id } }); 

        Save();
        return true;
    }

    public void AddCoins(double x)
    {
        _coins += x;
        OnStateChanged?.Invoke();
        Save();
    }
    public void AddValuta(double x)
    {
        _valuta += x;
        OnStateChanged?.Invoke();
        Save();
    }

    private void Update()
    {
        _coins += TotalCps * Time.deltaTime;
    }

    public double TotalCps
    {
        get
        {
            double cps = 0;
            foreach (var m in _machines)
                if (!m.IsLocked) cps += m.CPS;
            return cps;
        }
    }

    private void ApplySave(SaveData save)
    {
        _coins = save.Coins;
        _valuta = save.Valuta;


        if (save.Machines == null || save.Machines.Length == 0)
        {
            if (_machines.Length > 0) _machines[0].Unlock();
            return;
        }

        for (int i = 0; i < _machines.Length; i++)
        {
            if (i < save.Machines.Length)
                _machines[i].LoadFromSave(save.Machines[i].Level, save.Machines[i].Unlocked);
        }
    }

    public void Save()
    {
        var data = new SaveData
        {
            Coins = _coins,
            Valuta = _valuta,
            LastExitTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Machines = new MachineSave[_machines.Length]
        };

        for (int i = 0; i < _machines.Length; i++)
        {
            data.Machines[i] = new MachineSave
            {
                Level = _machines[i].Lvl,
                Unlocked = !_machines[i].IsLocked
            };
        }

        _saveService.Save(data);
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused) Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}