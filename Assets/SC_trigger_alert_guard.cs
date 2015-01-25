using UnityEngine;
using System.Collections;

public class SC_trigger_alert_guard : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		collider.GetComponent<SC_ai_behaviour>().SetCheck(transform.position);
	}
}
