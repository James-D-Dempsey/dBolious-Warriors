using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Very similar to CPUinfo.cs, Main differences are different variable types that are player based rather than cpu
public class playerInfo : MonoBehaviour
{
    public Text hitPercentData;
    public float i_hitPercent = 0;
    public int i_stockCount = 3;

    void Update()
    {
        if (hitPercentData == null)
        {
            return;
        }

        hitPercentData.text = "Player 1: " + i_hitPercent + "\nStocks: " + i_stockCount;
    }
}
