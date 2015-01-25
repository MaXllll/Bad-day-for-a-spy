using UnityEngine;
using System.Collections;

public class Fart_manager : MonoBehaviour
{
		[SerializeField]
		private GameObject
				fart_prefab;
		[SerializeField]
		private float
				step_fart_long = 0.35f;
		[SerializeField]
		private LayerMask
				guards_layer;
		[SerializeField]
		private AudioClip
				short_fart_1;
		[SerializeField]
		private AudioClip
				long_fart;
		[SerializeField]
		private AudioClip
				sneaky_fart;
		[SerializeField]
		private LayerMask
				walls_layer;
	
		void Update ()
		{
				/*if (Input.GetKeyDown (KeyCode.Space)) {
						fart (10, 5, 1);
				}
				if (Input.GetKeyDown (KeyCode.C)) {
						fart_long (7, 2, 0);
				}*/
		}

		public void fart (int loud_level, int duration, int volume, bool sneaky)
		{
				if (loud_level != 0) {
						if (sneaky) {
								Sound (loud_level, sneaky_fart);
						} else {
								Sound (loud_level, short_fart_1);
						}
				}
				
				GameObject go = (GameObject)Instantiate (fart_prefab, transform.position, Quaternion.identity);
				SC_fart sc_fart = go.GetComponent<SC_fart> ();
				sc_fart.StartCoroutine (sc_fart.Fart (volume, duration));
		}

		private IEnumerator fart_long_co (int loud_level, int duration, int volume)
		{	
				float time = 0f;
				if (loud_level != 0) {
						Sound (loud_level, long_fart);
				}
				while (time < duration) {
						GameObject go = (GameObject)Instantiate (fart_prefab, transform.position, Quaternion.identity);
						SC_fart sc_fart = go.GetComponent<SC_fart> ();
						sc_fart.StartCoroutine (sc_fart.Fart (volume, duration));

						time += step_fart_long;			
						
						yield return new WaitForSeconds (step_fart_long);
				}
		}

		public void fart_long (int loud_level, int duration, int volume)
		{
				StartCoroutine (fart_long_co (loud_level, duration, volume));
		}

		private void Sound (int loud_level, AudioClip clip)
		{

				AudioSource.PlayClipAtPoint (clip, Vector3.zero);

				Collider[] proximity_guards = Physics.OverlapSphere (transform.position, loud_level, guards_layer);
				for (int i= 0; i < proximity_guards.Length; ++i) {
						//Vector3 V3_collider_direction = proximity_guards [i].transform.position - transform.position;
						//RaycastHit _hit;
						//if (!Physics.Raycast (transform.position, V3_collider_direction, out _hit, loud_level, walls_layer)) {
						proximity_guards [i].GetComponent<SC_ai_behaviour> ().SetCheck (transform.position);
						//}
				}
		}
}