using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySwitch : Interactable {

    private bool switchOn = false;

    public override void OnHoldDown(GameObject h, GameObject s) {
        switchOn = !switchOn;
        Debug.Log("Switch on? " + switchOn);
    }
}
