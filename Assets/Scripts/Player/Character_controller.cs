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
		private Animator
				anim;
		[SerializeField]
		private GameObject
				start_position;
		private Vector3 velocity = Vector3.zero;
		static public Character_controller characte_controller;


		// Use this for initialization
		void Start ()
		{
			
		}
	
		// Update is called once per frame
		void Update ()
		{
				

				Vector3 input = Vector3.zero;
				if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
						input.z += 1;
				}
				if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
						input.z -= 1;
				}
				if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
						input.x -= 1;
				}
				if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
						input.x += 1;
				}

				
				transform.position = new Vector3 (transform.position.x, 0.3f, transform.position.z);
						
				input = input.normalized;
				velocity = Vector3.MoveTowards (velocity, input * maxSpeed, acceleration * Time.deltaTime);
				controller.Move (velocity * Time.deltaTime);
				if (velocity != Vector3.zero) {
						transform.rotation = Quaternion.LookRotation (-velocity);
						anim.SetBool ("Walk", true);
				} else {
						anim.SetBool ("Walk", false);
				}
				//transform.position = new Vector3 (0, 0, 0);
		}

		public static void resetPosition ()
		{
				Application.LoadLevel (Application.loadedLevel);
		}
}
