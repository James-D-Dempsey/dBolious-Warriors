using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUinfo : MonoBehaviour
{
    public Text hitPercentData; //Text that shows below values
    public float i_hitPercent = 0; //Records current hitpercent
    public int i_stockCount = 3; //Records stock count

    void Update()
    {
        hitPercentData.text = "CPU: " + i_hitPercent + "\nStocks: " + i_stockCount; //changes when difference is found in above variables

    }
}
