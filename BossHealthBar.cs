using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Enemy BossHP;
    public Image fillImage;
    private Slider slider;



    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {   
        if(slider.value < slider.minValue)
        {
            fillImage.enabled = false;
        }

        if(slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        float fillValue = BossHP.currentHP / BossHP.MaxHP;

        if(fillValue <= slider.maxValue / 3)
        {
           fillImage.color = Color.red; 
        } 
        else if (fillValue > slider.maxValue / 3)
        {
            fillImage.color = Color.green;
        }

        slider.value = fillValue;
    }
}

