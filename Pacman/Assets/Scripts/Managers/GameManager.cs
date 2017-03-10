using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //All the game variable
    //Number of pacman lives
    public static int lives = 3;

    public enum GameState { Init, Game, Dead, Scores }
    public static GameState gameState;

    //First level 0
    public static int Level = 0;

    //Pacman game object
    private GameObject pacman;

    //Clyde Ghost game object
    private GameObject clyde;

    //Pinky game object
    private GameObject pinky;

    //Blinky game object
    private GameObject blinky;

    //Inky gameobject
    private GameObject inky;

    private GameNavigation gui;

    public static bool scared;
    static public int score;

    public float scareLength;
    private float timeToCalm;

    //represents the speed of ghosts depending on which level the player is in
    public float SpeedPerLevel;



    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }



    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }

        AssignGhosts();
    }

    void Start()
    {
        gameState = GameState.Init;
    }

    //On the level load
    void OnLevelWasLoaded()
    {
        //The very first level we start with 3 lives
        if (Level == 0)
        {
            lives = 3;
        }

        Debug.Log("Level " + Level + " Loaded!");
        //Assign ghosts into the level - calling the method
        AssignGhosts();
        //calling method to set the scared to false and kill streak to 0
        ResetVariables();


        // Here we are assigning the speed each ghost will have
        //Pacman speed
        pacman.GetComponent<PlayerController>().pacman_speed += Level * SpeedPerLevel / 2;
        //Clyde
        clyde.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
        //Blinky will be the fastest ghost and first out of the maze
        blinky.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
        //Inky
        inky.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;
        //Pinky
        pinky.GetComponent<GhostMove>().speed += Level * SpeedPerLevel;


    }

    private void ResetVariables()
    {
        timeToCalm = 0.0f;
        scared = false;
        PlayerController.killstreak = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (scared && timeToCalm <= Time.time)
        {
            CalmGhosts();
        }

    }

    public void ResetScene()
    {
        CalmGhosts();

        pacman.transform.position = new Vector3(15f, 11f, 0f);
        blinky.transform.position = new Vector3(15f, 20f, 0f);
        pinky.transform.position = new Vector3(14.5f, 17f, 0f);
        inky.transform.position = new Vector3(16.5f, 17f, 0f);
        clyde.transform.position = new Vector3(12.5f, 17f, 0f);

        pacman.GetComponent<PlayerController>().ResetDestination();
        blinky.GetComponent<GhostMove>().InitializeGhost();
        pinky.GetComponent<GhostMove>().InitializeGhost();
        inky.GetComponent<GhostMove>().InitializeGhost();
        clyde.GetComponent<GhostMove>().InitializeGhost();

        gameState = GameState.Init;
        gui.ReadyScreenShow();

    }

    public void ToggleScare()
    {
        if (!scared) ScareGhosts();
        else CalmGhosts();
    }

    public void ScareGhosts()
    {
        scared = true;
        //Set the ghosts to scared mode
        inky.GetComponent<GhostMove>().Frighten();
        clyde.GetComponent<GhostMove>().Frighten();
        blinky.GetComponent<GhostMove>().Frighten();
        pinky.GetComponent<GhostMove>().Frighten();

        timeToCalm = Time.time + scareLength;

        Debug.Log("Ghosts Scared");
    }

    public void CalmGhosts()
    {
        scared = false;
        blinky.GetComponent<GhostMove>().Calm();
        pinky.GetComponent<GhostMove>().Calm();
        inky.GetComponent<GhostMove>().Calm();
        clyde.GetComponent<GhostMove>().Calm();
        PlayerController.killstreak = 0;
    }


    public void PacmanLoseLife()
    {
        lives--;
        gameState = GameState.Dead;

        // update UI too
        GUIScript ui = GameObject.FindObjectOfType<GUIScript>();
        Destroy(ui.lives[ui.lives.Count - 1]);
        ui.lives.RemoveAt(ui.lives.Count - 1);
    }

    void AssignGhosts()
    {
        // Assign ghosts
        clyde = GameObject.Find("clyde");
        pinky = GameObject.Find("pinky");
        inky = GameObject.Find("inky");
        blinky = GameObject.Find("blinky");
        pacman = GameObject.Find("pacman");

      

        gui = GameObject.FindObjectOfType<GameNavigation>();



    }


    public static void DestroyGame()
    {

        lives = 3;
        score = 0;
        Level = 0;
        
        Destroy(GameObject.Find("Game Manager"));
    }
}
