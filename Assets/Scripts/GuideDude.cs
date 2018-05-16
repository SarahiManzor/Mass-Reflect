using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideDude : MonoBehaviour {

    [SerializeField]
    private GameObject[] eyes;
    [SerializeField]
    private GameObject playerHead;

    private bool recentering;
    private float recenterStartTime;

    [SerializeField]
    private TextMesh words;

    private Coroutine textRoutine;

    private string playerResponse = "Yes";

    private int textIndex = 0;

    public bool talkerDude = true;

    public float floatingOffset = 0f;
    public float floatingSpeed = 1.5f;

    private Vector3 startPos;

    void Start() {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        CheckRotations();

        if (textRoutine == null && talkerDude) {
            RunNextText();
        }
    }

    private void RunNextText() {
        if (textIndex == 0) {
            StartUpdateText("Hey there! I will be\nyou guide for today.\nNod for me to get started.");
            textIndex++;
            playerResponse = null;
        }
        else if (textIndex == 1) {
            if (playerResponse == "Yes") {
                StartUpdateText("Nice job! Now shake\nyour head at me.");
                textIndex++;
                playerResponse = null;
            }
            else if (playerResponse == "No") {
                StartUpdateText("Just shake your\nhead up and down!");
                playerResponse = null;
            }
        }
        else if (textIndex == 2) {
            if (playerResponse == "No") {
                StartUpdateText("Well done! Thats all im good for\nright now.Would you like to see\nmy nod reading capabilities again ?");
                textIndex++;
                playerResponse = null;
            }
            else if (playerResponse == "Yes") {
                StartUpdateText("Just shake your\nhead left and right!");
                playerResponse = null;
            }
        }
        else if (textIndex == 3)
        {
            if (playerResponse == "No")
            {
                StartUpdateText("Have fun with the course!\nThe controls are shown to your right.");
                textIndex++;
                playerResponse = null;
            }
            else if (playerResponse == "Yes")
            {
                StartUpdateText("Sounds good!!!");
                textIndex = 0;
                playerResponse = null;
            }
        }
    }

    public void Respond(bool isYes) {
        if (textRoutine == null) {
            playerResponse = isYes ? "Yes" : "No";
        }
    }

    public void StartUpdateText(string newString) {
        if (textRoutine == null) {// && newString != words.text) {
            textRoutine = StartCoroutine(UpdateText(newString));
        }
    }

    private IEnumerator UpdateText(string newString) {
        int charCount = 0;
        yield return new WaitForSeconds(.25f);
        words.text = "";
        while (words.text.Length < newString.Length) {
            words.text += newString.ToCharArray()[charCount];
            charCount++;
            yield return new WaitForSeconds(.05f);
        }
        textRoutine = null;
    }

    private void CheckRotations() {
        foreach (GameObject g in eyes) {
            if ((g.transform.localEulerAngles.y > 50f && g.transform.localEulerAngles.y < 310f) || recentering) {
                if (!recentering) {
                    recentering = true;
                    recenterStartTime = Time.time;
                }
                else if (Time.time - recenterStartTime > 2f) {
                    recentering = false;
                }
                Vector3 targetPos = playerHead.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetPos);
                targetRotation.x = 0f;
                targetRotation.z = 0f;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, .01f);
            }

            //if (Vector3.Distance(transform.position, playerHead.transform.position) > 2.5f) {
            //    transform.position = Vector3.Lerp(transform.position, playerHead.transform.position, 0.001f);
            //    recentering = true;
            //    recenterStartTime = Time.time;
            //}
            g.transform.LookAt(playerHead.transform);
        }

        transform.position = startPos + Vector3.up * 0.05f * Mathf.Sin((Time.time + floatingOffset) * floatingSpeed);
    }
}
