using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [RequireComponent(typeof(DirectionalAnimation))]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    [AddComponentMenu("RpgMapEditor/Controllers/CharBasicController", 10)]
    public class CharBasicController : MonoBehaviour
    {
        protected bool isForce = false;
        // dash cool down
        public float flash_cd = 2.0f;
        // dash end time
        protected float end = 0.35f;

        protected float flash_start = 0.0f;
        protected float start = 0.0f;
        protected Vector3 d_dir;

        public DirectionalAnimation AnimCtrl { get { return m_animCtrl; } }
        public PhysicCharBehaviour PhyCtrl { get { return m_phyChar; } }

        public bool IsVisible
        {
            get
            {
                return m_animCtrl.TargetSpriteRenderer.enabled;
            }

            set
            {
                SetVisible( value );
            }
        }
        protected bool isDash = false;
        protected DirectionalAnimation m_animCtrl;
        protected PhysicCharBehaviour m_phyChar;

        protected float m_timerBlockDir = 0f;

        protected virtual void Start()
        {
            m_animCtrl = GetComponent<DirectionalAnimation>();
            m_phyChar = GetComponent<PhysicCharBehaviour>();
        }
        
        protected virtual void Update()
        {
            float fAxisX = 0.0f;
            float fAxisY = 0.0f;
            
            // during dash
            if (isForce) {
                
                if (Time.time < start + end) {

                    transform.Translate(d_dir * Time.deltaTime * 3.0f);
                    return;
                } else {
                    d_dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
                    gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    isForce = false;

                    if (d_dir.x > 0) {
                        AnimCtrl.AnimDirection = eAnimDir.Right;
                    } else {
                        AnimCtrl.AnimDirection = eAnimDir.Left;
                    }
                }
            } 

            if (Input.GetMouseButtonDown(1)) {
                // flash cooldown
                if (Time.time > flash_start + flash_cd) {
                    gameObject.GetComponent<BoxCollider>().isTrigger = false;
                    isForce = true;
                    flash_start = Time.time;
                    start = Time.time;

                    d_dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
                    d_dir.z = 0;
                    if (d_dir.x < 0) {
                        AnimCtrl.AnimDirection = eAnimDir.Down;
                    }
                    else {
                        AnimCtrl.AnimDirection = eAnimDir.Up;
                    }
                }
            } else {
                // else (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                fAxisX = Input.GetAxis("Horizontal");
                fAxisY = Input.GetAxis("Vertical");
                //Debug.Log("input movement horizon vertical" + fAxisX+" "+fAxisY);
                UpdateMovement(fAxisX, fAxisY);
            }
        }

        protected void cooldown_effect(){
            
        }


        void UpdateDash(Vector3 dashPosition) {
            
            

           
            //}
        }

        void UpdateEvade(Vector3 edir) {
            //Debug.Log("Evade: " +edir.ToString());
            m_phyChar.isEvade = true;
            m_phyChar.Dir = edir;
            
            m_animCtrl.IsPlaying = m_phyChar.IsMoving;
            isDash=true;

            if (edir.x > 0) { // right
                m_animCtrl.AnimDirection = eAnimDir.Up;
            } else { // left
                m_animCtrl.AnimDirection = eAnimDir.Down;
            }   
                
            //m_animCtrl.SetAnimDirection(m_phyChar.Dir);
            isDash=false;
        }

        protected void UpdateMovement( float fAxisX, float fAxisY )
        {
            m_timerBlockDir -= Time.deltaTime;
            m_phyChar.Dir = new Vector3(fAxisX, fAxisY, 0);

            m_animCtrl.IsPlaying = m_phyChar.IsMoving;
            m_animCtrl.SetAnimDirection(m_phyChar.Dir);
        }

        public virtual void SetVisible( bool value )
        {
            m_animCtrl.TargetSpriteRenderer.enabled = value;
        }
    }
}