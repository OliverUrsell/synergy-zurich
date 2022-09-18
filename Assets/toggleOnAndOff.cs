using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleOnAndOff : MonoBehaviour
{
    public float delay = 1.0f;

    void Start(){
        Invoke("toggleActive", delay);
    }

    void toggleActive(){
        gameObject.SetActive(!gameObject.activeSelf);
        Invoke("toggleActive", delay);
    }
}
