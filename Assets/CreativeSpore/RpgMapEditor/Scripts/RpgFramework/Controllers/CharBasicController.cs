using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [RequireComponent(typeof(DirectionalAnimation))]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    [AddComponentMenu("RpgMapEditor/Controllers/CharBasicController", 10)]
    public class CharBasicController : MonoBehaviour
    {
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
            
            if (Input.GetMouseButtonDown(1)) {
                Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
                dir.z = 0;
                
                UpdateEvade(dir);
            } else {
                // else (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                fAxisX = Input.GetAxis("Horizontal");
                fAxisY = Input.GetAxis("Vertical");
                //Debug.Log("input movement horizon vertical" + fAxisX+" "+fAxisY);
                UpdateMovement(fAxisX, fAxisY);
            }
        }

        protected void UpdateEvade(Vector3 edir) {
            Debug.Log("Evade: " +edir.ToString());
            m_phyChar.isEvade = true;
            m_phyChar.Dir = edir;

            //m_animCtrl.IsPlaying = m_phyChar.IsMoving;
            //m_animCtrl.SetAnimDirection(m_phyChar.Dir);
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