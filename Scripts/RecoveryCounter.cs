using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCounter : MonoBehaviour
{

    //[System.NonSerialized] 

    public float recoveryTime = 1f;
    [System.NonSerialized] public float counter;
    [System.NonSerialized] public bool recovering = false;

    // Update is called once per frame
    void Update()
    {
        if (counter <= recoveryTime)
        {
            counter += Time.deltaTime;
            recovering = true;
        }
        else
        {
            recovering = false;
        }
    }
}
