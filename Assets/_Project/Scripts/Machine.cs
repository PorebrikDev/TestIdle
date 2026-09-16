using UnityEngine;

public class Machine
{
    private readonly DataMashine _data;

    public int Lvl { get; private set; }
    public bool IsLocked { get; private set; } = true;

    //Расчеты машины
    public double CPS => _data.BaseCps * (1 + Lvl);
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

    public void LoadFromSave(int level, bool unlocked)
    {
        Lvl = level;
        IsLocked = !unlocked;
    }
}