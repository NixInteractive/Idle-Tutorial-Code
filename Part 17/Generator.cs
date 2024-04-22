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
    public TextMeshProUGUI BuyDisp;

    public Button BuyButton;

    public Generator product;

    public string generatorName;

    void Awake()
    {
        data.defaultIncome = data.income;
        data.defaultTime = data.maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        quantityDisp.text = GM.SciNotToUSName(data.quantity);
        incomeDisp.text = GM.SciNotToUSName(data.quantity * data.income.Divide(data.maxTime)) + "/s";
        nameDisp.text = generatorName;
        BuyDisp.text = "Buy X" + GM.SciNotToUSName(CalculateMax());

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

    public BigDouble CalculateMax()
    {
        BigDouble maxAmount = GM.money.Divide(data.moneyCost);

        if(GM.ooze.Divide(data.oozeCost) < maxAmount)
        {
            maxAmount = GM.ooze.Divide(data.oozeCost);
        }

        if(product && product.data.quantity.Divide(data.productCost) < maxAmount)
        {
            maxAmount = product.data.quantity.Divide(data.productCost);
        }

        maxAmount = maxAmount.Floor();

        return maxAmount;
    }

    public void Buy()
    {
        BigDouble amount = CalculateMax();

        //if (data.moneyCost > GM.money || data.oozeCost > GM.ooze)
        //{
        //    return;
        //}

        if (product)
        {
            //if (data.productCost > product.data.quantity)
            //{
            //    return;
            //}

            product.data.quantity -= data.productCost * amount;
        }

        GM.money -= data.moneyCost * amount;
        GM.ooze -= data.oozeCost * amount;

        data.quantity += amount;
    }
}