using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreativeSpore.RpgMapEditor{
	public class Projectile : MonoBehaviour {

		public float Speed = 0.1f;
		public Vector3 Dir = new Vector3();
		public float TimeToLive = 5f;
        public float DamageQty = 0.5f;

		public GameObject OnDestroyFx;
		public bool IsDestroyOnCollision = true;

		public void SetProjectile(float _speed, Vector3 bulletDir, float damage, float ttl, int Bulletlayer) {
			Speed = _speed;
			Dir = bulletDir;
			DamageQty = damage;
			TimeToLive = ttl;
			gameObject.layer = Bulletlayer;
			Destroy( transform.gameObject, TimeToLive);
		}
		void Start()
		{
			
		}

		void Update () 
		{
			if( AutoTileMap.Instance.GetAutotileCollisionAtPosition( transform.position ) == eTileCollisionType.BLOCK )
			{
				Destroy( transform.gameObject );
			}
			//transform.Translate(Speed * new Vector3(1,1,0) * Time.deltaTime);
			transform.position += Speed * Dir * Time.deltaTime;
		}

		void OnDestroy()
		{
			if( OnDestroyFx != null )
			{
				Instantiate( OnDestroyFx, transform.position, transform.rotation );
			}
		}
		
		void OnTriggerStay(Collider other) 
		{
			if( IsDestroyOnCollision && other.attachedRigidbody && (other.gameObject.layer != gameObject.layer) )
			{
				//apply damage here
                DamageData.ApplyDamage(other.attachedRigidbody.gameObject, DamageQty, Dir);
				Destroy(gameObject);
			}
		}
	}
}