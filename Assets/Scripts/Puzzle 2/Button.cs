using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
	private Outline outlinener;
	public Animator cageAnimator;
	public Animator puzzleAnimator;
	public void HideOutline() {
		outlinener.enabled = false;
	}

	public void Interaction() {
		cageAnimator.SetTrigger ("Open");
		puzzleAnimator.SetTrigger ("Destroy");
		FindObjectOfType<PlayerController> ().AudioPuzzle2();
	}

	public void ShowOutline() {
		outlinener.enabled = true;
	}

	// Start is called before the first frame update
	void Start()
    {
		outlinener = GetComponent<Outline> ();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
