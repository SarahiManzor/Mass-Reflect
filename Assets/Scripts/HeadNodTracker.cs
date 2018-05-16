using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadNodTracker : MonoBehaviour {

    [SerializeField]
    private GameObject head;
    private GameObject idealHead;
    private GuideDude guideDude;

    private float low = 0f;
    private float high = 0f;

    private float lastOffangle = 0;
    private float offsetRange = 2f;

    private bool noddingUp = false;

    public float threshhold = 7.5f;
    private float timeToNod = 2f;

    private float nodStartTime;
    private float totalNods = 0;

    public bool isYes = true;

    private void Start() {
        guideDude = GetComponent<GuideDude>();
        idealHead = GameObject.Instantiate(new GameObject());
        idealHead.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update() {
        if (FacingGuide()) {
            CheckNod();
        }
    }

    private bool FacingGuide() {
        idealHead.transform.position = head.transform.position;
        idealHead.transform.LookAt(guideDude.transform);
        float angle = Quaternion.Angle(idealHead.transform.rotation, head.transform.rotation);
        Debug.Log(angle);

        return angle < 60 && Vector3.Distance(head.transform.position, guideDude.transform.position) < 5f;
    }

    void CheckNod() {
        float headAngle = isYes ? head.transform.localEulerAngles.x : head.transform.localEulerAngles.y;
        if (headAngle > 180) {
            headAngle -= 360;
        }
        headAngle *= -1;

        if (totalNods > 0) {
            float timeSinceStart = Time.time - nodStartTime;

            if (timeSinceStart > timeToNod) {
                totalNods = 0;
            }
        }

        if (!noddingUp) {
            if (headAngle < low) {
                low = headAngle;
            }
            float angleChange = Mathf.Abs(headAngle - low);
            if (angleChange > threshhold) {
                noddingUp = true;
                high = headAngle;
                if (totalNods == 0) {
                    nodStartTime = Time.time;
                }
                else {
                    float offsetAngleChange = Mathf.Abs(lastOffangle - (isYes ? head.transform.localEulerAngles.y : head.transform.localEulerAngles.x));
                    if (offsetAngleChange > offsetRange) {
                        totalNods = 0;
                        return;
                    }
                }
                totalNods++;
                lastOffangle = isYes ? head.transform.localEulerAngles.y : head.transform.localEulerAngles.x;
            }
        }
        else if (noddingUp) {
            if (headAngle > high) {
                high = headAngle;
            }
            float angleChange = Mathf.Abs(headAngle - high);
            if (angleChange > threshhold) {
                noddingUp = false;
                low = headAngle;
                if (totalNods == 0) {
                    nodStartTime = Time.time;
                }else {
                    float offsetAngleChange = Mathf.Abs(lastOffangle - (isYes ? head.transform.localEulerAngles.y : head.transform.localEulerAngles.x));
                    if (offsetAngleChange > offsetRange) {
                        totalNods = 0;
                        return;
                    }
                }
                totalNods++;
                lastOffangle = isYes ? head.transform.localEulerAngles.y : head.transform.localEulerAngles.x;
            }
        }

        if (totalNods > 2) {
            //guideDude.StartUpdateText(isYes ? "Yes" : "No");
            guideDude.Respond(isYes);
            noddingUp = false;
            totalNods = 0;
        }
    }
}
