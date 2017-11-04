using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CreativeSpore.RpgMapEditor{
	public class WeaponHandler : MonoBehaviour {

		public Transform weaponHold;
        public Weapon startWeapon;
		Weapon equippedGun;
		// Use this for initialization
		
		void Start()
		{
			if (startWeapon != null) {
				EquipWeapon(startWeapon);
			}

		}
			
		public void EquipWeapon(Weapon weaponToEquip){
		if (equippedGun != null){
			Destroy(equippedGun.gameObject);
		}
		
		equippedGun = Instantiate(weaponToEquip, new Vector3(weaponHold.position.x,weaponHold.position.y,0) , weaponHold.rotation) as Weapon;
		equippedGun.setLayer(gameObject.layer);
		equippedGun.transform.parent = weaponHold;
		}

		public void Reload(){
			StartCoroutine(equippedGun.Reload());
		}

		public void Shoot(){
			if (equippedGun != null) {
				equippedGun.Shoot();
			}
		}
	}
}
