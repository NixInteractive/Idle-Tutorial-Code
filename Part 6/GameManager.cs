using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;

public class GameManager : MonoBehaviour
{
    public BigDouble money = 0f;

    public TextMeshProUGUI moneyDisp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyDisp.text = "Money: " + SciNotToUSName(money);
    }

    public void OnMoneyClick()
    {
        money*=2;
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
}
