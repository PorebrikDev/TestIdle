using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Mashine")]
public class DataMashine : ScriptableObject
{
    public int Id;
    public double BaseCps;
    public Sprite Icon;
    public Sprite Defolt;
    public NameStage[] NameStages;
}

[Serializable]
public class NameStage
{
    public int MinLvl;
    public string Name;
}