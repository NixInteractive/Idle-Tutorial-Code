using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using System;

[System.Serializable]
public class GameData
{
    public BigDouble money;

    public BigDouble[] quantities = new BigDouble[17];

    public DateTime saveTime;
}
