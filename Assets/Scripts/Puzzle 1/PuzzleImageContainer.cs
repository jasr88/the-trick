using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImagePuzzleData {
    public ImageData[] images;
}

[Serializable]
public class ImageData {
    public string name;
    public string[] colors;
    public int[] grid;
    public int[] gridLetters;
}




public class PuzzleImageContainer : MonoBehaviour
{
    public TextAsset puzzleData;

    public PuzzleImage[] puzzlesInSection;

}
