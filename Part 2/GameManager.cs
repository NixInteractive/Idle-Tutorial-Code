using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;

public class GameManager : Monobehaviour
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
        moenyDisp.text = "Money: " + money;
    }

    public void OnMoneyClick()
    {
        money*=2;
    }
}
