using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SC_trigger_alert_guard : MonoBehaviour {

	[SerializeField]
	private LayerMask _layer_mask;

	private List<Collider> _colliders_alerted_guards = new List<Collider>();

	private Transform _T_trigger;


	void Start()
	{
		_T_trigger = transform;
	}

	void OnTriggerStay(Collider collider)
	{
		if (!_colliders_alerted_guards.Contains(collider))
		{
			if (!Physics.Raycast(_T_trigger.position, collider.transform.position - _T_trigger.position, Vector3.Distance(collider.transform.position, _T_trigger.position), _layer_mask))
			{
				_colliders_alerted_guards.Add(collider);
				collider.GetComponent<SC_ai_behaviour>().SetCheck(transform.position);
			}
		}
	}
}
