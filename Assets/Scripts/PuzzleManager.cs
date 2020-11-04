using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public PuzzleImage[] puzzlesE1;
	public bool puzzle1IsDone;
	public bool puzzle2IsDone;
    public bool puzzle3IsDone;

	public Animator cageAnimator;
	public GameObject button;

	public GameObject anagram;

	private void Update() {
		if (puzzlesE1[0].isSolved && puzzlesE1[1].isSolved && puzzlesE1[2].isSolved && puzzlesE1[3].isSolved && !puzzle1IsDone) {
			puzzle1IsDone = true;
			FinishP1 ();
		}
		if (puzzle2IsDone) {
			anagram.SetActive (true);
		}

		if (LetterButton.Solved == 10 &&  !puzzle3IsDone) {
			puzzle3IsDone = true;
			Debug.LogError ("Use the key");
			FindObjectOfType<PlayerController> ().AudioWeird ();
		}
	}

	private void FinishP1() {
		Debug.Log ("Completaste el Puzzle 1");

		button.SetActive (true);
		cageAnimator.SetTrigger ("Appear");
	}

}
