using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Monobehaviour
{
    public float money = 0f;

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
        money++;
    }
}
