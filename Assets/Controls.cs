using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

    public int minimum_camera_y = 20;
    public int maximum_camera_y = 50;
    public float movement_multiplier = 1f;

    public Transform cameraRotationPoint;
    public float rotation_multiplier = 1f;

    public Light light;
    private float lightNormal = 358.5F;
    private float lightFocused = 90F;

    private bool focused = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            if(focused && touch.phase == TouchPhase.Began){
                focused = false;
                returnToPosition();
            }else{

                if(touch.phase == TouchPhase.Began && touch.tapCount >= 2)
                {
                    Ray screenRay = Camera.main.ScreenPointToRay(touch.position);

                    RaycastHit hit;
                    if (Physics.Raycast(screenRay, out hit))
                    {
                        // Find out if we hit a tile or a floor object
                        overlap o = hit.collider.gameObject.GetComponent(typeof(overlap)) as overlap;
                        if(o != null){
                            focus(hit.collider.gameObject.transform.parent.gameObject.GetComponent(typeof(Floor)) as Floor);
                        }else{
                            Floor f = hit.collider.gameObject.GetComponent(typeof(Floor)) as Floor;
                            if(f != null){
                                focus(f);
                            }
                        }
                    }
                }

                if(touch.phase == TouchPhase.Moved){
                    // Process up down movement
                    float new_y = transform.position.y + touch.deltaPosition.y * movement_multiplier;
                    if(new_y > maximum_camera_y) new_y = maximum_camera_y;
                    if(new_y < minimum_camera_y) new_y = minimum_camera_y;
                    transform.position = new Vector3(transform.position.x, new_y, transform.position.z);

                    // Process side to side movement
                    transform.RotateAround(cameraRotationPoint.position, Vector3.up, touch.deltaPosition.x * rotation_multiplier * Time.deltaTime);
                }
            }
        }
    }

    void returnToPosition(){
        // Show all the floors
        foreach(Floor fl in FindObjectsOfType<Floor>(true) as Floor[]){
            fl.gameObject.SetActive(true);
        }

        focused = false;
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        light.range = lightNormal;
    }

    void focus(Floor f){

        // Hide all other floors
        foreach(Floor fl in FindObjectsOfType<Floor>() as Floor[]){
            if(fl != f){
                fl.gameObject.SetActive(false);
            }
        }

        focused = true;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        transform.position = f.focusPoint.position;
        transform.rotation = f.focusPoint.rotation;

        light.range = lightFocused;
    }

}
