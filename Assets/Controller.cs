using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	[SerializeField]
	private float moveSpeed = 7;
	
	[SerializeField]
	private float halfSpeed = 3.5f;

	private Rigidbody rigid;

	// Use this for initialization
	void Start () {
		rigid = rigidbody;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		float horizontalSpeed = Input.GetAxis ("Horizontal") * moveSpeed;
		float verticalSpeed = Input.GetAxis ("Vertical") * moveSpeed;
		
		rigid.velocity = new Vector3 (horizontalSpeed, rigid.velocity.y, verticalSpeed);

		if (Input.GetKeyDown (KeyCode.LeftShift)) 
		{
			rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, halfSpeed);
		}
		
	}
}
