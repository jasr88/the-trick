using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Key : MonoBehaviour, IInteractable
{
	private Outline outliner;

	public Animator door;
	public Animator dog;

	private PuzzleManager pm;
	private int count = 0;
	private bool dogIsMoving;
	public Image back;
	public Text message;

	public PlayerController p;

	public void HideOutline() {
		outliner.enabled = false;
		pm = FindObjectOfType<PuzzleManager> ();
	}

	public void Interaction() {
		if (pm.puzzle1IsDone && pm.puzzle2IsDone && pm.puzzle3IsDone) {
			StartCoroutine ("OpenDoor");
		} else if (count == 0) {
			count++;
			p.Audio1t ();
		} else {
			p.Audio2t ();
		}
	}

	public void ShowOutline() {
		outliner.enabled = true;
	}

	// Start is called before the first frame update
	void Start()
    {
		outliner = GetComponent<Outline> ();
		
		back.CrossFadeAlpha (0, 0.1f, false);
		message.CrossFadeAlpha (0, 0.1f, false);
		p = FindObjectOfType<PlayerController> (); 
	}

	void Update() {
		if (dogIsMoving) {
			dog.transform.Translate (Vector3.forward * Time.deltaTime*0.5f);
		}
		
	}

	IEnumerator OpenDoor() {
		GetComponentInParent<Animator> ().SetTrigger ("Use");
		yield return new WaitForSeconds (3);
		door.SetTrigger ("Open");
		yield return new WaitForSeconds (3);
		dog.SetTrigger ("Ending");
		yield return new WaitForSeconds (2.5f);
		p.AudioEnd ();
		dogIsMoving = true;
		back.GetComponentInParent<Canvas> ().enabled = true;
		yield return new WaitForSeconds (3.5f);
		back.CrossFadeAlpha (1f, 2f, false);
		yield return new WaitForSeconds (1.5f);
		message.CrossFadeAlpha (1f, 2f, false);
	}
}
