using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleImage : MonoBehaviour
{
    public Dictionary<string, List<Pixel>> pixelGroups = new Dictionary<string, List<Pixel>>();
    public List<BackPanelPixel> bpp;
    public bool isSolved;
    private HashSet<string> colorsSolved = new HashSet<string>();

	private void Start() {
        CreateGroups ();

    }

	public void PaintAllPixelsOfColor(string colorHex) {
        List<Pixel> pixelGroup = FindGroupOfPixels (colorHex);
        if (colorsSolved.Contains (colorHex))
            return;
        colorsSolved.Add (colorHex);
        foreach (Pixel p in pixelGroup) {
            p.StopBlinking ();
            p.ChangeCorrectColor ();
        }
        if (colorsSolved.Count == pixelGroups.Count-1) {
            PaintAllPixelsOfColor ("#ffffff");
            foreach (BackPanelPixel bp in bpp) {
                bp.RevealBackPanel ();
            }
        }
        isSolved = colorsSolved.Count == pixelGroups.Count;
    }

    private void CreateGroups() {
        foreach (Pixel p in GetComponentsInChildren<Pixel> ()) {
            List<Pixel> tempGroup;
            if (!pixelGroups.TryGetValue (p.setOfColors.correctColorString, out tempGroup)) {
                pixelGroups.Add (p.setOfColors.correctColorString, new List<Pixel> () { p });
            } else {
                tempGroup.Add (p);
            }
        }

        bpp = new List<BackPanelPixel> (GetComponentsInChildren<BackPanelPixel> ());
    }

    private List<Pixel> FindGroupOfPixels(string colorHex) {
        foreach (KeyValuePair<string, List<Pixel>> psc in pixelGroups) {
            if (psc.Key == colorHex) {
                return psc.Value;
            }
        }
        return null;
    }
}
