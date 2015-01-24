using UnityEngine;
using System.Collections;

public class SC_shadow : MonoBehaviour {

	static public bool _b_player_is_in_shadow = false;


	private void OnTriggerEnter(Collider collider)
	{
		_b_player_is_in_shadow = true;
	}

	private void OnTriggerExit(Collider collider)
	{
		_b_player_is_in_shadow = false;
	}
}
