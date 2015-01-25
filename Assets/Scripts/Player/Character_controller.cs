using UnityEngine;
using System.Collections;

public class Character_controller : MonoBehaviour
{
		[SerializeField]
		private CharacterController
				controller;
		[SerializeField]
		private float
				maxSpeed;
		[SerializeField]
		private float
				acceleration;
		[SerializeField]
		private Transform
				character_transform;
		[SerializeField]
		private Animator
				anim;
		private Vector3 velocity = Vector3.zero;
	


		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				

				Vector3 input = Vector3.zero;
				if (Input.GetKey (KeyCode.Z)) {
						input.z += 1;
				}
				if (Input.GetKey (KeyCode.S)) {
						input.z -= 1;
				}
				if (Input.GetKey (KeyCode.Q)) {
						input.x -= 1;
				}
				if (Input.GetKey (KeyCode.D)) {
						input.x += 1;
				}

				
			transform.position = new Vector3(transform.position.x, 0.3f,transform.position.z);
						
				input = input.normalized;
				velocity = Vector3.MoveTowards (velocity, input * maxSpeed, acceleration * Time.deltaTime);
				controller.Move (velocity * Time.deltaTime);
				if (velocity != Vector3.zero) {
						transform.rotation = Quaternion.LookRotation (-velocity);
						anim.SetBool ("Walk", true);
				} else {
						anim.SetBool ("Walk", false);
				}
		}
}
