﻿using UnityEngine;
using System.Collections;

public class Fart_manager : MonoBehaviour
{
		[SerializeField]
		private GameObject
				fart_prefab;
		[SerializeField]
		private float
				step_fart_long = 0.50f;
		[SerializeField]
		private LayerMask
				guards_layer;
		[SerializeField]
		private AudioClip
				short_fart;
		[SerializeField]
		private AudioClip
				long_fart;
	
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Space)) {
						fart (3, 5, 1);
				}
				if (Input.GetKeyDown (KeyCode.C)) {
						StartCoroutine (fart_long (5, 2, 0));
				}
		}

		public void fart (int loud_level, int duration, int volume)
		{
				Sound (loud_level, short_fart);
				GameObject go = (GameObject)Instantiate (fart_prefab, transform.position, Quaternion.identity);
				SC_fart sc_fart = go.GetComponent<SC_fart> ();
				sc_fart.StartCoroutine (sc_fart.Fart (volume, duration));
		}

		public IEnumerator fart_long (int loud_level, int duration, int volume)
		{	
				float time = 0f;
				Sound (loud_level, long_fart);
				while (time < duration) {
						GameObject go = (GameObject)Instantiate (fart_prefab, transform.position, Quaternion.identity);
						SC_fart sc_fart = go.GetComponent<SC_fart> ();
						sc_fart.StartCoroutine (sc_fart.Fart (volume, duration));

						time += step_fart_long;			
						
						yield return new WaitForSeconds (step_fart_long);
				}
		}

		private void Sound (int loud_level, AudioClip clip)
		{

				AudioSource.PlayClipAtPoint (clip, Vector3.zero);
				
				Collider[] proximity_guards = Physics.OverlapSphere (transform.position, loud_level, guards_layer);

				for (int i= 0; i < proximity_guards.Length; ++i) {
						proximity_guards [i].GetComponent<SC_ai_behaviour> ().SetCheck (transform.position);
						
				}
		}
}