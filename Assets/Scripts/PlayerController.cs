using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

  public float Speed;

  private Rigidbody playRigidbody;

  void Start () 
  {
    playRigidbody = GetComponent<Rigidbody> ();
  }

  void LateUpdate () 
  {
    float moveHorizontal = Input.GetAxis ("Horizontal");
    float moveVertical = Input.GetAxis ("Vertical");

    Vector3 movement = new Vector3 (moveHorizontal,0.0f,moveVertical);

    playRigidbody.AddForce (movement.normalized * Speed);
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag ("PickUp"))
    {
      Destroy (other.gameObject);
    }
  }
}
