using UnityEngine;
using System.Collections;

public class SC_ai_behaviour : MonoBehaviour {

	private Transform _T_ai;
	private NavMeshAgent _nav_mesh_agent;
	[SerializeField]
	private SC_patrol_way _patrol_way;

	public enum AIState {Wait, Patrol, GoCheck, LookAround, ReturnToPosition}
	private AIState _current_state = AIState.Wait;

	private Vector3 _V3_start_position;
	private Vector3 _V3_start_euler_angle;

	// Wait variables
	[SerializeField]
	private float _f_wait_rotation_speed = 10;
	[SerializeField]
	private float _f_wait_angle_offset_max = 45;
	private float _f_wait_current_angle_offset = 0;
	private bool _b_wait_rotation_direction = true;

	// Patrol variables
	private int _i_current_way_point = 0;
	private Vector3 _V3_current_way_point;

	// LookAround variables
	private Vector3 _V3_start_angle_look_around;
	private float _f_angle_look_around;


	void Start()
	{
		_T_ai = transform;
		_nav_mesh_agent = GetComponent<NavMeshAgent>();

		_V3_start_position = _T_ai.position;
		_V3_start_euler_angle = _T_ai.eulerAngles;

		if (_patrol_way != null)
		{
			_V3_current_way_point = _patrol_way.GetWayPoint(_i_current_way_point);
			SetPatrol();
		}
	}


	void Update()
	{
		//Debug.Log(_current_state);

		switch (_current_state)
		{
		case AIState.Wait:
			// Tourne Legerment
			if (_b_wait_rotation_direction)
			{
				_f_wait_current_angle_offset += _f_wait_rotation_speed * Time.deltaTime;
				if (_f_wait_current_angle_offset > _f_wait_angle_offset_max * 0.5f)
				{
					_f_wait_current_angle_offset = _f_wait_angle_offset_max * 0.5f;
					_b_wait_rotation_direction = false;
				}

			}
			else
			{
				_f_wait_current_angle_offset -= _f_wait_rotation_speed * Time.deltaTime;
				if (_f_wait_current_angle_offset < -_f_wait_angle_offset_max * 0.5f)
				{
					_f_wait_current_angle_offset = -_f_wait_angle_offset_max * 0.5f;
					_b_wait_rotation_direction = true;
				}
			}
			_T_ai.eulerAngles = _V3_start_euler_angle + Vector3.up * _f_wait_current_angle_offset;
			break;

		case AIState.Patrol:
			if (Vector2.Distance(new Vector2(_T_ai.position.x, _T_ai.position.z), new Vector2(_nav_mesh_agent.destination.x, _nav_mesh_agent.destination.z)) < 0.25f)
				NextWayPoint();
			break;

		case AIState.GoCheck:
			if (Vector2.Distance(new Vector2(_T_ai.position.x, _T_ai.position.z), new Vector2(_nav_mesh_agent.destination.x, _nav_mesh_agent.destination.z)) < 0.25f)
				SetLookAround();
			break;

		case AIState.LookAround:
			_f_angle_look_around += Time.deltaTime * 90;
			_T_ai.eulerAngles = _V3_start_euler_angle + Vector3.up * _f_angle_look_around;
			if (_f_angle_look_around > 360)
				SetReturnToPosition();
			break;

		case AIState.ReturnToPosition:
			if (Vector2.Distance(new Vector2(_T_ai.position.x, _T_ai.position.z), new Vector2(_nav_mesh_agent.destination.x, _nav_mesh_agent.destination.z)) < 0.25f)
				SetWait();
			break;
		}
	}


	private void SetWait()
	{
		_current_state = AIState.Wait;
		_nav_mesh_agent.destination = _T_ai.position;
	}

	private void SetPatrol()
	{
		_current_state = AIState.Patrol;
		_nav_mesh_agent.destination = _V3_current_way_point;
	}

	private void NextWayPoint()
	{
		_i_current_way_point = _patrol_way.GetNextIndex(_i_current_way_point);
		_V3_current_way_point = _patrol_way.GetWayPoint(_i_current_way_point);
		_nav_mesh_agent.destination = _V3_current_way_point;
	}

	public void SetCheck(Vector3 V3_position_to_check)
	{
		_current_state = AIState.GoCheck;
		_nav_mesh_agent.destination = V3_position_to_check;
	}

	private void SetLookAround()
	{
		_current_state = AIState.LookAround;
		_nav_mesh_agent.destination = _T_ai.position;
		_V3_start_angle_look_around = _T_ai.eulerAngles;
		_f_angle_look_around = 0f;
	}

	private void SetReturnToPosition()
	{
		if (_patrol_way == null)
		{
			_current_state = AIState.ReturnToPosition;
			_nav_mesh_agent.destination = _V3_start_position;
		}
		else
		{
			SetPatrol();
		}
	}
}
