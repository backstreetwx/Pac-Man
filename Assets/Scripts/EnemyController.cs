using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

  private GameController gameController; 

  void Start () 
  {
    gameController = FindObjectOfType<GameController> ();
    if (gameController == null) 
    {
      Debug.Log ("Can not find 'GameController' script");
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag ("Player")) 
    {
      gameController.GameOver ();
      Destroy (other.gameObject);
    }
  }

}
