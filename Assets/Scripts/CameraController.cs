using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

  public GameObject Player;

  private Vector3 offset;


  void Start () 
  {
    offset = transform.position - Player.transform.position;
  }


  void LateUpdate () 
  {
    try
    {
      transform.position = Player.transform.position + offset;  
    }
    catch(MissingReferenceException exception)
    {
      Debug.Log ("Can not find GameObject 'Player'");
    }

  }
}
