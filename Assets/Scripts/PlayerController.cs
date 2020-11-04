using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 10;
    public float viewSensitivity = 50f;
    public float interactionDistance = 1.5f;

    public static string selectedColor = "#";

    private CharacterController player;
    private Vector3 walkingDirection = Vector3.zero;

	private Vector2 lookDirection = Vector2.zero;

    [SerializeField]
    private Camera playerCamera = null;
    private float yRotation = 0f;
    private int layerMask;
    private IInteractable hittedObject;

    public Canvas generalInstructions;
    public Canvas imageInstructions;
    public Canvas anagramInstructions;
    public Canvas death;

    public Animator puzzleAnimator;
    public Animator cageAnimator;
    private float gravity =9.8f;

    public AudioClip awake;
    public AudioClip puzzle1;
    public AudioClip puzzle2;
    public AudioClip puzzle3;
    public AudioClip weird;
    public AudioClip end;

    public AudioClip firstTime;
    public AudioClip secondTime;

    public AudioSource audioData;


    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponentInParent<AudioSource> ();
        Cursor.lockState = CursorLockMode.Locked;
        player = GetComponent<CharacterController> ();
        player.Move (Vector3.zero);

        layerMask = 1 << 8;
        layerMask = ~layerMask;
        StartCoroutine ("ShowInstructions", generalInstructions);
        StartCoroutine ("Voice");
    }

    // Update is called once per frame
    void Update()
    {
        Movement ();
        MouseLook ();

        RaycastHit hit;
        if (Physics.Raycast (playerCamera.transform.position, playerCamera.transform.TransformDirection (Vector3.forward), out hit, interactionDistance, layerMask)) {
            Debug.DrawRay (playerCamera.transform.position, playerCamera.transform.TransformDirection (Vector3.forward) * hit.distance, Color.yellow);
            if (hittedObject != null && hittedObject != hit.collider.GetComponent<IInteractable> ()) {
                hittedObject.HideOutline ();
            }

            hittedObject = hit.collider.GetComponent<IInteractable> ();

            if (hittedObject != null) {
                hittedObject.ShowOutline ();
                if (Input.GetMouseButtonDown (0)) {
                    hittedObject.Interaction ();
                }
            }
        } else if (hittedObject != null) {
            hittedObject.HideOutline ();
        }

    }

    public void AudioPuzzle2() {
        audioData.clip = puzzle2;
        audioData.Play ();
    }
    public void AudioPuzzle3() {
        audioData.clip = puzzle3;
        audioData.Play ();
    }
    public void AudioWeird() {
        audioData.clip = weird;
        audioData.Play ();
    }

    public void AudioEnd() {
        audioData.clip = end;
        audioData.Play ();
    }
    public void Audio1t() {
        audioData.clip = firstTime;
        audioData.Play ();
    }

    public void Audio2t() {
        audioData.clip = secondTime;
        audioData.Play ();
    }

    public void ShowImageInstructions() {
        generalInstructions.enabled = false;
        StartCoroutine ("ShowInstructions", imageInstructions);
    }

    public void StopInstructions() {
        StopAllCoroutines ();
    }

	private void Movement() {
        float speed = Input.GetKey (KeyCode.LeftShift) ? baseSpeed * 2 : baseSpeed;

        walkingDirection = transform.right * Input.GetAxis ("Horizontal") + transform.forward * Input.GetAxis ("Vertical");
        walkingDirection = walkingDirection * Time.deltaTime * speed;
        if (!player.isGrounded) {
            walkingDirection.y -= gravity * Time.deltaTime;
        } else {
            walkingDirection.y = 0;
        }
        player.Move (walkingDirection);
    }

    private void MouseLook() {
        lookDirection = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y")) * Time.deltaTime * viewSensitivity;

        yRotation -= lookDirection.y;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);

        transform.Rotate (Vector3.up, lookDirection.x);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation,0f,0f);
    }

    IEnumerator Voice() {
        yield return new WaitForSeconds (1);
        audioData.clip = awake;
        audioData.Play ();
    }
    IEnumerator ShowInstructions(Canvas ci) {
        ci.enabled = true;
        yield return new WaitForSeconds (10);
        ci.enabled = false;
        yield return new WaitForSeconds (0.2f);
        ci.enabled = true;
        yield return new WaitForSeconds (0.5f);
        ci.enabled = false;
        yield return new WaitForSeconds (0.2f);
        ci.enabled = true;
        yield return new WaitForSeconds (0.5f);
        ci.enabled = false;
        yield return new WaitForSeconds (0.1f);
        ci.enabled = true;
        yield return new WaitForSeconds (0.2f);
        ci.enabled = false;
        yield return new WaitForSeconds (0.1f);
        ci.enabled = true;
        yield return new WaitForSeconds (0.2f);
        ci.enabled = false;
    }

	private void OnTriggerEnter(Collider other) {
        Debug.Log (other.name);
        if (other.name == "Trigger") {
            cageAnimator.SetTrigger ("DestroyCage");
            FindObjectOfType<PuzzleManager> ().puzzle2IsDone = true;
            AudioPuzzle3 ();
        }

        if (other.name == "DeathZone") {
            StartCoroutine ("DeathScreen");
        }

        if (other.name == "Puzzle 1 trigger") {
            other.enabled = false;
            audioData.clip = puzzle1;
            audioData.Play ();
        }
    }



    IEnumerator DeathScreen() {
        yield return new WaitForSeconds (1f);
        death.enabled = true;
        death.GetComponent<GraphicRaycaster> ().enabled = true;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds (0.5f);
        gravity = 0;
    }

    public void Restart() {
        StartCoroutine ("RestartCR");
    }

    IEnumerator RestartCR() {
        puzzleAnimator.SetTrigger ("Restart");
        cageAnimator.SetTrigger ("DestroyCage");
        yield return null;
        cageAnimator.SetTrigger ("Appear");
        player.enabled = false;
        yield return null;
        transform.position = new Vector3 (-6.5f, 1.1f, 7.6f);
        transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 0f));
        yield return null;
        player.enabled = true;
        gravity = 9.8f;
        death.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
