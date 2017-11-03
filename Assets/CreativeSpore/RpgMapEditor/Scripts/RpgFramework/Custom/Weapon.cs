using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CreativeSpore.RpgMapEditor{
	public class Weapon : MonoBehaviour {
		
		public Projectile projectile;

		// 연사 속도
		public float msBetweenShots = 100;
		// 탄 속도
		public float bulletVelocity = 35;

		// 탄 계산 타임 변수
		float nextShotTime;

		public void Shoot() {
			Vector3 vBulletDir = Vector3.zero;
			Vector3 vBulletPos = Vector3.zero;
			Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
				//if (equippedGun.type == "gun") <<<< add
			vBulletPos += transform.position;
			vBulletDir = (dir - new Vector3(transform.position.x, transform.position.y,0f)).normalized;
			if (Time.time > nextShotTime){
				nextShotTime = Time.time + msBetweenShots/1000;

				vBulletPos += transform.position;
                vBulletDir = (dir - new Vector3(transform.position.x, transform.position.y,0f)).normalized;

                //float fRand = Random.Range(-1f, 1f);
				//fRand = Mathf.Pow(fRand, 5f);
				//vBulletDir = Quaternion.AngleAxis(BulletAngDispersion*fRand, Vector3.forward) * vBulletDir;
                vBulletDir = new Vector3(vBulletDir.x,vBulletDir.y,-0.5f);

                //CreateBullet( vBulletPos, vBulletDir);

				Projectile newProjectile = Instantiate(projectile, vBulletPos, projectile.transform.rotation) as Projectile;
				//newProjectile.SetSpeed(bulletVelocity);
			}
		}
	}
}

/*
	public static BulletController CreateBullet( GameObject caller, GameObject bulletPrefab, Vector3 vPos, Vector3 vDir, float speed, float dmgQty = 0.5f )
	{
		GameObject bullet = Instantiate(bulletPrefab, vPos, bulletPrefab.transform.rotation) as GameObject;
		
		// set friendly tag, to avoid collision with this layers
		bullet.layer = caller.layer;
		
		BulletController bulletCtrl = bullet.GetComponent<BulletController>();		
		bulletCtrl.Dir = vDir;
		bulletCtrl.Speed = speed;
		bulletCtrl.DamageQty = dmgQty;
		
		return bulletCtrl;
	}

 */