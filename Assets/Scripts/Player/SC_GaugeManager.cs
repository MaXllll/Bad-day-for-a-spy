using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SC_GaugeManager : MonoBehaviour
{

		[SerializeField]
		public Image
				img_stressBar;
		[SerializeField]
		public Image
				img_fartBar;
		[SerializeField]
		public GameObject
				player;
		private float f_stressValue = 10;
		private float f_fartProbability;
		private int i_fartLevel;
		private bool b_isInShadow = true;

		[SerializeField]
		//private float f_inc_stress = 0.3f or 0.2f;
		private float f_inc_stress;
		private float f_dec_stress = 0.6f;
		[SerializeField]
		//private float f_inc_fart = 0.06f or 0.03f;
		private float f_inc_fart;

		private int i_max = 100;
		private int i_min = 10;


		// Use this for initialization
		void Start ()
		{
				InvokeRepeating ("CalculateFart", 1, 2.2f);
		}
	
		// Update is called once per frame
		void Update ()
		{
				ChangeStressValue ();
				ChangeFartValue ();
		}

		public void SetIsInShadow (bool isInShadowP)
		{
				b_isInShadow = isInShadowP;
		}

		public void ChangeStressValue ()
		{
				if (!b_isInShadow && f_stressValue < i_max) {
						f_stressValue += f_inc_stress;			
						if (f_stressValue > i_max)
								f_stressValue = i_max;
						img_stressBar.rectTransform.sizeDelta = new Vector2 (img_stressBar.rectTransform.rect.width, f_stressValue);
				} else if (b_isInShadow && f_stressValue > i_min) {
						f_stressValue -= f_dec_stress;
						if (f_stressValue < i_min)
								f_stressValue = i_min;
						img_stressBar.rectTransform.sizeDelta = new Vector2 (img_stressBar.rectTransform.rect.width, f_stressValue);
				}		
				//Debug.Log (f_stressValue);
		}

		public void ChangeFartValue ()
		{
				if (!b_isInShadow && f_fartProbability < i_max) {
						f_fartProbability += f_inc_fart;
						f_fartProbability += f_stressValue / 500;
						if (f_fartProbability > i_max)
								f_fartProbability = i_max;
						img_fartBar.rectTransform.sizeDelta = new Vector2 (img_fartBar.rectTransform.rect.width, f_fartProbability);
				} else {
						f_fartProbability += f_inc_fart;
						if (f_fartProbability > i_max)
							f_fartProbability = i_max;
						img_fartBar.rectTransform.sizeDelta = new Vector2 (img_fartBar.rectTransform.rect.width, f_fartProbability);
				}
				/*else if(b_isInShadow && f_fartProbability > 0){
			f_fartProbability -= f_dec_stress;
			if(f_fartProbability < 0) f_fartProbability = 0;
			img_fartBar.rectTransform.sizeDelta = new Vector2(img_fartBar.rectTransform.rect.width,f_fartProbability);
		}*/		
		}

		// duration in secondes, 0/1/2 for range, loud_level +- 10
		// for long fart => 7/2/0
		public void CalculateFart ()
		{
				float dice_roll = Random.value * 100;
				if (f_fartProbability > 35 && dice_roll < f_fartProbability) {
						if (f_fartProbability < 50) {
								player.GetComponent<Fart_manager> ().fart (0, 3, 0);
						} else if (f_fartProbability < 75) {
								player.GetComponent<Fart_manager> ().fart (10, 3, 1);
						} else if (f_fartProbability <= 100) {
								if (dice_roll > 60) {
										player.GetComponent<Fart_manager> ().fart_long (7, 3, 0);					
								} else {
										player.GetComponent<Fart_manager> ().fart (10, 6, 2);
								}
						}
						f_fartProbability = 0;
						img_fartBar.rectTransform.sizeDelta = new Vector2 (img_fartBar.rectTransform.rect.width, f_fartProbability);
				}
		}
}
