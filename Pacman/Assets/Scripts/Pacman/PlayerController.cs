using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public string user;
    //Initialise the speed of pacman
    public float pacman_speed = 0.4f;
    //Current destination of pacman
    Vector2 destination = Vector2.zero;
    //The direction pacman is going
    Vector2 direction = Vector2.zero;
    //Next direction of pacman will be based on user input from keyboard
    Vector2 directionNext = Vector2.zero;

    //Boolean variable playing dead is used to set tru when the pacman has been caught by enimies
    private bool animateDead = false;

    //Create an array of point game objects from the sprites
    [Serializable]
    public class PointSprites
    {
        public GameObject[] pointSprites;
    }

    public PointSprites points;


    // script handles
    private GameNavigation GUINav;
    private GameManager GM;

    

    // Use this for initialization
    void Start()
    {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        GUINav = GameObject.Find("UI Manager").GetComponent<GameNavigation>();
        destination = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (GameManager.gameState)
        {
            case GameManager.GameState.Game:
                ReadInputAndMove();
                Animate();
                break;

            case GameManager.GameState.Dead:
                if (!animateDead)
                    StartCoroutine("PlayDeadAnimation");
                break;
        }


    }

    IEnumerator PlayDeadAnimation()
    {
        animateDead = true;
        GetComponent<Animator>().SetBool("Die", true);
        //PLays the animation for 1 sec
        yield return new WaitForSeconds(1);
        //Pacman back to life
        GetComponent<Animator>().SetBool("Die", false);
        //back to another life
        animateDead = false;

        if (GameManager.lives <= 0)
        {
            int highSCoreFromRegistry = (int)PlayerPrefs.GetFloat("Highscore");
            if (GameManager.score >= highSCoreFromRegistry)
            {
                PlayerPrefs.SetFloat("Highscore", GameManager.score);
            }

            

            Debug.Log("High Score: " + GameManager.score);
            
                GUINav.H_ShowGameOverScreen();
        }

        else
            GM.ResetScene();
    }

    //Animate method is used so that we are able to animate the pacman movement 
    void Animate()
    {
        Vector2 dir = destination - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool Valid(Vector2 direction)
    {
        // cast line from 'next to pacman' to pacman
        // not from directly the center of next tile but just a little further from center of next tile
        Vector2 pos = transform.position;
        direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
        RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
        return hit.collider.name == "pacdot" || (hit.collider == GetComponent<Collider2D>());
    }

    public void ResetDestination()
    {
        destination = new Vector2(15f, 11f);
        GetComponent<Animator>().SetFloat("DirX", 1);
        GetComponent<Animator>().SetFloat("DirY", 0);
    }

    void ReadInputAndMove()
    {
        
        Vector2 p = Vector2.MoveTowards(transform.position, destination, pacman_speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        
        if (Input.GetAxis("Horizontal") > 0) directionNext = Vector2.right;
        if (Input.GetAxis("Horizontal") < 0) directionNext = -Vector2.right;
        if (Input.GetAxis("Vertical") > 0) directionNext = Vector2.up;
        if (Input.GetAxis("Vertical") < 0) directionNext = -Vector2.up;

        // if pacman is in the center of a tile
        if (Vector2.Distance(destination, transform.position) < 0.00001f)
        {
            if (Valid(directionNext))
            {
                destination = (Vector2)transform.position + directionNext;
                direction = directionNext;
            }
            else   // if next direction is not valid
            {
                if (Valid(direction))  // and the prev. direction is valid
                    destination = (Vector2)transform.position + direction;   // continue on that direction

                // otherwise, do nothing
            }
        }
    }

    public Vector2 getDir()
    {
        return direction;
    }

    public static int killstreak = 0;


    public void UpdateScore()
    {
        killstreak++;

        // limit killstreak at 4
        if (killstreak > 4) killstreak = 4;

        Instantiate(points.pointSprites[killstreak - 1], transform.position, Quaternion.identity);
        GameManager.score += (int)Mathf.Pow(2, killstreak) * 100;

    }
}
