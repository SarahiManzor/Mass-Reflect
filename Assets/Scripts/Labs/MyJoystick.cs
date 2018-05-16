using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyJoystick : Interactable {

    [SerializeField]
    private GameObject stick;
    [SerializeField]
    private GameObject frontAxis;
    private Rigidbody carBody;

    private Vector3 startRot;
    
    public float speedFactor = 1f;
    public float turnFactor = 1f;

    public override void OnHoldDown(GameObject g, GameObject s) {
        base.OnHoldDown(g, s);
        startRot = stick.transform.localEulerAngles;
        if (startRot.x > 90) {
            startRot.x -= 360;
        }
        if (startRot.y > 90) {
            startRot.y -= 360;
        }
        carBody = s.GetComponent<Rigidbody>();
    }

    void Update () {
        if (hand != null) {
            //Stick animation
            float xRot = -(hand.transform.localPosition.z - handStartPos.z) * 400f;
            float zRot = -(hand.transform.localPosition.x - handStartPos.x) * 400f;

            float x = Mathf.Clamp(xRot + startRot.x, -45f, 45f);
            float z = Mathf.Clamp(zRot + startRot.y, -45f, 45f);
            
            stick.transform.localRotation = Quaternion.Euler(x, z, 0f);

            //Car functionality
            carBody.velocity = (frontAxis.transform.forward * -x * speedFactor);
            selectedObject.transform.Rotate(0f, -z * turnFactor, 0f);
            frontAxis.transform.localRotation = Quaternion.Euler(0f, -z/2f, 0f);
        }
    }
}
