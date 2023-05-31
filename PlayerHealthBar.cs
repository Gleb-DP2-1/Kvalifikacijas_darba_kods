using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHP;
    public Image fillImage;
    private Slider slider;
    public Text HP_Label;



    void Awake()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        HP_Label.text = playerHP.CurHP.ToString() + " / " + playerHP.HP.ToString();
        if(slider.value < slider.minValue)
        {
            fillImage.enabled = false;
        }

        if(slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        float fillValue = playerHP.CurHP / playerHP.HP;

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
