using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BigDouble money = 0f;
    public BigDouble startMoney;

    public TextMeshProUGUI moneyDisp;
    public TextMeshProUGUI oozeDisp;
    public TextMeshProUGUI offlineDisp;
    public TextMeshProUGUI offlineTimeDisp;

    public Image oozeBar;

    public GameObject offlinePanel;

    public float oozeTime = 0;
    public BigDouble oozeIncome;
    public BigDouble ooze;

    public DataPersist DP;

    public Generator[] generators;
    public GeneratorUpgrade[] upgrades;

    public CanvasGroup[] menus;
    public CanvasGroup[] subMenus; 

    void Awake()
    {
        upgrades = FindObjectsOfType<GeneratorUpgrade>();
        DP = GetComponent<DataPersist>();

        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        moneyDisp.text = "Money: " + SciNotToUSName(money);
        oozeDisp.text = "Ooze: " + SciNotToUSName(ooze);

        oozeTime += Time.deltaTime;

        if(oozeTime >= 1)
        {
            ooze += oozeIncome;
            oozeTime = 0;
        }

        oozeBar.fillAmount = oozeTime;

        SaveGame();
    }

    public void ToggleMenu(CanvasGroup menu)
    {
        foreach(CanvasGroup cg in menus)
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }

        menu.alpha = 1;
        menu.blocksRaycasts = true;
        menu.interactable = true;
    }

    public void ToggleSubMenu(CanvasGroup menu)
    {
        foreach (CanvasGroup cg in subMenus)
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }

        menu.alpha = 1;
        menu.blocksRaycasts = true;
        menu.interactable = true;
    }

    public void CloseOfflinePanel()
    {
        Destroy(offlinePanel);
    }

    private BigDouble CalculateOfflineTime()
    {
        BigDouble timeElapsed = DateTime.Now.Subtract(DP.GD.saveTime).TotalSeconds;
        BigDouble maxOfflineTime = new BigDouble(43200);

        if(timeElapsed > maxOfflineTime)
        {
            timeElapsed = maxOfflineTime;
        }

        if(timeElapsed < 0)
        {
            timeElapsed = 0;
        }

        return timeElapsed;
    }

    private void ProcessOfflineProduction(BigDouble timePassed)
    {
        if(timePassed < 5)
        {
            CloseOfflinePanel();
        }

        for(int i = generators.Length -1; i >= 0; i--)
        {
            if(generators[i].data.quantity > 0)
            {
                BigDouble timesTriggered = timePassed.Divide(generators[i].data.maxTime);

                if (generators[i].product)
                {
                    generators[i].product.data.quantity += generators[i].data.income * generators[i].data.quantity * timesTriggered;
                }
                else
                {
                    money += generators[i].data.income * generators[i].data.quantity * timesTriggered;
                    offlineDisp.text = SciNotToUSName(money - startMoney);
                }

                generators[i].data.time += timePassed % generators[i].data.maxTime;
            }
        }
        ooze += timePassed * oozeIncome;
        offlineTimeDisp.text = TimeSpan.FromSeconds(CalculateOfflineTime().ToDouble()).ToString(@"hh\:mm\:ss");
    }

    public string SciNotToUSName(BigDouble num)
    {
        string displayNumber = $"{(num.Mantissa * BigDouble.Pow(10, num.Exponent % 3)):G3} ";
        int prefixIndex = (int)BigDouble.Floor(BigDouble.Abs(num.Exponent)).ToDouble();
        string name = "";
        int prefixOffset = 0;

        if (num.Exponent < 33)
        {
            prefixIndex /= 3;
            name += $"{Prefixes.prefixes[prefixIndex]}";
            return displayNumber + name;
        }
        else
        {
            prefixIndex = (prefixIndex - 3) / 3;
            int tempPrefixIndex = prefixIndex;
            List<int> prefixList = new();
            for (int i = 0; i < prefixIndex.ToString().Length; i++)
            {
                int lastNum = tempPrefixIndex % 10;
                prefixList.Add(lastNum);
                tempPrefixIndex /= 10;
                name += Prefixes.allPrefixes[prefixList[i] + prefixOffset];
                prefixOffset += 10;
            }
            return $"{displayNumber} {name}";
        }
    }

    public void DeleteSave()
    {
        if (DP.TryLoad())
        {
            money = new BigDouble(10);
            ooze = BigDouble.Zero;
            oozeIncome = new BigDouble(1);
            GetComponent<UpgradeManager>().oozeProductionCost = new BigDouble(10);
            DP.GD.oozeUpgradeCost = new BigDouble(10);

            for (int i = generators.Length - 1; i >= 0; i--)
            {
                generators[i].data.quantity = 0;
                generators[i].data.time = 0;
                generators[i].data.maxTime = generators[i].data.defaultTime;
                generators[i].data.income = generators[i].data.defaultIncome;
            }

            for(int i = 0; i <upgrades.Length; i++)
            {
                upgrades[i].count = 0;
            }

            SaveGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void LoadGame()
    {
        if (DP.TryLoad())
        {
            DP.LoadData();

            money = DP.GD.money;
            startMoney = money;
            ooze = DP.GD.ooze;
            oozeIncome = DP.GD.oozeIncome;
            
            
            for(int i = generators.Length-1; i>=0; i--)
            {
                generators[i].data = new GeneratorData(DP.GD.generators[i]);
            }

            for(int i = 0; i <upgrades.Length; i++)
            {
                upgrades[i].count = DP.GD.upgradeCounts[i];
            }

            ProcessOfflineProduction(CalculateOfflineTime());
        }
        else
        {
            CloseOfflinePanel();
            money = new BigDouble(10);
            ooze = BigDouble.Zero;
            DP.GD.oozeUpgradeCost = new BigDouble(10);

            for (int i = generators.Length - 1; i >= 0; i--)
            {
                generators[i].data.quantity = 0;
            }

            SaveGame();
        }
    }

    private void SaveGame()
    {
        DP.GD.money = money;
        DP.GD.ooze = ooze;
        DP.GD.oozeIncome = oozeIncome;
        DP.GD.generators = new GeneratorData[generators.Length];
        DP.GD.upgradeCounts = new BigDouble[upgrades.Length];

        for (int i = generators.Length - 1; i >= 0; i--)
        {
            DP.GD.generators[i] = new GeneratorData(generators[i].data);
        }

        for(int i = 0; i < upgrades.Length; i++)
        {
            DP.GD.upgradeCounts[i] = upgrades[i].count;
        }

        DP.SaveData();
    }
}