using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gamekit2D
{
    public class Damager : MonoBehaviour
    {
        [Serializable]
        public class DamagableEvent : UnityEvent<Damager, Damageable>
        { }


        [Serializable]
        public class NonDamagableEvent : UnityEvent<Damager>
        { }

        //call this inside the onDamageableHit or OnNonDamageableHit to get what was hit.
        public Collider2D LastHit { get { return m_LastHit; } }

        [Tooltip("Recommending using this if the object involve rotating since damager area haven't supported rotating yet")]
        public bool useTriggerCollider = false;
        public Vector2 offset = new Vector2(0f, 0f);
        public Vector2 size = new Vector2(2.5f, 1f);
        [Tooltip("If this is set, the offset x will be changed base on the sprite flipX setting. e.g. Allow to make the damager always forward in the direction of sprite")]
        public bool offsetBasedOnSpriteFacing = true;
        [Tooltip("SpriteRenderer used to read the flipX value used by offset Based OnSprite Facing")]
        public SpriteRenderer spriteRenderer;
        [Tooltip("If set, enable damager on awake")]
        public bool enableDamageOnAwake = true;
        [Tooltip("If disabled, damager ignore trigger when casting for damage (only work when not using trigger collider)")]
        public bool canHitTriggers;
        public bool disableDamageAfterHit = false;
        [Tooltip("If set, OnHitEvent will still be triggered when hitting an invincible gameObject")]
        public bool receiveOnHitEventOnInvincibleObject = true;
        [Tooltip("If set, an invincible damageable object will still get hit")]
        public bool ignoreInvincibility = false;
        public LayerMask hittableLayers;
        public DamagableEvent OnDamageableHit;
        public NonDamagableEvent OnNonDamageableHit;


        protected bool m_SpriteOriginallyFlipped;
        protected bool m_CanDamage = true;
        protected ContactFilter2D m_AttackContactFilter;
        protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];
        protected Transform m_DamagerTransform;
        protected Collider2D m_LastHit;

        void Awake()
        {
            m_AttackContactFilter.layerMask = hittableLayers;
            m_AttackContactFilter.useLayerMask = true;
            m_AttackContactFilter.useTriggers = canHitTriggers;

            if (offsetBasedOnSpriteFacing && spriteRenderer != null)
                m_SpriteOriginallyFlipped = spriteRenderer.flipX;

            m_DamagerTransform = transform;
            m_CanDamage = enableDamageOnAwake;
        }

        public void EnableDamage()
        {
            m_CanDamage = true;
        }

        public void DisableDamage()
        {
            m_CanDamage = false;
        }

        public bool CanDamage()
        {
            return m_CanDamage;
        }

        public ContactFilter2D GetContactFilter()
        {
            return m_AttackContactFilter;
        }

        void FixedUpdate()
        {
            if (!m_CanDamage || useTriggerCollider)
                return;

            //Get global scale of this object 
            Vector2 scale = m_DamagerTransform.lossyScale;

            //Calculate the actual offset of this damager based on transform scale
            Vector2 facingOffset = Vector2.Scale(offset, scale);
            if (offsetBasedOnSpriteFacing && spriteRenderer != null && spriteRenderer.flipX != m_SpriteOriginallyFlipped)
                facingOffset = new Vector2(-offset.x * scale.x, offset.y * scale.y);

            //Calculate the actual size of this damager
            Vector2 scaledSize = Vector2.Scale(size, scale);
            
            //Calculate the bottom left position and the top right position 
            Vector2 pointA = (Vector2)m_DamagerTransform.position + facingOffset - scaledSize * 0.5f;
            Vector2 pointB = pointA + scaledSize;

            //check overlap
            int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);           

            //loop through all the collider2d that was hit
            for (int i = 0; i < hitCount; i++)
            {
                m_LastHit = m_AttackOverlapResults[i];
                
                RunEvent();
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!m_CanDamage || !useTriggerCollider||!m_AttackContactFilter.layerMask.Contains(collision.gameObject)) return;

            m_LastHit = collision;

            RunEvent();
            
        }


        private void RunEvent()
        {
            //get damageable component if exist
            Damageable damageable = m_LastHit.GetComponent<Damageable>();

            if (damageable)
            {
                if (receiveOnHitEventOnInvincibleObject || !damageable.IsInvulnerable())
                {
                    OnDamageableHit.Invoke(this, damageable);
                }

                damageable.TakeDamage(this, ignoreInvincibility);

                if (disableDamageAfterHit)
                {

                    DisableDamage();
                }
            }
            else
            {
                OnNonDamageableHit.Invoke(this);

                if (disableDamageAfterHit)
                {
                    DisableDamage();
                }
            }
        }

    }
}
