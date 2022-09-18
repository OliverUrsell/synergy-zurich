using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SignalController : MonoBehaviour
{

    [System.Serializable]
    public class sensors {
        public float airQuality;
        public float brightness;
        public float fireDetected;
        public float presence;
        public float temperature;
    }

    [System.Serializable]
    public class room {
        public string id;
        public sensors sensors;
    }

    [System.Serializable]
    public class floor {
        public room[] rooms;
    }

    [System.Serializable]
    public class Response {
        public floor[] floors;
    }

    public enum Sensor{
        AIR_QUALITY,
        BRIGHTNESS,
        PRESENCE,
        TEMPERATURE,
    }

    public float refreshSeconds = 5.0f;
    private const string signalURL = "https://synergy-hackzurich.herokuapp.com/buildings/now";
    public static Sensor selectedSensor = Sensor.TEMPERATURE;
    public static List<string> staircase_ids = new List<string>{"701", "601", "501", "301", "201", "206", "207", "101", "001"};
    public static Dictionary<string, room> idToRoom = null;

    public Pill pill;

    public List<GameObject> fireEnable;
    public List<GameObject> fireDisable;

    public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(signalURL));
    }

    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(signalURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Received: " + request.downloadHandler.text);
                Response r = JsonUtility.FromJson<Response>(request.downloadHandler.text);

                // string json = "{\"floors\": [{\"rooms\": [{\"id\": \"001\",\"sensors\": {\"airQuality\": 400,\"brightness\": 500,\"fireDetected\": 1,\"presence\": 1,\"temperature\": 30}}]}]}";
                // Response r = JsonUtility.FromJson<Response>(json);
                
                // Generate a dictionary of room ids to sensor values
                bool fire = false;
                idToRoom = new Dictionary<string,room>();
                foreach(floor f in r.floors){
                    foreach(room rm in f.rooms){
                        idToRoom[rm.id] = rm;
                        if(rm.sensors.fireDetected == 1){
                            fire = true;
                        }
                    }
                }

                if(fire){
                    fireMode();
                }else{
                    // Reset the game objects that may have been changed in fire mode
                    foreach(GameObject go in fireEnable){
                        go.SetActive(false);
                    }

                    foreach(GameObject go in fireDisable){
                        go.SetActive(true);
                    }

                    updateOverlapColors();
                }

                // string json = "{\"floors\": [{\"rooms\": [{\"id\": \"001\",\"sensors\": {\"airQuality\": 400,\"brightness\": 500,\"fireDetected\": 0,\"presence\": 1,\"temperature\": 30}}]}]}";
                // Response r = JsonUtility.FromJson<Response>(json);
                // // Debug.Log(r.floors[0].rooms[0].id);
                // updateOverlapColors(r);
            }
        }

        Invoke("GenerateRequest", refreshSeconds);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateRequest();
    }

    void updateOverlapColors(){
        var overlapComponents = FindObjectsOfType<overlap>() as overlap[];
        foreach(overlap o in overlapComponents){
            o.setColorWithSignals();
        }
    }

    public void fireMode(){
        var overlapComponents = FindObjectsOfType<overlap>() as overlap[];
        foreach(overlap o in overlapComponents){
            o.setFireMode();
        }

        foreach(GameObject go in fireEnable){
            go.SetActive(true);
        }

        foreach(GameObject go in fireDisable){
            go.SetActive(false);
        }

    }

    public void setSensor(Sensor s){
        selectedSensor = s;
        pill.setPill(s);
        if(idToRoom != null){
            updateOverlapColors();
        }
    }

    public void setSensorAir(){
        setSensor(Sensor.AIR_QUALITY);
    }

    public void setSensorTemp(){
        setSensor(Sensor.TEMPERATURE);
    }

    public void setSensorBright(){
        setSensor(Sensor.BRIGHTNESS);
    }
}
