using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public GameManager GM;
    public GeneratorData data;

    public Image progressBar;

    public TextMeshProUGUI nameDisp;
    public TextMeshProUGUI quantityDisp;
    public TextMeshProUGUI incomeDisp;
    public TextMeshProUGUI costDisp;

    public Button BuyButton;

    public Generator product;

    public string generatorName;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        quantityDisp.text = GM.SciNotToUSName(data.quantity);
        incomeDisp.text = GM.SciNotToUSName(data.income);
        nameDisp.text = generatorName;

        if (product)
        {
            costDisp.text = "Slime: " + product.generatorName + ": " + GM.SciNotToUSName(data.productCost) + " ~ Money: " + GM.SciNotToUSName(data.moneyCost) + " ~ Ooze: " + GM.SciNotToUSName(data.oozeCost);
        }
        else
        {
            costDisp.text = "Money: " + GM.SciNotToUSName(data.moneyCost) + " ~ Ooze: " + GM.SciNotToUSName(data.oozeCost);
        }

        if (data.quantity > 0)
        {
            data.time += Time.deltaTime;
        }

        if (data.time >= data.maxTime)
        {
            if (!product)
            {
                GM.money += data.income * data.quantity;
            }
            else
            {
                product.data.quantity += data.income * data.quantity;
            }

            data.time -= data.maxTime;
        }

        progressBar.fillAmount = (float)data.time.Divide(data.maxTime).ToDouble();

        if (data.moneyCost > GM.money || data.oozeCost > GM.ooze)
        {
            BuyButton.interactable = false;
        }
        else if (product)
        {
            if (data.productCost > product.data.quantity)
            {
                BuyButton.interactable = false;
            }
            else
            {
                BuyButton.interactable = true;
            }
        }
        else
        {
            BuyButton.interactable = true;
        }
    }

    public void Buy()
    {
        if (data.moneyCost > GM.money || data.oozeCost > GM.ooze)
        {
            return;
        }

        if (product)
        {
            if (data.productCost > product.data.quantity)
            {
                return;
            }

            product.data.quantity -= data.productCost;
        }

        GM.money -= data.moneyCost;
        GM.ooze -= data.oozeCost;

        data.quantity++;
    }
}
