using NUnit.Framework;
using UnityEngine;

public class MachineTests
{
    private DataMashine MakeConfig(double baseCps = 1.0)
    {
        var config = ScriptableObject.CreateInstance<DataMashine>();
        config.BaseCps = baseCps;
        return config;
    }

    [Test]
    public void Upgrade_IncreasesLevel()
    {
        var m = new Machine(MakeConfig());

        m.Upgrade();

        Assert.AreEqual(1, m.Lvl);
    }

    [Test]
    public void CPS_GrowsWithLevel()
    {
        var m = new Machine(MakeConfig(baseCps: 2.0));

        var before = m.CPS;
        m.Upgrade();
        var after = m.CPS;

        Assert.Greater(after, before);
    }

    [Test]
    public void IsLocked_InitiallyTrue()
    {
        var m = new Machine(MakeConfig());

        Assert.IsTrue(m.IsLocked);
    }
}