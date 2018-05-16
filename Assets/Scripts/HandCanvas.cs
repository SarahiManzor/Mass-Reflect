using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCanvas : MonoBehaviour
{

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject[] spawnables;

    private Vector3[] spawnableStartPos;
    private Quaternion[] spawnableStartRot;
    private Vector3[] spawnableStartScale;

    // Use this for initialization
    void Start()
    {
        spawnableStartPos = new Vector3[spawnables.Length];
        spawnableStartRot = new Quaternion[spawnables.Length];
        spawnableStartScale = new Vector3[spawnables.Length];
        for (int i = 0; i < spawnables.Length; i++)
        {
            spawnableStartPos[i] = spawnables[i].transform.localPosition;
            spawnableStartRot[i] = spawnables[i].transform.localRotation;
            spawnableStartScale[i] = spawnables[i].transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localEulerAngles.z > 90f && transform.localEulerAngles.z < 270f)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }

        for (int i = 0; i < spawnables.Length; i++)
        {
            if (spawnables[i].transform.parent != canvas.transform)
            {
                spawnables[i].GetComponent<CanvasGrabbable>().Release();
                GameObject newSpawnable = GameObject.Instantiate(spawnables[i]);
                newSpawnable.GetComponent<Rigidbody>().isKinematic = true;
                newSpawnable.GetComponent<Collider>().isTrigger = true;
                newSpawnable.transform.SetParent(canvas.transform);
                newSpawnable.transform.localPosition = spawnableStartPos[i];
                newSpawnable.transform.localRotation = spawnableStartRot[i];
                newSpawnable.transform.localScale = spawnableStartScale[i];
                spawnables[i] = newSpawnable;
            }
        }
    }
}
