using UnityEngine;
using System.Collections;

public class AttachThePlayer : MonoBehaviour {

	private GameController gameController; 

	void Start () 
	{
    GameObject _gameControllerObject = GameObject.FindWithTag ("GameController");
		if (_gameControllerObject != null) 
		{
			gameController = _gameControllerObject.GetComponent<GameController> ();
		}
		if (_gameControllerObject == null) 
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
