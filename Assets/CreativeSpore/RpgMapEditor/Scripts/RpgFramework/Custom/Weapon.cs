using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CreativeSpore.RpgMapEditor{
	public class Weapon : MonoBehaviour {
		
		public Projectile projectile;

		// 연사 속도
		public float msBetweenShots = 100;
		// 탄 속도
		public float bulletVelocity = 4.0f;

		public float damage = 5.0f;
		public float ttl = 5.0f;
		// 탄 계산 타임 변수
		float nextShotTime;
		
		// weapon layer
		int Weaponlayer;
		/*
		0.06	0.08	-0.1
		0.04	0.08	-0.1
		0.02	0.07	-0.1

		-0.1	0.03	-0.1
		-0.09	0.05	-0.1
		-0.07	0.04	-0.1
		*/

		public Vector3[] vOffDirRight = new Vector3[3]  
		{ 
			new Vector3( 0.06f, 0.08f, -0.0001f ),
			new Vector3( 0.04f, 0.08f, -0.0001f ),
			new Vector3( 0.02f, 0.07f, -0.0001f ),
		};

		public Vector3[] vOffDirDown = new Vector3[3]  
		{ 
			new Vector3( -0.1f, 0.03f, -0.0001f ),
			new Vector3( -0.09f, 0.05f, -0.0001f ),
			new Vector3( -0.07f, 0.04f, -0.0001f ),
		};

		//public SpriteRenderer WeaponSprite;
		//public Sprite WeaponSpriteHorizontal;
		//public Sprite WeaponSpriteVertical;

		//private DirectionalAnimation m_charAnimCtrl;

		public void setLayer(int w){
			Weaponlayer = w;
		}
		void Start () 
		{
            //m_charAnimCtrl = GetComponent<DirectionalAnimation>();
		}
		
		/*
		void LateUpdate () 
		{
			WeaponSprite.sprite = WeaponSpriteHorizontal;
			Quaternion qRot = WeaponSprite.transform.localRotation;

			if (m_charAnimCtrl.AnimDirection == eAnimDir.Right)
			{
				qRot.eulerAngles = new Vector3(0f, 0f, 0f);
                WeaponSprite.transform.localPosition = vOffDirRight[(int)m_charAnimCtrl.CurrentFrame % vOffDirRight.Length];
			}
            else if (m_charAnimCtrl.AnimDirection == eAnimDir.Left)
			{
				qRot.eulerAngles = new Vector3(0f, 180f, 0f);
                Vector3 vOff = vOffDirRight[(int)m_charAnimCtrl.CurrentFrame % vOffDirRight.Length];
				vOff.x = -vOff.x;
				vOff.z = -vOff.z;
				WeaponSprite.transform.localPosition = vOff;
			}
            else if (m_charAnimCtrl.AnimDirection == eAnimDir.Down)
			{
				qRot.eulerAngles = new Vector3(0f, 0f, 270f);
				WeaponSprite.sprite = WeaponSpriteVertical;
                WeaponSprite.transform.localPosition = vOffDirDown[(int)m_charAnimCtrl.CurrentFrame % vOffDirDown.Length];
			}
			else // UP
			{
				qRot.eulerAngles = new Vector3(0f, 180f, 90f);
                Vector3 vOff = vOffDirDown[(int)m_charAnimCtrl.CurrentFrame % vOffDirDown.Length];
				vOff.x = -vOff.x;
				vOff.z = -vOff.z;
				vOff.y = vOff.y + 0.08f;
				WeaponSprite.transform.localPosition = vOff;
			}
			
			WeaponSprite.transform.localRotation = qRot;
		}

		*/
		public void Shoot() {
			Vector3 vBulletDir = Vector3.zero;
			Vector3 vBulletPos = Vector3.zero;
			

			//if (equippedGun.type == "gun") <<<< add
			if (Time.time > nextShotTime){
				nextShotTime = Time.time + msBetweenShots/1000;
				
				Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

				vBulletPos += transform.position;
				Debug.Log("b_tr" + transform.position.ToString());
                vBulletDir = (dir - new Vector3(transform.position.x, transform.position.y,0f)).normalized;
				Debug.Log("three"+vBulletDir.ToString());

                vBulletDir = new Vector3(vBulletDir.x,vBulletDir.y,-0.5f);

				Projectile newProjectile = Instantiate(projectile, vBulletPos, projectile.transform.rotation) as Projectile;
				newProjectile.SetProjectile(bulletVelocity, vBulletDir, damage, ttl, Weaponlayer);
				
			}
		}
		

	}
}