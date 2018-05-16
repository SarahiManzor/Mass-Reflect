using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDial : Interactable {

    private Vector3 startRot;
    private Quaternion handStartRot;

    private Vector3 selectedStartScale;
    
    private float totalRotation = 0;
    private float lastRotation = 0;
    private int wholeRotations = 0;
    
    public override void OnHoldDown(GameObject g, GameObject s) {
        if (s != selectedObject) {
            if (selectedObject == null) {
                selectedStartScale = s.transform.localScale;
            }
            else {
                selectedStartScale = selectedObject.transform.localScale;
            }
            transform.localRotation = Quaternion.Euler(0f,0f,0f);
            totalRotation = 0;
            lastRotation = 0;
            wholeRotations = 0;
        }
        base.OnHoldDown(g, s);
        startRot = transform.localEulerAngles;
        handStartRot = hand.transform.rotation;
        lastRotation = transform.localRotation.eulerAngles.z;
    }

    void Update() {
        if (hand != null) {
            //Debug.Log("Moving dial.");

            float handRotChange = handStartRot.eulerAngles.z - hand.transform.rotation.eulerAngles.z;
            transform.localRotation = Quaternion.Euler(startRot.x, startRot.y, startRot.z + handRotChange);

            float rotChange = transform.localRotation.eulerAngles.z - lastRotation;

            totalRotation += rotChange;

            if (Mathf.Abs(lastRotation - totalRotation) > 340f) {
                if (lastRotation > 310f) {
                    Debug.Log("Add whole");
                    wholeRotations++;
                }else if (lastRotation < 50f) {
                    Debug.Log("Subtract whole");
                    wholeRotations--;
                }
            } 

            //Debug.Log(lastTotalRotation - totalRotation);
            Debug.Log("LastRot: " + lastRotation);
            Debug.Log("TotalRot: " + totalRotation);
            lastRotation = totalRotation;
            //Debug.Log(totalRotation);

            if (selectedObject != null) {
                selectedObject.transform.localScale = selectedStartScale + Vector3.one * (totalRotation + 360f * wholeRotations) / 100f;
            }

            lastRotation = transform.localRotation.eulerAngles.z;
        }
    }
}
