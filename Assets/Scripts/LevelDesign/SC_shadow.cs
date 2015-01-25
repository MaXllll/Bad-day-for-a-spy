using UnityEngine;
using System.Collections;

public class SC_shadow : MonoBehaviour {

	static public bool _b_player_is_in_shadow = false;
	/*
	[SerializeField]
	public GameObject go_player;
	*/

	private void OnTriggerEnter(Collider collider)
	{
		_b_player_is_in_shadow = true;
		//go_player.GetComponents('GaugeManager');
	}

	private void OnTriggerExit(Collider collider)
	{
		_b_player_is_in_shadow = false;
	}
}
