using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlap : MonoBehaviour
{

    public string id;
    private Renderer rend;

    public void setColorWithSignals(){
        if(SignalController.idToRoom.ContainsKey(id)){
            SignalController.room room = SignalController.idToRoom[id];
            float lerp;
            switch(SignalController.selectedSensor){
                case SignalController.Sensor.AIR_QUALITY:
                    lerp = (room.sensors.airQuality - 400)/(2000-400);
                    setColour(Color.Lerp(new Color(180f/255f, 142f/255f, 95f/255f), new Color(156f/255f, 207f/255f, 206f/225f), lerp));
                    break;
                case SignalController.Sensor.TEMPERATURE:
                    lerp = (room.sensors.temperature - 0)/(50-0);
                    setColour(Color.Lerp(new Color(152f/255f, 190f/255f, 216f/255f), new Color(250f/255f, 167f/255f, 86f/225f), lerp));
                    break;
                case SignalController.Sensor.BRIGHTNESS:
                    lerp = (room.sensors.brightness - 1)/(1000-1);
                    setColour(Color.Lerp(new Color(255f/255f, 233f/255f, 88f/255f), new Color(189f/255f, 191f/255f, 196f/225f), lerp));
                    break;
            }
        }
    }

    public void setFireMode(){
        if(SignalController.idToRoom.ContainsKey(id)){
            SignalController.room room = SignalController.idToRoom[id];
            if(room.sensors.fireDetected == 1){
                // Fire
                float lerp = (room.sensors.temperature - 0)/(50-0);
                setColour(Color.Lerp(new Color(255f/255f,147f/255f,0), new Color(255f/255f,20f/255f,0), lerp));
            }else{
                // No fire
                if(SignalController.staircase_ids.Contains(id)){
                    // This is a safe spot
                    setColour(new Color(0,1f,0));
                }else{
                    setColour(new Color(100f/255,100f/255,100f/255, 0.2f));
                }
            }
        }else{
            // No fire
            if(SignalController.staircase_ids.Contains(id)){
                // This is a safe spot
                setColour(new Color(0,1f,0));
            }else{
                setColour(new Color(100f/255,100f/255,100f/255, 0.2f));
            }
        }
    }

    public void setColour(Color color) {
        rend.material.color = color;
    }

    private int getFloor(){
        switch(id[0]){
            case '0':
                return 0;
            case '1':
                return 1;
            case '2':
                return 2;
            case '3':
                return 3;
            case '5':
                return 4;
            case '6':
                return 5;
            case '7':
                return 6;
        }
        Debug.LogError("Couldn't get overlap floor: " + id[0]);
        return -1;
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Setup a new material for this object
        rend = GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Standard"));
        rend.material.SetFloat("_Mode", 3);

        // setColour(Random.ColorHSV());x
    }
}
