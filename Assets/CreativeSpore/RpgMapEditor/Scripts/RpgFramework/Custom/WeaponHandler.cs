using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CreativeSpore.RpgMapEditor{
	public class WeaponHandler : MonoBehaviour {
		public Weapon startWeapon;
		public int currentWeapon;
		public Weapon[] weapons = new Weapon[2];
		public Transform weaponHold;
		
		// Use this for initialization
		void Start()
		{
			weapons[0] = null;
			weapons[1] = null;
			weapons[0] = Instantiate(startWeapon, weaponHold.position, weaponHold.rotation) as Weapon;
			weapons[0].setLayer(gameObject.layer);
			weapons[0].transform.parent = weaponHold;
			weapons[0].remainMagazine = weapons[0].maxMagazineCount;
			//weapons[0].Reload();
			currentWeapon = 0;
		}

		public void reverseDirection(int c_dir) {
			// left
			Vector3 temp = weaponHold.transform.localPosition;
			temp.x = -1 * temp.x;
			weaponHold.transform.localPosition = temp;
			if (c_dir ==0) {
				//weaponHold.position = new Vector3 (weaponHold.position);
				weapons[currentWeapon].setDirection(c_dir);
			} else { // right
				//weaponHold.position = new Vector3 (weaponHold.position);
				weapons[currentWeapon].setDirection(c_dir);
			}
			
			//Debug.Log("after position " +weaponHold.transform.localPosition.ToString());
		}

		public void addWeapon(Weapon getWeapon) {
			getWeapon.GetComponent<Collider>().enabled = false;
			getWeapon.gameObject.GetComponent<ItemCheck>().enabled = false;
			getWeapon.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			getWeapon.setLayer(gameObject.layer);

			Vector3 temp= getWeapon.gameObject.transform.position;
			temp.x = weaponHold.position.x;
			temp.y = weaponHold.position.y;
			getWeapon.gameObject.transform.position = temp;
			EquipWeapon(getWeapon);
			
		}

		public void EquipWeapon(Weapon secondWeapon){
			if (weapons[1] != null) {
				DropGun();
			}
			weapons[1] = secondWeapon;
			weapons[1].transform.parent = weaponHold;
			//weapons[1].setLayer(gameObject.layer);
			weapons[1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
			weapons[0].gameObject.GetComponent<SpriteRenderer>().enabled = false;
			//weapons[0].gameObject.SetActive(false);
			//weapons[1].gameObject.SetActive(true);
			currentWeapon = 1;
			weapons[currentWeapon].RefreshBulletCount();
		}

		public void DropGun() {
			if(weapons[1] == null) { return; }
			//if(currentWeapon == 0 ){
				//weapons[1].gameObject.SetActive(true);
			//}
			weapons[1].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
			weapons[1].gameObject.GetComponent<ItemCheck>().enabled = true;
			weapons[1].GetComponent<Collider>().enabled = true;
			weapons[1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
			weapons[1].setLayer(GameObject.FindGameObjectsWithTag("itempos")[0].layer);
			weapons[1].transform.parent = GameObject.FindGameObjectsWithTag("itempos")[0].transform;

			weapons[1] = null;
			
			currentWeapon = 0;
			weapons[0].gameObject.GetComponent<SpriteRenderer>().enabled = true;
			weapons[currentWeapon].RefreshBulletCount();
			//weapons[currentWeapon].gameObject.SetActive(true);
		}

		public void AddBullet() {
			if (weapons[1] != null) {
			weapons[1].refillBullet();
			} else {
				weapons[0].refillBullet();
			}
		}

		public void Change() {
			//Debug.Log("change layer"+ gameObject.layer);
			if (weapons[1] != null) {
				if (currentWeapon == 0) {
					weapons[1].gameObject.GetComponent<SpriteRenderer>().enabled = true;
					weapons[0].gameObject.GetComponent<SpriteRenderer>().enabled = false;
					//weapons[1].gameObject.SetActive(true);
					//weapons[0].gameObject.SetActive(false);
					currentWeapon = 1;
				} else if(currentWeapon == 1) {
					weapons[0].gameObject.GetComponent<SpriteRenderer>().enabled = true;
					weapons[1].gameObject.GetComponent<SpriteRenderer>().enabled = false;
					// weapons[0].gameObject.SetActive(true);
					// weapons[1].gameObject.SetActive(false);
					currentWeapon = 0;
				}
				weapons[currentWeapon].RefreshBulletCount();
				Debug.Log("change weapon" + currentWeapon);
			}	
		}

		public void Reload(){
			StartCoroutine(weapons[currentWeapon].Reload());
		}

		public void Shoot(){
			if (this.weapons[currentWeapon] != null) {
				weapons[currentWeapon].Shoot();
			}
		}
	}
}