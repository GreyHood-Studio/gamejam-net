using UnityEngine;
using System.Collections;


namespace CreativeSpore.RpgMapEditor
{
    [RequireComponent (typeof(WeaponHandler))]
    [AddComponentMenu("RpgMapEditor/Controllers/PlayerController", 10)]
	public class PlayerController : CharBasicController {

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
        
        void Awake()
        {
            /* 
#if UNITY_5_4 || UNITY_5_5_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
*/
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
            base.Start();
            weaponH = GetComponent<WeaponHandler>();
			if( Camera2D == null )
			{
				Camera2D = GameObject.FindObjectOfType<Camera2DController>();
			}
			
			m_camera2DFollowBehaviour = Camera2D.transform.GetComponent<FollowObjectBehaviour>();
			m_camera2DFollowBehaviour.Target = transform;
		}

        private int m_lastTileIdx = -1;
        private int m_lastFogSightLength = 0;

        protected override void Update()
		{
            eAnimDir savedAnimDir = m_animCtrl.AnimDirection;
            
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
                // character mouse direction 
                Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
                if (dir.x > 0) {
                    m_animCtrl.AnimDirection = eAnimDir.Right;
                } else if (dir.x < 0) {
                    m_animCtrl.AnimDirection = eAnimDir.Left;
                }

                if (Input.GetKeyDown("r")) { // mousebuttondown(0)
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
	}
}