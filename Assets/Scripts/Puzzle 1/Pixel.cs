using System.Collections;
using UnityEngine;

public class Pixel : MonoBehaviour, IInteractable
{
	private Outline outlinener;
	public Material mat;

	public Color neutralColor = new Color(0.2f,0.2f,0.2f);
	public Colors setOfColors;

	public AudioSource audioData;

	public AudioClip c;
	public AudioClip i;

	public void Start() {
		audioData = GetComponentInParent<AudioSource> ();
		outlinener = GetComponent<Outline> ();
		mat = GetComponent<Renderer> ().material;

		mat.EnableKeyword ("_EMISSION");
		mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
		mat.SetColor ("_Color", neutralColor);
		mat.SetColor ("_EmissionColor", neutralColor);
	}

	public void HideOutline() {
		outlinener.enabled = false;
	}

	internal void ChangeCorrectColor() {
		mat.color = setOfColors.correctColor;

		mat.EnableKeyword ("_EMISSION");
		mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
		mat.SetColor ("_Color", setOfColors.correctColor);
		mat.SetColor ("_EmissionColor", setOfColors.correctColor);
	}

	public void Interaction() {
		Color tempColor;
		if (setOfColors.correctColorString == PlayerController.selectedColor) {
			mat.SetColor("_Color", setOfColors.correctColor);
			GetComponentInParent<PuzzleImage> ().PaintAllPixelsOfColor (setOfColors.correctColorString);
			audioData.clip = c;
			audioData.Play ();
			GetComponent<Collider> ().enabled = false;
		} else if(ColorUtility.TryParseHtmlString(PlayerController.selectedColor, out tempColor) && mat.color != setOfColors.correctColor) {
			mat.SetColor ("_Color", tempColor);
			StartCoroutine ("BlinkColor", tempColor);
			audioData.clip = i;
			audioData.Play ();
		}
		if (PlayerController.selectedColor == "#") {
			PlayerController pc = FindObjectOfType<PlayerController> ();
			pc.StopInstructions ();
			pc.ShowImageInstructions ();
		}
	}

	public void ShowOutline() {
		outlinener.enabled = true;
	}

	public void StopBlinking() {
		StopAllCoroutines ();
	}

	IEnumerator BlinkColor(Color colorBlink) {
		yield return new WaitForSeconds (0.3f);
		mat.SetColor ("_Color", neutralColor);
		mat.SetColor ("_EmissionColor", neutralColor);
		yield return new WaitForSeconds (0.3f);
		mat.SetColor ("_Color", colorBlink);
		mat.SetColor ("_EmissionColor", colorBlink);
		yield return new WaitForSeconds (0.3f);
		mat.SetColor ("_Color", neutralColor);
		mat.SetColor ("_EmissionColor", neutralColor);
		yield return new WaitForSeconds (0.3f);
		mat.SetColor ("_Color", colorBlink);
		mat.SetColor ("_EmissionColor", colorBlink);
		yield return new WaitForSeconds (0.3f);
		mat.SetColor ("_Color", neutralColor);
		mat.SetColor ("_EmissionColor", neutralColor);
		yield return new WaitForSeconds (0.3f);
		mat.SetColor ("_Color", setOfColors.correctColor);
		mat.SetColor ("_EmissionColor", setOfColors.correctColor);
		yield return new WaitForSeconds (0.1f);
		mat.SetColor ("_Color", neutralColor);
		mat.SetColor ("_EmissionColor", neutralColor);
	}



}
