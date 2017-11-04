using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CreativeSpore.RpgMapEditor{
	public class WeaponHandler : MonoBehaviour {

		public int currentWeapon;
		public Weapon[] weapons = new Weapon[2];
		public Transform weaponHold;
		public Weapon startWeapon;

		// Use this for initialization
		
		void Start()
		{
			weapons[0] = null;
			weapons[1] = null;

			weapons[0] = Instantiate(startWeapon, new Vector3(weaponHold.position.x,weaponHold.position.y,0) , weaponHold.rotation) as Weapon;
			weapons[0].setLayer(gameObject.layer);
			weapons[0].transform.parent = weaponHold;
		}

		public void EquipWeapon(Weapon secondWeapon){

			if (weapons[1] == null) {
				weapons[1] = secondWeapon;
				weapons[1].setLayer(gameObject.layer);
				weapons[1].transform.parent = weaponHold;
			} else {
				DropGun(weapons[1]);
				weapons[1] = secondWeapon;
				weapons[1].setLayer(gameObject.layer);
				weapons[1].transform.parent = weaponHold;
			}

			weapons[0].gameObject.SetActive(false);
			weapons[1].gameObject.SetActive(true);
			currentWeapon = 1;
		}

		public void DropGun(Weapon dropWeapon) {
			if(currentWeapon == 0 ){
				weapons[1].gameObject.SetActive(true);
			}
			dropWeapon.transform.parent = null;
			weapons[1] = null;
		}

		public void AddBullet() {
			weapons[1].refillBullet();
		}

		public void Change() {
			if (weapons[1] != null) {
				if (currentWeapon == 0) {
					weapons[1].gameObject.SetActive(true);
					weapons[0].gameObject.SetActive(false);
					currentWeapon = 1;
				} else if(currentWeapon == 1) {
					weapons[0].gameObject.SetActive(true);
					weapons[1].gameObject.SetActive(false);
					currentWeapon = 0;
				}
			}
			Debug.Log("change weapon" + currentWeapon);
		}

		public void Reload(){
			StartCoroutine(weapons[currentWeapon].Reload());
		}

		public void Shoot(){
			if (weapons[currentWeapon] != null) {
				weapons[currentWeapon].Shoot();
			}
		}
	}
}
