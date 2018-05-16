using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls teleporting functionality
/// </summary>
public class Teleporter : MonoBehaviour {

    [SerializeField]
    private LineRenderer tpLr;
    [SerializeField]
    private GameObject camRig;
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private LayerMask teleportMask;

    [SerializeField]
    private GameObject eye;

    private SteamVR_TrackedObject controller;
    private SteamVR_Controller.Device device;

    private int lineCount = 0;
    private float segmentLength = 0.4f;

    [SerializeField]
    private float rayDrop = 0.08f;

    [SerializeField]
    private ControllerGrabbing[] controllerGrabbers; //References to controllerGrabber script in order to not break joints on teleport

    private Coroutine runningRoutine;

    private void Start() {
        controller = GetComponent<SteamVR_TrackedObject>();
    }

    private void Update() {
        device = SteamVR_Controller.Input((int)controller.index);

        CheckTeleport();
    }

    /// <summary>
    /// Checks for teleport input and acts accordingly
    /// </summary>
    private void CheckTeleport() {
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
            lineCount = 0;

            while (lineCount < 100) {
                if (lineCount == 0) { //The initial line segment is always set to shoot directly out of controller.
                    lineCount = 1;
                    tpLr.positionCount = 2;
                    tpLr.SetPosition(0, transform.position);
                    tpLr.SetPosition(1, transform.position + transform.forward * segmentLength);
                }
                else { //For each additional line segment just use the previous point and do maths
                    lineCount += 1;
                    tpLr.positionCount = lineCount + 1;
                    tpLr.SetPosition(tpLr.positionCount - 1, tpLr.GetPosition(tpLr.positionCount - 2) + transform.forward * segmentLength + Vector3.down * (rayDrop * segmentLength) * lineCount);
                }

                Vector3 direction = tpLr.GetPosition(lineCount) - tpLr.GetPosition(lineCount - 1);
                float distance = Vector3.Distance(tpLr.GetPosition(lineCount), tpLr.GetPosition(lineCount - 1));
                RaycastHit hit;

                if (Physics.Raycast(tpLr.GetPosition(lineCount - 1), direction, out hit, segmentLength * 2f, teleportMask)) { //Only draws amount of lines needed
                    if (hit.transform.tag == "Ground" && hit.normal.normalized == Vector3.up) { //Only draws indicator raycast hits an object of tag ground and has a normal vector of 0,1,0
                        indicator.transform.position = hit.point;
                        indicator.gameObject.SetActive(true);
                        tpLr.SetPosition(tpLr.positionCount - 1, hit.point);
                        tpLr.positionCount = lineCount + 1;
                        break;
                    }
                    indicator.gameObject.SetActive(false);
                    tpLr.SetPosition(tpLr.positionCount - 1, hit.point);
                    tpLr.positionCount = lineCount + 1;
                    break;
                }
                indicator.gameObject.SetActive(false);
            }
        }
        else if (tpLr.positionCount > 0) { //Checks the final line segment and checks for valid warp.
            Vector3 direction = tpLr.GetPosition(lineCount) - tpLr.GetPosition(lineCount - 1);
            RaycastHit hit;
            if (Physics.Raycast(tpLr.GetPosition(lineCount - 1), direction, out hit, segmentLength * 2f, teleportMask)) {
                if (hit.transform.tag == "Ground" && hit.normal.normalized == Vector3.up) {
                    Vector3 offset = camRig.transform.position - eye.transform.position + Vector3.up * (eye.transform.position.y - camRig.transform.position.y);
                    if (runningRoutine != null) {
                        StopCoroutine(runningRoutine);
                        controllerGrabbers[0].IsTeleporting(false);
                        controllerGrabbers[1].IsTeleporting(false);
                    }
                    runningRoutine = StartCoroutine(MovePlayer(hit.point + offset));
                }
            }
            tpLr.positionCount = 0;
            lineCount = 0;

            indicator.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Moves the player to target location 
    /// </summary>
    /// <param name="target">Target position</param>
    private IEnumerator MovePlayer(Vector3 target) {
        controllerGrabbers[0].IsTeleporting(true);
        controllerGrabbers[1].IsTeleporting(true);
        SteamVR_Fade.Start(Color.black, .2f);
        yield return new WaitForSeconds(.2f);
        camRig.transform.position = target;
        SteamVR_Fade.Start(Color.clear, .2f);
        yield return new WaitForSeconds(.2f);
        controllerGrabbers[0].IsTeleporting(false);
        controllerGrabbers[1].IsTeleporting(false);
    }
}