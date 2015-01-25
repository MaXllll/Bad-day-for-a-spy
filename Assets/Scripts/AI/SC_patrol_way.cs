using UnityEngine;
using System.Collections;

public class SC_patrol_way : MonoBehaviour {

	[SerializeField]
	private Transform[] _T_way_points;
	[SerializeField]
	private bool _b_show_patrol_in_editor = true;
	

	public int GetNextIndex(int i_index)
	{
		++i_index;
		if (i_index >= _T_way_points.Length)
			i_index = 0;

		return i_index;
	}

	public Vector3 GetWayPoint(int i_index)
	{
		return _T_way_points[i_index].position;
	}

	void OnDrawGizmos()
	{
		if (_b_show_patrol_in_editor)
		{
			Gizmos.color = Color.green;
			for(int i = 0; i < _T_way_points.Length; ++i)
			{
				if (_T_way_points[i] != null)
				{
					Gizmos.DrawSphere(_T_way_points[i].position, 0.5f);
					if (_T_way_points[GetNextIndex(i)] != null)
						Gizmos.DrawLine(_T_way_points[i].position, _T_way_points[GetNextIndex(i)].position);
				}
			}
		}
	}
}
