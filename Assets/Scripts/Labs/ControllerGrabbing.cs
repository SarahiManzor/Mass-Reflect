using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls grabbing physic functionality with objects that are interactable
/// </summary>
public class ControllerGrabbing : MonoBehaviour {

    [SerializeField]
    private GameObject grabParent;

    private FixedJoint fj;

    private SteamVR_TrackedObject controller;
    private SteamVR_Controller.Device device;

    [SerializeField]
    private Rigidbody rb;

    private float breakForce = 1e5f;

    private void Start() {
        controller = GetComponent<SteamVR_TrackedObject>();
    }

    private void Update() {
        device = SteamVR_Controller.Input((int)controller.index);

        rb.MovePosition(transform.position);
        rb.MoveRotation(transform.rotation);

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
            if (fj != null) {
                IsTeleporting(false);
                Rigidbody rb2 = fj.connectedBody;
                fj.connectedBody = null;
                Destroy(fj);
                fj = null;
                rb2.velocity /= 1.2f;
                rb2.angularVelocity /= 1.2f;
                if (!rb.name.Contains("Laser")) {
                    rb2.transform.SetParent(grabParent.transform);
                }
                rb2.mass = 1000f;
            }
        }
    }

    /// <summary>
    /// Sets fixed joint to not break on teleport if "tele" is true.
    /// </summary>
    /// <param name="tele">Is the characted teleporting</param>
    public void IsTeleporting(bool tele) {
        if (fj != null) {
            fj.breakForce = tele ? Mathf.Infinity : breakForce;
            fj.breakTorque = tele ? Mathf.Infinity : breakForce;
            fj.connectedBody.transform.parent = tele ? transform : null;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.transform.tag == "Grabbable") {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && fj == null) {
                fj = rb.gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = other.transform.GetComponent<Rigidbody>();
                fj.breakForce = breakForce;
                fj.breakTorque = breakForce;
                fj.connectedBody.mass = 100f;

                if (other.transform.parent != grabParent.transform && !other.transform.name.Contains("Laser"))
                {
                    fj.connectedBody.isKinematic = false;
                    other.transform.SetParent(grabParent.transform);
                    other.transform.GetComponent<Collider>().isTrigger = false;
                }
            }
        }
    }
}
