using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public BigDouble oozeProductionCost;
    public TextMeshProUGUI oozeUpgradeCostDisp;

    private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GameManager>();
        oozeProductionCost = GM.DP.GD.oozeUpgradeCost;
    }

    // Update is called once per frame
    void Update()
    {
        oozeUpgradeCostDisp.text = GM.SciNotToUSName(oozeProductionCost);
    }

    public void OozeProductionUpgrade()
    {
        if(oozeProductionCost > GM.money)
        {
            return;
        }

        GM.oozeIncome *= 2;
        GM.money -= oozeProductionCost;
        oozeProductionCost *= 100;
        GM.DP.GD.oozeUpgradeCost = oozeProductionCost;
    }
}
