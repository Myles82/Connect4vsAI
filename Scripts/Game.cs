using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject player1Chip;
    [SerializeField] GameObject player2Chip;
    spawningScript dropper;
    AI ai;
    Grid grid;
    SpriteRenderer sp;
    Light2D indicator;
    bool touching = false;
    bool active = false;
    int actions = 1;
    bool canBeUndone = false;
    [SerializeField] GameObject playAgainText;
    [SerializeField] GameObject playAgainPanal;

    [SerializeField] Light2D topLight;
    enum state { play,gameOver}
    state gameState = state.play;
    // Start is called before the first frame update
    void Start()
    {
        indicator = GetComponentInChildren<Light2D>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        dropper = GameObject.Find("Dropper").GetComponent<spawningScript>();
        ai = GameObject.Find("AIDropper").GetComponent<AI>();
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine(setFirstPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameState == state.play) 
        {
            
            
          
            if (actions > 0)
            {

                if (Input.GetKeyDown("1"))
                {
                    play(1);
                }
                else if (Input.GetKeyDown("2"))
                {
                    play(2);
                }
                else if (Input.GetKeyDown("3"))
                {
                    play(3);
                }
                else if (Input.GetKeyDown("4"))
                {
                    play(4);
                }
                else if (Input.GetKeyDown("5"))
                {
                    play(5);
                }
                else if (Input.GetKeyDown("6"))
                {
                    play(6);
                }
                else if (Input.GetKeyDown("7"))
                {
                    play(7);
                }

            }

            if (actions == 0 && canBeUndone)
            {
                if (Input.GetKeyDown("u"))
                {
                    StartCoroutine(Undo());
                }
            }




           



            if ((Input.GetMouseButtonDown(0) && touching) || Input.GetKeyDown(KeyCode.Return))
            {
                if (actions == 0 && dropper.isSettled() && canBeUndone)
                {
                    activate();
                }
            }
        }
        if (Input.GetKeyDown("r"))
        {
            Reset();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        touching = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touching = false;
    }

    private void play(int col)
    {
        if (grid.isRoomInColumn(col) && dropper.isSettled())
        {
            dropper.dropAt(player1Chip, col);
            actions = 0;
            StartCoroutine(postPlay(1));
        }
        else if (dropper.isSettled())
        {
            Debug.Log("no room in column");
        }
        
    }

    private void activate()
    {
      
       
            topLight.color = Color.red;
            setIndecator(false);


            ai.play(player2Chip);
            actions = 1;
            canBeUndone = false;
            StartCoroutine(postPlay(2));
        
        
    }

    IEnumerator postPlay(int player)
    {
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(dropper.isSettled);

        int winner = winCheck();
        if(winner!=0)
        {
            win(winner);
        }
        else if (tieCheck())
        {
            tie();
        }
        else if (player == 1)
        {
            setIndecator(true);
            canBeUndone = true;
        }
        else if (player == 2)
        {
            topLight.color = new Color32(0, 104, 255, 255);
        }
        Debug.Log("update");
    }

    private int winCheck()
    {
        return ai.CheckWinner(grid.updateGrid());
    }

    private void win(int winner)
    {
        gameState = state.gameOver;
        topLight.color = Color.green;
        StartCoroutine(displayEndScreen());
        Debug.Log("Player " + winner + " Wins");
    }

    IEnumerator displayEndScreen()
    {
        yield return new WaitForSeconds(3);
        playAgainPanal.GetComponent<Animator>().SetTrigger("ShowPlayAgain");
        playAgainText.GetComponent<Animator>().SetTrigger("ShowPlayAgain");
    }

    private bool tieCheck()
    {
        if (grid.isRoomInColumn(1)||
            grid.isRoomInColumn(2)||
            grid.isRoomInColumn(3)||
            grid.isRoomInColumn(4)||
            grid.isRoomInColumn(5)||
            grid.isRoomInColumn(6)||
            grid.isRoomInColumn(7))
        {
            return false;
        }
        return true;
    }

    private void tie()
    {
        gameState = state.gameOver;
        topLight.color = Color.yellow;
        Debug.Log("Game ended in a tie");
    }
    
    IEnumerator setFirstPlayer()
    {
        setIndecator(false);
        int randomInt = Random.Range(1, 3);
        if (randomInt == 1)
        {
            actions = 1;
            topLight.color = new Color32(0, 104, 255, 255);
        }
        if (randomInt == 2)
        {
            topLight.color = Color.red;
            actions = 0;
            yield return new WaitForSeconds(1);
            activate(); 
        }
    }

    IEnumerator Undo()
    {
        
        canBeUndone = false;
        setIndecator(false );
        dropper.lastChip.GetComponent<Rigidbody2D>().gravityScale = -10;
        yield return new WaitForSeconds(0.6f);
        grid.updateGrid();
        Destroy(dropper.lastChip);
        actions = 1;

    }

    private void setIndecator(bool on)
    {
        if (on)
        {
            sp.color = Color.green;
            indicator.gameObject.SetActive(true);
        }
        else
        {
            sp.color = new Color32(164, 164, 164, 255);
            indicator.gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
