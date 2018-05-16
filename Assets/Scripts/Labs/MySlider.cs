using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySlider : Interactable {

    [SerializeField]
    private Material itemPickupOutline;
    private Vector3 startPos;

    public void Start() {
        itemPickupOutline.SetFloat("g_flOutlineWidth", 0.0025f);
    }

    public override void OnHoldDown(GameObject g, GameObject s) {
        base.OnHoldDown(g, s);
        startPos = transform.position;
    }

    void Update () {
        if (hand != null) {
            float changeZ = handStartPos.z - hand.transform.position.z;
            float localXPos = transform.parent.position.z - (startPos.z - changeZ);
            localXPos = Mathf.Clamp(localXPos, -0.08f, 0.08f);
            transform.localPosition = new Vector3(localXPos, transform.localPosition.y, transform.localPosition.z);
        
            itemPickupOutline.SetFloat("g_flOutlineWidth", (-localXPos + 0.08f) / 0.16f * 0.01f + 0.0025f);
        }
	}
}
