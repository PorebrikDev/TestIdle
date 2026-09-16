using UnityEngine;

public class Machine
{
    private readonly DataMashine _data;

    public int Lvl { get; private set; }
    public bool IsLocked { get; private set; } = true;

    private bool _boosted;
    public bool IsBoosted => _boosted;

    //Расчеты машины
    public double CPS => _data.BaseCps * (1 + Lvl) * (_boosted ? 2.0 : 1.0);
    public double UpgradeCost => _data.BaseCps * 100 * Mathf.Pow(1.15f, Lvl);
    public double UnlockCost => _data.BaseCps * 500;
    //

    public Machine(DataMashine data) { _data = data; }

    public string GetDisplayName()
    {
        string result = _data.name;
        foreach (var stage in _data.NameStages)
        {
            if (Lvl >= stage.MinLvl) result = stage.Name;
            else break;
        }
        return result;
    }

    public Sprite Icon => _data.Icon;
    public Sprite Defolt => _data.Defolt;
    public void Upgrade() => Lvl++;
    public void Unlock() => IsLocked = false;

    public void ActivateBoost() => _boosted = true;
    public void DeactivateBoost() => _boosted = false;

    public void LoadFromSave(int level, bool unlocked)
    {
        Lvl = level;
        IsLocked = !unlocked;
    }
}