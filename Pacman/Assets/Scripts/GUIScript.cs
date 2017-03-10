using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIScript : MonoBehaviour {


    //This creates a list of images. In our case it is a list of Pacman lives images
    public List<Image> lives = new List<Image>(3);

    //Text Variable for score
    public Text score_text;
    //Text Variable for high
    public Text highscore_text;
    //Text Variable for level
    public Text level_text;

    //score variable to hold current score of player
    public int score;
    //high variable to hold highscore of player
    public int high;

    // Use this for initialization
    void Start()
    {
        score_text = GetComponentsInChildren<Text>()[1];
        highscore_text = GetComponentsInChildren<Text>()[0];
        level_text = GetComponentsInChildren<Text>()[2];

        //3 Lives for Pacman
        for (int i = 0; i < 3 - GameManager.lives; i++)
        {
            //This is used to remove the image from the list of images
            Destroy(lives[lives.Count - 1]);
            //This is used to remove the image from the list of images
            lives.RemoveAt(lives.Count - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // update score text
        score = GameManager.score;
        //Update score_text
        score_text.text = "Score\n" + score;
        //Update level_text
        level_text.text = "Level\n" + (GameManager.Level + 1);
        //Update highscore_text
        highscore_text.text = "High Score\n" + (int)PlayerPrefs.GetFloat("Highscore");

    }


}
