using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functionality for shooting bouncy lasers
/// </summary>
public class LaserShooter : MonoBehaviour {

    public int maxBounces = 10;
    
    private bool lasering = false;

    [SerializeField]
    private LineRenderer rayLr;
    [SerializeField]
    private LayerMask laserMask;

    private List<GameObject> hitObjects = new List<GameObject>();

    [SerializeField]
    private Material glowingMat;
    [SerializeField]
    private Material defaultMat;

    [SerializeField]
    private int totalOrbs = 0;
    [SerializeField]
    private GameObject[] doors;
    private Vector3[] doorStartPos;

    private void Start() {
        doorStartPos = new Vector3[doors.Length];
        for (int i = 0; i < doors.Length; i++) {
            doorStartPos[i] = doors[i].transform.position;
        }
    }

    void Update() {
        CheckLaser();
    }

    /// <summary>
    /// Check for laser button input and shoot a bouncy laser.
    /// </summary>
    private void CheckLaser() {
        for (int i = 0; i < doors.Length; i++) {
            if (hitObjects.Count == totalOrbs) {
                doors[i].transform.position = Vector3.Lerp(doors[i].transform.position, doorStartPos[i] + Vector3.up * 2f, 0.025f);
            }
            else {
                doors[i].transform.position = Vector3.Lerp(doors[i].transform.position, doorStartPos[i], 0.025f);
            }
        }

        foreach (GameObject g in hitObjects) {
            g.transform.GetComponent<Renderer>().material = defaultMat;
            g.transform.GetComponent<Collider>().enabled = true;
        }
        hitObjects.Clear();
        RaycastHit hit;
        Vector3 pos = transform.position;
        Vector3 direction = transform.forward;
        rayLr.positionCount = 1;
        rayLr.SetPosition(0, transform.position);
        while (Physics.Raycast(pos, direction, out hit, Mathf.Infinity, laserMask)) { //Shoots a raycast out from the last position hit and in a new direction
            if (hit.transform.CompareTag("Objective")) { //If the raycast hits an objective object, keeps track of that object and shoots out a new raycast from behind that object. 
                hitObjects.Add(hit.transform.gameObject);
                //hit.transform.gameObject.SetActive(false);
                hit.transform.GetComponent<Renderer>().material = glowingMat;
                hit.transform.GetComponent<Collider>().enabled = false;
                pos = hit.point;
            }
            else {
                rayLr.positionCount = rayLr.positionCount + 1;
                rayLr.SetPosition(rayLr.positionCount - 1, hit.point);
                pos = hit.point;
                direction = direction - 2 * (Vector3.Dot(direction, hit.normal.normalized)) * hit.normal.normalized;
                if (rayLr.positionCount > 10 || hit.transform.CompareTag("Ground")) {
                    break;
                }
            }
        }

        if (!Physics.Raycast(pos, direction, out hit, Mathf.Infinity)) {
            rayLr.positionCount = rayLr.positionCount + 1;
            rayLr.SetPosition(rayLr.positionCount - 1, pos + direction * 100f);
        }
    }
}
