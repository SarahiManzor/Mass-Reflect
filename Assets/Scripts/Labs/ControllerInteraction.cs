using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller interaction with various inputs
/// </summary>
public class ControllerInteraction : MonoBehaviour {

    private SteamVR_TrackedObject controller;
    private SteamVR_Controller.Device device;

    private Interactable interactingObject; //The object currently being interacted with
    public GameObject selectedObject; //The object we will modify
    private bool interacting = false;

    private void Start() {
        controller = GetComponent<SteamVR_TrackedObject>();
    }
    
    void Update() {
        device = SteamVR_Controller.Input((int)controller.index);

        if (interactingObject != null && !interactingObject.isDescrete) {
            device.TriggerHapticPulse(900);
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
            if (interactingObject != null) {
                interactingObject.OnHoldUp();
                interactingObject = null;
                interacting = false;
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Interactable" && !interacting) {
            device.TriggerHapticPulse(900);
        }
    }

    private void OnTriggerStay(Collider other) {
        device = SteamVR_Controller.Input((int)controller.index);
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && other.transform.tag == "Interactable" && !interacting) {
            device.TriggerHapticPulse(900);
            interactingObject = other.transform.GetComponent<Interactable>();
            interactingObject.OnHoldDown(transform.gameObject, selectedObject);
            interacting = true;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}