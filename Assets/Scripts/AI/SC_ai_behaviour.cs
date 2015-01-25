using UnityEngine;
using System.Collections;

public class SC_ai_behaviour : MonoBehaviour {

	private Transform _T_ai;
	private NavMeshAgent _nav_mesh_agent;
	[SerializeField]
	private SC_patrol_way _patrol_way;
	[SerializeField]
	private SC_ai_sight _ai_sight;
	[SerializeField]
	private Animator _animator;

	public enum AIState {Wait, Patrol, GoCheck, LookAround, ReturnToPosition, Chase}
	public AIState _current_state {get; private set;}

	[SerializeField]
	private float _f_walk_speed = 2.5f;
	[SerializeField]
	private float _f_run_speed = 6f;

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

	// GoCheck variables
	private float _f_go_check_time_limit = 3;
	private float _f_go_check_current_time = 0;

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
		else
			SetWait();

		_nav_mesh_agent.speed = _f_walk_speed;
	}


	void Update()
	{
		switch (_current_state)
		{
		case AIState.Wait:
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
			_f_go_check_current_time += Time.deltaTime;
			if (_f_go_check_current_time > _f_go_check_time_limit || Vector2.Distance(new Vector2(_T_ai.position.x, _T_ai.position.z), new Vector2(_nav_mesh_agent.destination.x, _nav_mesh_agent.destination.z)) < 1.25f)
				SetLookAround();
			break;

		case AIState.LookAround:
			_f_angle_look_around += Time.deltaTime * 90;
			_T_ai.eulerAngles = _V3_start_angle_look_around + Vector3.up * _f_angle_look_around;
			if (_f_angle_look_around > 360)
				SetReturnToPosition();
			break;

		case AIState.ReturnToPosition:
			if (Vector2.Distance(new Vector2(_T_ai.position.x, _T_ai.position.z), new Vector2(_nav_mesh_agent.destination.x, _nav_mesh_agent.destination.z)) < 0.25f)
				SetWait();
			break;

		case AIState.Chase:
			if (_ai_sight._b_player_is_in_sight)
			{
				_nav_mesh_agent.destination = _ai_sight._V3_last_player_position_in_sight;
				if (Vector2.Distance(new Vector2(_T_ai.position.x, _T_ai.position.z), new Vector2(_nav_mesh_agent.destination.x, _nav_mesh_agent.destination.z)) < 1.25f)
				{
					Debug.Log("PLAYER CAUGHT !!!!");
					Character_controller.resetPosition();
				}
			}
			else
			{
				_current_state = AIState.GoCheck;
				_nav_mesh_agent.destination = _ai_sight._V3_last_player_position_in_sight;
				_f_go_check_current_time = 0;
			}
			break;
		}

		if (_nav_mesh_agent.velocity != Vector3.zero)
			_animator.SetBool ("Walk", true);
		else
			_animator.SetBool ("Walk", false);
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
		_nav_mesh_agent.speed = _f_walk_speed;
	}

	private void NextWayPoint()
	{
		_i_current_way_point = _patrol_way.GetNextIndex(_i_current_way_point);
		_V3_current_way_point = _patrol_way.GetWayPoint(_i_current_way_point);
		_nav_mesh_agent.destination = _V3_current_way_point;
	}

	public void SetCheck(Vector3 V3_position_to_check)
	{
		if (_current_state != AIState.Chase)
		{
			_current_state = AIState.GoCheck;
			_nav_mesh_agent.destination = V3_position_to_check;
			_f_go_check_current_time = 0;
		}
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
			_nav_mesh_agent.speed = _f_walk_speed;
			_current_state = AIState.ReturnToPosition;
			_nav_mesh_agent.destination = _V3_start_position;
		}
		else
		{
			SetPatrol();
		}
	}

	public void SetChase()
	{
		_current_state = AIState.Chase;
		_nav_mesh_agent.speed = _f_run_speed;
	}
}
