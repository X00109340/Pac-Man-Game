using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public string txtFile = "HighScores";
    string txtContents;

    void Start()
    {
        TextAsset txtAssets = (TextAsset)Resources.Load(txtFile);
        txtContents = txtAssets.text;
        Debug.Log("READING.....");
    }

    void OnGui()
    {
        GUILayout.Label(txtContents);
    }
}
