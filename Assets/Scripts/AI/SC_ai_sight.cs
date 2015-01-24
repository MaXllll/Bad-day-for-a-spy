using UnityEngine;
using System.Collections;

public class SC_ai_sight : MonoBehaviour {

	[SerializeField]
	private float _f_sight_distance = 10;
	[SerializeField]
	private float _f_sight_angle = 90;

	private Transform _T_trigger;

	public Transform _T_in_sight {set; private get;}


	void Start()
	{
		GetComponent<SphereCollider>().radius = _f_sight_distance;
		_T_trigger = transform;
	}


	void OnTriggerStay(Collider collider)
	{
		Vector3 V3_collider_direction = collider.transform.position - _T_trigger.position;
		if (Vector3.Angle(_T_trigger.forward, V3_collider_direction) < _f_sight_angle * 0.5f)
		{
			RaycastHit _hit;
			if (Physics.Raycast(_T_trigger.position, V3_collider_direction, out _hit, _f_sight_distance))
			{
				if (_hit.collider == collider)
				{
					Debug.Log("Player spotted !!!");
					_T_in_sight = collider.transform;
				}
			}
		}
	}


	void OnTriggerExit(Collider collider)
	{
		_T_in_sight = null;
	}
}
