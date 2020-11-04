using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterButton : MonoBehaviour, IInteractable
{
    public string correctLetter = "";
    public string[] letters;

    private Material mat;
    public Color correctColor;
    private Text Letter;

    private Outline outliner;
    private int index;

    public static int Solved = 0;

    public AudioSource audioData;

    public AudioClip c;
    public AudioClip i;

    // Start is called before the first frame update
    void Start()
    {
        outliner = GetComponent<Outline> ();
        mat = GetComponent<Renderer> ().material;
        index = Random.Range (0, letters.Length-1);
        Letter = GetComponentInChildren<Text> ();
        Letter.text = letters[index];
        audioData = GetComponentInParent<AudioSource> ();
    }

	public void Interaction() {
        if (mat.color == correctColor)
            return;
        ChangeIndex ();
        Letter.text = letters[index];
        if (letters[index] == correctLetter) {
            mat.SetColor ("_Color", correctColor);
            mat.EnableKeyword ("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            mat.SetColor ("_EmissionColor", correctColor);
            Solved++;
            Letter.color = Color.black;
            audioData.clip = c;
            audioData.Play ();
        } else {
            mat.SetColor ("_Color", Color.red);
            mat.EnableKeyword ("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            mat.SetColor ("_EmissionColor", Color.red);
            audioData.clip = i;
            audioData.Play ();
        }
	}

	public void ShowOutline() {
        outliner.enabled = true;

    }

	public void HideOutline() {
        outliner.enabled = false;
    }

    private void ChangeIndex() {
        if (index == letters.Length - 1) {
            index = 0;
        } else {
            index++;
        }
    }
}
