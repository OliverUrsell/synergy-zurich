using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pill : MonoBehaviour
{

    public Sprite airSprite;
    public string airLeftText = "Dirty";
    public string airRightText = "Clean";

    public Sprite brightnessSprite;
    public string brightnessLeftText = "Dark";
    public string brightnessRightText = "Bright";

    public Sprite temperatureSprite;
    public string temperatureLeftText = "Cool";
    public string temperatureRightText = "Hot";

    public Image pillImage;
    public TextMeshProUGUI leftTextUI;
    public TextMeshProUGUI rightTextUI;


    public void setPill(SignalController.Sensor sensor){

        Sprite newSprite = null;
        string leftText = null;
        string rightText = null;

        switch(sensor){
            case SignalController.Sensor.AIR_QUALITY:
                newSprite = airSprite;
                leftText = airLeftText;
                rightText = airRightText;
                break;
            case SignalController.Sensor.BRIGHTNESS:
                newSprite = brightnessSprite;
                leftText = brightnessLeftText;
                rightText = brightnessRightText;
                break;
            case SignalController.Sensor.TEMPERATURE:
                newSprite = temperatureSprite;
                leftText = temperatureLeftText;
                rightText = temperatureRightText;
                break;
        }

        pillImage.sprite = newSprite;
        leftTextUI.text = leftText;
        rightTextUI.text = rightText;

    }

    private void setAirQuality(){}

    private void setBrightness(){}

    private void setTemperature(){}

}
