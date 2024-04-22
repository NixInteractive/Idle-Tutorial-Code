using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using TMPro;

[System.Serializable]
public class GeneratorUpgrade : MonoBehaviour
{
    public BigDouble baseCost;
    public BigDouble count;

    private BigDouble currentCost;
    public BigDouble modifier;

    private GameManager GM;
    public Generator generator;

    public TextMeshProUGUI costDisp;
    public TextMeshProUGUI countDisp;

    public enum UpgradeTypes
    {
        Speed,
        Amount
    };

    public UpgradeTypes type;

    public GeneratorUpgrade(GeneratorUpgrade GU)
    {
        baseCost = GU.baseCost;
        count = GU.count;
    }

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        UpdateCost();
        countDisp.text = "Lvl: " + GM.SciNotToUSName(count);
    }

    public void UpdateCost()
    {
        currentCost = baseCost * modifier.Pow(count);
        costDisp.text = "Cost: " + GM.SciNotToUSName(currentCost);
    }

    public void BuyUpgrade()
    {
        if(GM.money < currentCost)
        {
            return;
        }

        switch (type)
        {
            case UpgradeTypes.Speed:
                generator.data.maxTime *= 0.75f;
                break;
            case UpgradeTypes.Amount:
                generator.data.income *= 1.5f;
                break;
        }

        count++;
        countDisp.text = "Lvl: " + GM.SciNotToUSName(count);
        GM.money -= currentCost;
        UpdateCost();
    }
}
