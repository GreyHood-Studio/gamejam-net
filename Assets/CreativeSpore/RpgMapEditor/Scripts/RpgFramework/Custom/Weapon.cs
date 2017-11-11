﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CreativeSpore.RpgMapEditor{
	public class Weapon : Photon.MonoBehaviour {
		
		//Network Related
		private PhotonView PhotonView;
		
		// 무기 타입 0 = basic, 1 = Short, 2 = Medium, 3 = Long
		// 최대 탄 용량 infinite, 60, 64, 60
		// 탄창 크기 10, 10, 8, 5
		// 탄속 3, 1.5, 3, 5
		// 사정거리 5, 3, 7, 10(막탄 5)
		// 연사력 10s, 10s, 15s, 30s
		// 산탄도 0, 45, 15, 0 == (n/5 + 1) 한번에 나가는 총알의 개수
		// 재장전 시간 1.2, 1.2, 1.2, 1.2
		// 화력 1, 1, 1, 1
		public int weaponType = 0;
		
		public Sprite[] gunImage = new Sprite[2];
		public int maxMagazineCount, remainBullet, remainMagazine;
		float reloadTime, msBetweenShots, bulletVelocity, damage, ttl;

		float bulletArea;
		int maxBulletCount;

		// 총알 타입
		public Projectile[] projectileType = new Projectile[4];
		
		// 탄 계산 타임 변수
		float nextShotTime;
		// 재장전 체크
		bool reloading;
		// weapon layer
		int Weaponlayer;

		//private DirectionalAnimation m_charAnimCtrl;

		public void setDirection(int w_dir) {
			if (w_dir == 0)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = gunImage[0];
			} else {
				gameObject.GetComponent<SpriteRenderer>().sprite = gunImage[1];
			}
		}
		public void setLayer(int w){
			Weaponlayer = w;
		}

		void Awake()
		{
			PhotonView = GetComponent<PhotonView> ();
			setWeaponType(weaponType);
		}

		public void RefreshBulletCount() {
			GameObject.Find("Bullet_C_Count").GetComponent<Text>().text = ((int)remainMagazine).ToString();
			//GameObject.Find("Bullet_W_Count").GetComponent<Text>().text = ((int)remainBullet).ToString();
			GameObject.Find("Bullet_Max_Count").GetComponent<Text>().text = ((int)maxBulletCount).ToString();
		}
		void Start () 
		{
			reloading = false;
			remainBullet = maxBulletCount;

			GameObject.Find("Bullet_C_Count").GetComponent<Text>().text = ((int)remainMagazine).ToString();
			//GameObject.Find("Bullet_W_Count").GetComponent<Text>().text = ((int)remainBullet).ToString();
			GameObject.Find("Bullet_Max_Count").GetComponent<Text>().text = ((int)maxBulletCount).ToString();
		}

		public void setWeaponType(int type) {
			if (weaponType == 0) {
				maxBulletCount = 1000;
				maxMagazineCount = 10;
				bulletVelocity = 4f;
				msBetweenShots = 100f;
				ttl = 3f;
				bulletArea = 0;
				reloadTime = 1.2f;
				damage = 1.0f;
			} 
			else if (weaponType == 1) {
				maxBulletCount = 60;
				maxMagazineCount = 10;
				bulletVelocity = 3f;
				msBetweenShots = 100f;
				ttl = 3f;
				bulletArea = 45;
				reloadTime = 1.2f;
				damage = 1.0f;
			} 
			else if (weaponType == 2) {
				maxBulletCount = 64;
				maxMagazineCount = 8;
				bulletVelocity = 4f;
				msBetweenShots = 150f;
				ttl = 7f;
				bulletArea = 15;
				reloadTime = 1.2f;
				damage = 1.0f;
			} 
			else if (weaponType == 3) {
				maxBulletCount = 60;
				maxMagazineCount = 5;
				bulletVelocity = 4f;
				msBetweenShots = 300f;
				ttl = 10f;
				bulletArea = 0;
				reloadTime = 1.2f;
				damage = 1.0f;
			}
		}

		public void setBullet(int type) {
			if (weaponType == 0) {
				bulletVelocity = 3f;
				ttl = 3f;
				damage = 1.0f;
			} 
			else if (weaponType == 1) {
				bulletVelocity = 1.5f;
				ttl = 3f;
				damage = 1.0f;
			} 
			else if (weaponType == 2) {
				bulletVelocity = 3f;
				ttl = 7f;
				damage = 1.0f;
			} 
			else if (weaponType == 3) {
				bulletVelocity = 5f;
				ttl = 10f;
				damage = 1.0f;
			}
		}

		public void refillBullet() {
			remainBullet = maxBulletCount;
			GameObject.Find("Bullet_C_Count").GetComponent<Text>().text = ((int)remainMagazine).ToString();
			//GameObject.Find("Bullet_W_Count").GetComponent<Text>().text = ((int)remainBullet).ToString();
		}

		public IEnumerator Reload()
		{
			reloading = true;
			
			if (remainMagazine >= maxMagazineCount) { // 탄창에 총알이 다 차 있을 경우
				Debug.Log("already max bullet");
				yield return new WaitForSeconds(0.01f);
			} else { // 리로드 할 수 있는 경우
				// placement reload animation && sound

				// delay
				yield return new WaitForSeconds(reloadTime);

				// 다시 채워야하는 총알의 양
				int refill = maxMagazineCount - remainMagazine;
				
				if (remainBullet > refill) { // 남은 총알이 더 많을 경우
					remainMagazine += refill;
					remainBullet -= refill;
				} else { // 남은 총알이 탄창을 다 채울 수 없는 경우
					remainMagazine += remainBullet;
					remainBullet = 0;
				}
			}
			reloading = false;
			
			GameObject.Find("Bullet_C_Count").GetComponent<Text>().text = ((int)remainMagazine).ToString();
			GameObject.Find("Bullet_Max_Count").GetComponent<Text>().text = ((int)remainBullet).ToString();
			//GameObject.Find("Bullet_W_Count").GetComponent<Text>().text = ((int)remainBullet).ToString();
		}

		public void Shoot() {
			Vector3 vBulletDir = Vector3.zero;
			Vector3 vBulletPos = Vector3.zero;

			if (reloading) return;

			if (remainMagazine > 0) {
			
			//if (equippedGun.type == "gun") <<<< add
				if (Time.time > nextShotTime){
					
					nextShotTime = Time.time + msBetweenShots/1000;
					
					Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

					vBulletPos += transform.position;
					vBulletDir = (dir - new Vector3(transform.position.x, transform.position.y,0f)).normalized;
					vBulletDir.z = -0.5f;
					Debug.Log("Hey");
					PhotonView.RPC ("RPC_Shoot", PhotonTargets.All, vBulletPos, vBulletDir, Weaponlayer, weaponType);
					
					remainMagazine--;

					Debug.Log("bullet status" + remainBullet + " remain manazine(clip)" + remainMagazine);
				}
			} else {
				Debug.Log("reload");
				StartCoroutine(Reload());
				
			}
			GameObject.Find("Bullet_C_Count").GetComponent<Text>().text = ((int)remainMagazine).ToString();
			//GameObject.Find("Bullet_W_Count").GetComponent<Text>().text = ((int)remainBullet).ToString();
		}

		[PunRPC]
		private void RPC_Shoot(Vector3 bulletPos, Vector3 bulletDir, int myLayer, int type) {
			Projectile newProjectile = Instantiate(projectileType[type], bulletPos, projectileType[type].transform.rotation) as Projectile;
			///setBullet(type);
			newProjectile.SetProjectile(bulletVelocity, bulletDir, damage, ttl, myLayer);
		}
	}
}