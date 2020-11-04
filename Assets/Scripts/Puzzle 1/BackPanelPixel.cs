using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPanelPixel : MonoBehaviour
{
	private Material mat;
	private Color original;

	private void Start() {
		mat = GetComponent<Renderer> ().material;
		original = mat.color;
		mat.color = Color.black;
	}
	public void RevealBackPanel() {
		// Debug.Log ("Revelado");
		mat.EnableKeyword ("_EMISSION");
		mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
		mat.SetColor ("_EmissionColor", original);
	}
}
