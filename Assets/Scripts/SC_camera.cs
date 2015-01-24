using UnityEngine;
using System.Collections;

public class SC_camera : MonoBehaviour {

	[SerializeField]
	private float distance_from_transform = 25;

	[SerializeField]
	private Transform _T_player;
	private Transform _T_camera;


	void Start()
	{
		_T_camera = transform;
	}
	
	void Update()
	{
		Vector3 V3_camera_position = _T_player.position;
		V3_camera_position.y = distance_from_transform;
		_T_camera.position = V3_camera_position;
	}
}
