using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
    public class DefaultObjectPool : ObjectPool<DefaultObjectPool, DefaultPoolObject, Vector2>
    {
        static protected Dictionary<GameObject, DefaultObjectPool> s_PoolInstances = new Dictionary<GameObject, DefaultObjectPool>();

        private void Awake()
        {
            //This allow to make Pool manually added in the scene still automatically findable & usable
            if (prefab != null && !s_PoolInstances.ContainsKey(prefab))
                s_PoolInstances.Add(prefab, this);
        }


        private void OnDestroy()
        {
            s_PoolInstances.Remove(prefab);
        }

        //initialPoolCount is only used when the objectpool don't exist
        static public DefaultObjectPool GetObjectPool(GameObject prefab, int initialPoolCount = 10)
        {
            DefaultObjectPool objPool = null;
            if (!s_PoolInstances.TryGetValue(prefab, out objPool))
            {
                GameObject obj = new GameObject(prefab.name + "_Pool");
                objPool = obj.AddComponent<DefaultObjectPool>();
                objPool.prefab = prefab;
                objPool.initialPoolCount = initialPoolCount;

                s_PoolInstances[prefab] = objPool;
            }

            return objPool;
        }
    }

    public class DefaultPoolObject : PoolObject<DefaultObjectPool, DefaultPoolObject, Vector2>
    {
        public Transform transform;
        public Rigidbody2D rigidbody2D;
        public SpriteRenderer spriteRenderer;

        protected override void SetReferences()
        {
            transform = instance.transform;
            rigidbody2D = instance.GetComponent<Rigidbody2D>();
            spriteRenderer = instance.GetComponent<SpriteRenderer>();
        }

        public override void WakeUp(Vector2 position)
        {
            transform.position = position;
            instance.SetActive(true);
        }

        public override void Sleep()
        {
            instance.SetActive(false);
        }
    }
}
