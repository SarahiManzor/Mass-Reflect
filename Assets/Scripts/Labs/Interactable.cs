using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected GameObject hand;
    protected GameObject selectedObject;
    protected Vector3 handStartPos;

    public bool isDescrete = false;
    
    /// <summary>
    /// Called when user grips an object
    /// </summary>
    /// <param name="h">The controller gameobject</param>
    public virtual void OnHoldDown(GameObject h, GameObject s) {
        hand = h;
        selectedObject = s;
        hand.transform.GetChild(0).gameObject.SetActive(false);
        handStartPos = hand.transform.localPosition;
    }

    /// <summary>
    /// Called when user releases grip on an object.
    /// </summary>
    public virtual void OnHoldUp() {
        if (hand != null) hand.transform.GetChild(0).gameObject.SetActive(true);
        hand = null;
        selectedObject = null;
    }
}
