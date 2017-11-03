using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CreativeSpore.RpgMapEditor{
	public class WeaponHandler : MonoBehaviour {

		Weapon equippedGun;
		// Use this for initialization
		public void Shoot(){
			if (equippedGun != null) {
				
				equippedGun.Shoot();
			}
		}
	}
}
