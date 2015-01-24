using UnityEngine;
using System.Collections;

public class SC_rotation : MonoBehaviour {

	private Transform _T_gaz;
	private float f_speed;

	void Start()
	{
		_T_gaz = transform;
		f_speed = Random.value * 40 - 20;
		_T_gaz.Rotate(0, Random.value * 360, 0, Space.World);
	}
	
	void Update()
	{
		_T_gaz.Rotate(0, f_speed * Time.deltaTime, 0, Space.World);
	}
}
