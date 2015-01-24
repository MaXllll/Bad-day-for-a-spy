using UnityEngine;
using System.Collections;

public class SC_tmp_controlle_AI : MonoBehaviour {

	[SerializeField]
	private Camera _camera;
	[SerializeField]
	private SC_ai_behaviour _ai_behaviour;
	[SerializeField]
	private LayerMask _layer;

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit _hit;
			if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit, 100f, _layer))
			{
				_ai_behaviour.SetCheck(_hit.point);
			}
		}
	}
}
