using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImagePuzzleCreator:MonoBehaviour
{

	private static Dictionary<string, Colors> colorsSO = new Dictionary<string, Colors>();

	[ExecuteInEditMode]
	public static void CreateImagePuzzleContainer() {
		GameObject puzzleImageContainer = Instantiate (Resources.Load ("Puzzle1 Container", typeof (GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
		puzzleImageContainer.name = "Container Template";
	}

	public static void CreatePuzzles(GameObject container, ImagePuzzleData data) {
		Colors setOfColors = ScriptableObject.CreateInstance<Colors> ();
		setOfColors.correctColorString = "#ffffff";
		setOfColors.SetCorrectColor ();
		AssetDatabase.CreateAsset (setOfColors, "Assets/Colors/Color" + setOfColors.correctColorString + ".asset");
		colorsSO.Add (setOfColors.correctColorString, setOfColors);

		int puzzleIndex = 1;
		foreach (ImageData imageData in data.images) {
			foreach (string c in imageData.colors) {
				Colors fakeColor;
				if (!colorsSO.TryGetValue (c, out fakeColor)) {
					Colors setOfColorsForPixel = ScriptableObject.CreateInstance<Colors> ();
					setOfColorsForPixel.correctColorString = c;
					setOfColorsForPixel.SetCorrectColor ();
					AssetDatabase.CreateAsset (setOfColorsForPixel, "Assets/Colors/Color" + c + ".asset");
					colorsSO.Add (c, setOfColorsForPixel);
				}
			}
			AssetDatabase.SaveAssets ();

			GameObject localContainer = new GameObject ("Puzzle "+ puzzleIndex);
			localContainer.AddComponent<PuzzleImage> ();
			localContainer.transform.parent = container.transform;
			localContainer.transform.localPosition = Vector3.left * puzzleIndex * 3;
			CreateIndividualPuzzles (localContainer, imageData, puzzleIndex);
			CreateLetters (localContainer,imageData);
			puzzleIndex++;
		}
	}

	public static void CreateIndividualPuzzles(GameObject localContainer, ImageData imageData, int puzzleIndex) {
		PuzzleImage pi = localContainer.GetComponent<PuzzleImage> ();
		Dictionary<string, List<Pixel>> sameColor = new Dictionary<string, List<Pixel>> ();


		for (int row = 0; row < 12*12; row+=12) {
			float yPosition = (0.6f) - row/(12f*10f);
			for (int cell=0; cell < 12; cell++) {
				GameObject pixelGO = Instantiate (Resources.Load ("Pixel", typeof (GameObject)), localContainer.transform) as GameObject;
				pixelGO.name = "Pixel[" + row / 12 + "][" + cell + "]";
				pixelGO.transform.parent = localContainer.transform;
				float xPosition = (0.6f) - (cell / 10f);
				pixelGO.transform.localPosition = new Vector3 (xPosition, yPosition, 0);

				Pixel pData = pixelGO.GetComponent<Pixel> ();

				int colorIndex = imageData.grid[row + cell];
				Colors temp = null;
				if (colorIndex == 0) {
					colorsSO.TryGetValue ("#ffffff", out temp);
				} else {
					colorsSO.TryGetValue (imageData.colors[colorIndex - 1], out temp);
				}
				pData.setOfColors = temp;
			}
		}
	}


	public static void CreateLetters(GameObject localContainer, ImageData imageData) {
		for (int row = 0; row < 12 * 12; row += 12) {
			float yPosition = (0.6f) - row / (12f * 10f);
			for (int cell = 0; cell < 12; cell++) {
				int colorIndex = imageData.gridLetters[row + cell];
				string prefabToInstantiate = colorIndex == 0 ? "Pixel White" : "Pixel Black";
				GameObject pixelGO = Instantiate (Resources.Load (prefabToInstantiate, typeof (GameObject)), localContainer.transform) as GameObject;
				pixelGO.name = "Pixel[" + row / 12 + "][" + cell + "]";
				pixelGO.transform.parent = localContainer.transform;
				float xPosition = (-0.6f) + (cell / 10f);
				pixelGO.transform.localPosition = new Vector3 (xPosition, yPosition, -1.25f);
				pixelGO.transform.rotation = Quaternion.Euler (Vector3.up * 180);

				Pixel pData = pixelGO.GetComponent<Pixel> ();
			}
		}

	}

}
