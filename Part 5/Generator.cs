using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using BreakInfinity;
using UnityEngine.UI;

public class Generator : Monobehaviour
{
    public GameManager GM;

    public Image progressBar;
    
    public TextMeshProUGUI nameDisp;
    public TextMeshProUGUI quantityDisp;
    public TextMeshProUGUI incomeDisp;

    public Generator product;

    public BigDouble quantity;
    public BigDouble income;

    public float maxTime;
    public float time;

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

        time += Time.deltaTime;

        if(time >= maxTime)
        {
            if(!product)
            {
                GM.money += income * quantity;
            }
            else
            {
                product.quantity += income * quantity;
            }

            time = 0;
        }

        progressBar.fillAmount = time / maxTime;
    }
}
