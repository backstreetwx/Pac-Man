using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

  public GUIText GameOverText;
  public GUIText RestartText;

  private bool gameOver;
  private bool restart;

  void Start () 
  {
    gameOver = false;
    restart = false;
    GameOverText.text = "";
    RestartText.text = "";
  }


  void Update () 
  {
    if (restart) 
    {
      if (Input.GetKeyDown (KeyCode.R)) 
      {
        Application.LoadLevel (Application.loadedLevel);
      }
    }
    if (gameOver) 
    {
      RestartText.text = "Press 'R' for Restart";  
      restart = true;
    }
  }

  public void GameOver()
  {
    GameOverText.text = "Game Over!";
    gameOver = true;
  }
}
