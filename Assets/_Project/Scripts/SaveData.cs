using System;

[Serializable]
public class MachineSave
{
    public int Level;
    public bool Unlocked;
}

[Serializable]
public class SaveData
{
    public int Version = 1;
    public double Coins; 
    public double Valuta;
    public MachineSave[] Machines;
    public double LastExitTime;
}