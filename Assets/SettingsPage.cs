using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPage : MonoBehaviour
{

    public TextMeshProUGUI brightnessVal;
    public TextMeshProUGUI temperatureVal;

    public Slider temperatureSlider;
    public Slider brightnessSlider;

    public static float temperature = 20f;
    public static float brightness = 750f;

    void Start(){
        updateText();
    }

    private void updateText(){
        brightnessVal.text = brightness.ToString("#");
        temperatureVal.text = temperature.ToString("#.0");
    }

    public void toggle(){
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void temperatureChanged(){
        temperature = temperatureSlider.value;
        updateText();
    }

    public void brightnessChanged(){
        brightness = brightnessSlider.value;
        updateText();
    }

}
