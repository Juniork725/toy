using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    private void Awake()
    {
        int myNum = Convert.ToInt16(this.gameObject.name.Split(' ')[1]);
        if (myNum < DataController.Instance.gameData.StageNum)
        {
            this.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    
}
