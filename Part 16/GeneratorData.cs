using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

[System.Serializable]
public class GeneratorData
{
    public BigDouble quantity;
    public BigDouble income;
    public BigDouble productCost;
    public BigDouble moneyCost;
    public BigDouble oozeCost;
    public BigDouble maxTime;
    public BigDouble time;
    public BigDouble defaultTime;
    public BigDouble defaultIncome;

    public GeneratorData(GeneratorData data)
    {
        quantity = data.quantity;
        income = data.income;
        productCost = data.productCost;
        moneyCost = data.moneyCost;
        oozeCost = data.oozeCost;
        maxTime = data.maxTime;
        time = data.time;
}
}
