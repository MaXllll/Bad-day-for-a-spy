using UnityEngine;
using System.Collections;

public class SC_fart : MonoBehaviour {

	[SerializeField]
	private Transform[] _T_farts_center;
	[SerializeField]
	private Transform[] _T_farts_ring_1;
	[SerializeField]
	private Transform[] _T_farts_ring_2;
	[SerializeField]
	private LayerMask _layer_mask;


	void Start()
	{
		StartCoroutine(Fart(2, 10));
	}
	
	public IEnumerator Fart(int i_size, float f_duration)
	{
		Material Mat_fart;
		Mat_fart = _T_farts_center[0].renderer.material;
		for (int i = 1; i < _T_farts_center.Length; ++i)
		{
			_T_farts_center[i].renderer.sharedMaterial = Mat_fart;
			TestWall(_T_farts_center[i]);
		}
		StartCoroutine (FadeMaterial(Mat_fart, f_duration));

		yield return new WaitForSeconds(f_duration * 0.25f);

		if (i_size > 0)
		{
			Mat_fart = _T_farts_ring_1[0].renderer.material;
			for (int i = 1; i < _T_farts_ring_1.Length; ++i)
			{
				_T_farts_ring_1[i].renderer.sharedMaterial = Mat_fart;
				TestWall(_T_farts_ring_1[i]);
			}
			StartCoroutine (FadeMaterial(Mat_fart, f_duration * 0.75f));
		}

		yield return new WaitForSeconds(f_duration * 0.25f);

		if (i_size > 1)
		{
			Mat_fart = _T_farts_ring_2[0].renderer.material;
			for (int i = 1; i < _T_farts_ring_2.Length; ++i)
			{
				_T_farts_ring_2[i].renderer.sharedMaterial = Mat_fart;
				TestWall(_T_farts_ring_2[i]);
			}
			StartCoroutine (FadeMaterial(Mat_fart, f_duration * 0.5f));
		}

		yield return new WaitForSeconds(f_duration * 0.5f);

		Destroy(gameObject);
	}

	private IEnumerator FadeMaterial(Material Mat_fart, float f_duration)
	{
		Color _color = Mat_fart.color;
		_color.a = 0;
		float f_time = 0;

		while (f_time < f_duration * 0.25f)
		{
			f_time += Time.deltaTime;
			_color.a += Time.deltaTime * 2f / f_duration;
			Mat_fart.color = _color;
			yield return null;
		}

		f_time = f_duration * 0.5f;
		_color.a = 0.5f;
		Mat_fart.color = _color;

		yield return new WaitForSeconds(f_duration * 0.5f);

		while (f_time > 0)
		{
			f_time -= Time.deltaTime;
			_color.a -= Time.deltaTime * 2f / f_duration;
			Mat_fart.color = _color;
			yield return null;
		}

		_color.a = 0;
		Mat_fart.color = _color;
	}

	private void TestWall(Transform T_position)
	{
		if (Physics.Raycast(transform.position, T_position.position - transform.position, Vector3.Distance(T_position.position, transform.position), _layer_mask))
		{
			T_position.gameObject.SetActive(false);
		}
	}
}
