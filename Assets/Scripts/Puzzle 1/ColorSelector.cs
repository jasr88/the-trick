using UnityEngine;

public class ColorSelector : MonoBehaviour, IInteractable
{
	private Outline outlinener;
	public Colors setOfColors;
	public void HideOutline() {
		outlinener.enabled = false;
	}

	public void Interaction() {
		PlayerController.selectedColor = setOfColors.correctColorString;
	}

	public void ShowOutline() {
		outlinener.enabled = true;
	}

	// Start is called before the first frame update
	void Start()
    {
		outlinener = GetComponent<Outline> ();
		Renderer r = GetComponent<Renderer> ();
		r.material.EnableKeyword ("_EMISSION");
		r.material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
		r.material.SetColor ("_Color", setOfColors.correctColor);
		r.material.SetColor ("_EmissionColor", setOfColors.correctColor);
	}
}
