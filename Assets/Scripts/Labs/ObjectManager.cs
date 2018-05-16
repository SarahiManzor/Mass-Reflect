using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public GameObject selectedObject;

    [SerializeField]
    private TextMesh scaleText;
    [SerializeField]
    private TextMesh positionText;

    private void Update() {
        if (selectedObject != null) {
            scaleText.text = selectedObject.transform.lossyScale.ToString();
            positionText.text = selectedObject.transform.position.ToString();
        }
    }
}
