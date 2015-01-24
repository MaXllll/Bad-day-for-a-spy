using UnityEngine;
using System.Collections;

public class SC_ai_sight : MonoBehaviour {

	[SerializeField]
	private SC_ai_behaviour _ai_behaviour;
	[SerializeField]
	private float _f_sight_distance = 10;
	[SerializeField]
	private float _f_sight_angle = 90;
	[SerializeField]
	private LayerMask _layer_mask;

	private Transform _T_trigger;

	public bool _b_player_is_in_sight {get; private set;}
	public Vector3 _V3_last_player_position_in_sight {get; private set;}


	void Start()
	{
		GetComponent<SphereCollider>().radius = _f_sight_distance;
		_T_trigger = transform;
		_b_player_is_in_sight = false;
	}


	void OnTriggerStay(Collider collider)
	{
		float f_sight_distance;
		if (SC_shadow._b_player_is_in_shadow)
		{
			if (_ai_behaviour._current_state == SC_ai_behaviour.AIState.Wait || _ai_behaviour._current_state == SC_ai_behaviour.AIState.Patrol || _ai_behaviour._current_state == SC_ai_behaviour.AIState.ReturnToPosition)
				return;

			f_sight_distance = _f_sight_distance * 0.3f;
			Debug.Log(f_sight_distance);
		}
		else 
			f_sight_distance = _f_sight_distance;

		Vector3 V3_collider_direction = collider.transform.position - _T_trigger.position;
		if (Vector3.Angle(_T_trigger.forward, V3_collider_direction) < _f_sight_angle * 0.5f)
		{
			RaycastHit _hit;
			if (Physics.Raycast(_T_trigger.position, V3_collider_direction, out _hit, f_sight_distance, _layer_mask))
			{
				if (_hit.collider == collider)
				{
					if (!_b_player_is_in_sight)
						_ai_behaviour.SetChase();
					_b_player_is_in_sight = true;
					_V3_last_player_position_in_sight = _hit.transform.position;
					return;
				}
			}
		}
		_b_player_is_in_sight = false;
	}


	void OnTriggerExit(Collider collider)
	{
		_b_player_is_in_sight = false;
	}
}
