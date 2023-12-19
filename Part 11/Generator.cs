using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public GameManager GM;

    public Image progressBar;

    public TextMeshProUGUI nameDisp;
    public TextMeshProUGUI quantityDisp;
    public TextMeshProUGUI incomeDisp;
    public TextMeshProUGUI costDisp;

    public Button BuyButton;

    public Generator product;

    public BigDouble quantity;
    public BigDouble income;
    public BigDouble productCost;
    public BigDouble moneyCost;
    public BigDouble oozeCost;

    public BigDouble maxTime;
    public BigDouble time;

    public string generatorName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        quantityDisp.text = GM.SciNotToUSName(quantity);
        incomeDisp.text = GM.SciNotToUSName(income);
        nameDisp.text = generatorName;

        if (product)
        {
            costDisp.text = "Slime: " + product.generatorName + ": " + GM.SciNotToUSName(productCost) + " ~ Money: " + GM.SciNotToUSName(moneyCost) + " ~ Ooze: " + GM.SciNotToUSName(oozeCost);
        }
        else
        {
            costDisp.text = "Money: " + GM.SciNotToUSName(moneyCost) + " ~ Ooze: " + GM.SciNotToUSName(oozeCost);
        }

        if (quantity > 0)
        {
            time += Time.deltaTime;
        }

        if (time >= maxTime)
        {
            if (!product)
            {
                GM.money += income * quantity;
            }
            else
            {
                product.quantity += income * quantity;
            }

            time -= maxTime;
        }

        progressBar.fillAmount = (float)time.Divide(maxTime).ToDouble();

        if (moneyCost > GM.money || oozeCost > GM.ooze)
        {
            BuyButton.interactable = false;
        }
        else if (product)
        {
            if (productCost > product.quantity)
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
        if (moneyCost > GM.money || oozeCost > GM.ooze)
        {
            return;
        }

        if (product)
        {
            if (productCost > product.quantity)
            {
                return;
            }

            product.quantity -= productCost;
        }

        GM.money -= moneyCost;
        GM.ooze -= oozeCost;

        quantity++;
    }
}