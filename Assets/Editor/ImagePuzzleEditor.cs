using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImagePuzzleEditor : EditorWindow {
	public TextAsset imagePuzzlesDataFile;
	public GameObject generalContainer;


	[MenuItem ("Window/Image Puzzle Creator")]
	public static void ShowWindow() {
		GetWindow<ImagePuzzleEditor> ("Image Puzzle Creator");
	}

	void OnGUI() {
		GUILayout.Label ("Image Puzzle Creator", EditorStyles.largeLabel);

		GUILayout.Label ("Prefabs Creators", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Create General container")) {
			ImagePuzzleCreator.CreateImagePuzzleContainer ();
		}

		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);

		GUILayout.Label ("Assing Data", EditorStyles.boldLabel);
		imagePuzzlesDataFile = (TextAsset)EditorGUILayout.ObjectField ("File Data Asset", imagePuzzlesDataFile, typeof (TextAsset), true);
		generalContainer = (GameObject)EditorGUILayout.ObjectField ("Container Game Object", generalContainer, typeof (GameObject), true);


		EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);

		GUILayout.Label ("Create & Populate Main Prefab", EditorStyles.boldLabel);
	
		if (GUILayout.Button ("Create Puzzles")) {
			ImagePuzzleData data = JsonUtility.FromJson<ImagePuzzleData> (imagePuzzlesDataFile.text);
			ImagePuzzleCreator.CreatePuzzles (generalContainer, data);
		}

		EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
	}
}
