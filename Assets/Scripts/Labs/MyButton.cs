using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : Interactable {

    [SerializeField]
    private GameObject[] objects;
    
    public override void OnHoldDown(GameObject h, GameObject s) {
        Debug.Log("Button pressed");
        //s.SetActive(!s.activeSelf);

        for (int i = 0; i < objects.Length; i++) {
            objects[i].transform.localPosition = Vector3.zero + Vector3.right * (i - 1);
            objects[i].transform.rotation = new Quaternion();
            objects[i].transform.localScale = Vector3.one * 0.4f;
        }
    }

    public override void OnHoldUp() {
        
    }
}
