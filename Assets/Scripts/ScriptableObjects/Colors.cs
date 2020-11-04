using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors : ScriptableObject
{
	public string correctColorString;
	public Color correctColor;

	public void SetCorrectColor() {
		if (!ColorUtility.TryParseHtmlString (correctColorString, out correctColor)) {
			Debug.LogWarning ("Color cannot be converted");
		}
	}

}
