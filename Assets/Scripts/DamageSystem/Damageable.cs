using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gamekit2D
{
    public class Damageable : MonoBehaviour/*, IDataPersister*/
    {
        [Serializable]
        public class HealthEvent : UnityEvent<Damageable>
        { }

        [Serializable]
        public class DamageEvent : UnityEvent<Damager, Damageable>
        { }

        public float invulnerabilityDuration = 3f;
        public bool disableOnDeath = false;
        public bool isInvulnerableOnAwake = false;

        public HealthEvent OnResetHealth;
        public DamageEvent OnDie;

        protected bool m_IsAlive = true;
        protected bool m_Invulnerable;
        protected float m_InulnerabilityTimer;

        void Awake()
        {
            if(isInvulnerableOnAwake)
            {
                EnableInvulnerability();
            }
        }

        void Update()
        {
            if (m_Invulnerable)
            {
                m_InulnerabilityTimer -= Time.deltaTime;

                if (m_InulnerabilityTimer <= 0f)
                {
                    m_Invulnerable = false;
                }
            }
        }

        public void EnableInvulnerability(bool ignoreTimer = false)
        {
            m_Invulnerable = true;
            //technically don't ignore timer, just set it to an insanly big number. Allow to avoid to add more test & special case.
            m_InulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
        }

        public void DisableInvulnerability()
        {
            m_Invulnerable = false;
        }

        public bool IsInvulnerable()
        {
            return m_Invulnerable;
        }

        public void TakeDamage(Damager damager, bool canDamageInvincibility = false)
        {
            if (m_Invulnerable && !canDamageInvincibility)
                return;

            m_IsAlive = false;
            OnDie.Invoke(damager, this);
            if (disableOnDeath) gameObject.SetActive(false);

        }

        public void ResetHealth()
        {
            m_IsAlive = true;
            OnResetHealth.Invoke(this);
        }
    }
}
