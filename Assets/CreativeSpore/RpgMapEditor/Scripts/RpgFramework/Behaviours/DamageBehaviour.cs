using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/Behaviours/DamageBehaviour", 10)]
	public class DamageBehaviour : MonoBehaviour 
	{
		public float Health = 1f;
		public GameObject FXDeathPrefab;
		public GameObject RandomDrop;
		public float GodModeTimer = 0.3f;

		private SpriteRenderer m_sprRender;
		private MovingBehaviour m_movingBehaviour;

		private float m_timerGodMode;
		private DamageData m_lastDamageData;

		void Start () 
		{
			m_sprRender = GetComponentInChildren<SpriteRenderer>();
			m_movingBehaviour = GetComponent<MovingBehaviour>();

			GameObject.Find("Health_C_Count").GetComponent<Text>().text = ((int)Health).ToString();
		}
		
		void Update () 
		{
			if( m_timerGodMode > 0 )
			{
				m_timerGodMode -= Time.deltaTime;
				m_sprRender.color = Time.frameCount % 2 == 0? Color.red : Color.white;

				if( m_movingBehaviour != null )
				{
					m_movingBehaviour.ApplyForce( m_lastDamageData.Force * m_movingBehaviour.MaxForce );
				}
			}
			else
			{
				m_sprRender.color = Color.white;
			}
		}

		public void ApplyDamage( DamageData _damageData )
		{
			
			if( m_timerGodMode <= 0 )
			{
				m_timerGodMode = GodModeTimer;
				m_lastDamageData = _damageData;

				//AudioSource.PlayClipAtPoint(SoundLibController.GetInstance().GetSound("hurtEnemy_00"), transform.position); 
							
				Health-=_damageData.Quantity;
				Debug.Log("Health" + Health);
				//GetComponent<PlayerController> ().healthUi.text = Health.ToString();
				GameObject.Find("Health_C_Count").GetComponent<Text>().text = ((int)Health).ToString();
				if( Health <= 0 )
				{
					Destroy( gameObject );
					if( FXDeathPrefab )
					{
						GameObject fxDeath = Instantiate( FXDeathPrefab, transform.position, FXDeathPrefab.transform.rotation ) as GameObject;
						Destroy( fxDeath, 3f);
					}
					if( RandomDrop )
					{
						Instantiate( RandomDrop, transform.position, transform.rotation );
					}
				}
			}
		}
	}
}
