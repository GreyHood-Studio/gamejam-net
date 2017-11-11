using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CreativeSpore.RpgMapEditor
{
    [RequireComponent (typeof(WeaponHandler))]
    [AddComponentMenu("RpgMapEditor/Controllers/PlayerController", 10)]
	public class PlayerController : CharBasicController {

        // UI
        public Text healthUi;

        public Text clipRemainUi;
        public Text weaponRemainUi;


        // Network Related Parameters
		private PhotonView PhotonView;

        // 작업중 Weapon
        WeaponHandler weaponH;

		public float TimerBlockDirSet = 0.6f;
		public Camera2DController Camera2D;
		public float BulletAngDispersion = 15f;
        public SpriteRenderer ShadowSprite;
        //public SpriteRenderer WeaponSprite;
        public int FogSightLength = 5;

        /// <summary>
        /// If player is driving a vehicle, this will be that vehicle
        /// </summary>
        public VehicleCharController Vehicle;        

		private FollowObjectBehaviour m_camera2DFollowBehaviour;

        //#region Singleton and Persistence
        static PlayerController s_instance;
        
        // 0 is left 1 is right;
        int currentDir = 0;
        void Awake()
        {
            /* 
#if UNITY_5_4 || UNITY_5_5_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
*/
            PhotonView = GetComponent<PhotonView> ();

            if(s_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                s_instance = this;
            }
            //else
            //{
            //    DestroyImmediate(gameObject);
            //}      
        }
       // #endregion

        public void UndoDontDestroyOnLoad()
        {
            s_instance = null;
        }

        void OnDestroy()
        {
#if UNITY_5_4 || UNITY_5_5_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
        }

#if UNITY_5_4 || UNITY_5_5_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (mode != UnityEngine.SceneManagement.LoadSceneMode.Single) return;
#else
        void OnLevelWasLoaded()
        {
#endif
            if (s_instance != this) // this happens if UndoDontDestroyOnLoad was called
            {
                DestroyImmediate(gameObject);
            }
        }
        
        public override void SetVisible(bool value)
        {
            base.SetVisible(value);
            ShadowSprite.enabled = value;
            //WeaponSprite.enabled = value;
        }

		protected override void Start () 
		{
            Debug.Log(gameObject.name+ " "  + gameObject.layer.ToString());
            
            base.Start();
            weaponH = GetComponent<WeaponHandler>();

			if( Camera2D == null )
			{
				Camera2D = GameObject.FindObjectOfType<Camera2DController>();
			}

			if (PhotonView.isMine){
                m_camera2DFollowBehaviour = Camera2D.transform.GetComponent<FollowObjectBehaviour> ();
                m_camera2DFollowBehaviour.Target = transform;
            }

            if (!PhotonView.isMine)
                gameObject.transform.GetChild (2).GetComponent<SpriteRenderer> ().enabled = false;
		}

        private int m_lastTileIdx = -1;
        private int m_lastFogSightLength = 0;

        protected override void Update()
		{
            eAnimDir savedAnimDir = m_animCtrl.AnimDirection;
            
            if (!PhotonView.isMine)
				return;

            base.Update();
            /*
            if(m_keepAttackDirTimer > 0f)
            {
                m_keepAttackDirTimer -= Time.deltaTime;
                m_animCtrl.AnimDirection = savedAnimDir;
            }
            */
            m_phyChar.enabled = (Vehicle == null);
            if (Vehicle != null)
            {
                m_animCtrl.IsPlaying = false; 
            }

            else
            {   
                if (Input.GetAxis("Mouse ScrollWheel") != 0f ) {
                    weaponH.Change();
                }

                // character mouse direction 
                Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                if (!isForce){
                    if (dir.x > 0) {
                        m_animCtrl.AnimDirection = eAnimDir.Right;
                        if (currentDir == 0) {
                            // right
                            weaponH.reverseDirection(1);
                            currentDir = 1;
                        }
                    } else if (dir.x < 0) {
                        m_animCtrl.AnimDirection = eAnimDir.Left;
                        if (currentDir == 1) {
                            // left
                            weaponH.reverseDirection(0);
                            currentDir = 0;
                        }
                    }
                }

                // 재장전
                if (Input.GetKey("r")) {
                    weaponH.Reload();
                }

                // 무기 버리기
                if (Input.GetKey("g")) {
                    
                    weaponH.DropGun();
                }

                if (Input.GetMouseButtonDown(0)) { // mousebuttondown(0)
                    weaponH.Shoot();
                }
                
                bool isMoving = (m_phyChar.Dir.sqrMagnitude >= 0.01);
                if (isMoving)
                {
                    //m_phyChar.Dir.Normalize();
                    m_camera2DFollowBehaviour.Target = transform;
                }
                else
                {
                    m_phyChar.Dir = Vector3.zero;
                }
            }

            int tileIdx = RpgMapHelper.GetTileIdxByPosition(transform.position);

            if (tileIdx != m_lastTileIdx || m_lastFogSightLength != FogSightLength)
            {
                RpgMapHelper.RemoveFogOfWarWithFade(transform.position, FogSightLength);
            }

            m_lastFogSightLength = FogSightLength;
            m_lastTileIdx = tileIdx;
		}
        
        // 아이템 줍기
        void OnTriggerStay(Collider other)
        {
            Debug.Log(other.gameObject.tag);
            if (other.gameObject.tag == "items")
            {
                if (Input.GetKey("f")) {
                    if (other.gameObject.name == "AmmoItem") {
                        weaponH.AddBullet();
                        Destroy(other.gameObject);
                    } else if (other.gameObject.name == "HealthItem") {
                        if (GetComponent<DamageBehaviour>().Health < 5){
                            GetComponent<DamageBehaviour>().Health++;
                            GameObject.Find("Health_C_Count").GetComponent<Text>().text = GetComponent<DamageBehaviour>().Health.ToString();
                            Destroy(other.gameObject);
                        }
                    }
                }
            } else if (other.gameObject.tag == "gunfabs") {
                if (Input.GetKey("f")) {
                   
                    this.weaponH.addWeapon(other.gameObject.GetComponent<Weapon>());
                }
            }
        }
	}
}