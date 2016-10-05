using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float Speed;

	private Rigidbody rigidbody;

	void Start () 
	{
		rigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () 
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal,0.0f,moveVertical);

		rigidbody.AddForce (movement * Speed);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("PickUp"))
		{
			other.gameObject.SetActive (false);
		}
	}
}
