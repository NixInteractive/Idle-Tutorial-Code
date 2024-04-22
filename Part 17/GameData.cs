using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using System;

[System.Serializable]
public class GameData
{
    public BigDouble money;
    public BigDouble ooze;
    public BigDouble oozeIncome;
    public BigDouble oozeUpgradeCost;

    public GeneratorData[] generators;
    public BigDouble[] upgradeCounts;

    public DateTime saveTime;
}