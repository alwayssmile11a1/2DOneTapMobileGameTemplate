using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gamekit2D
{
    [System.Serializable]
    public class VFX
    {
        public GameObject prefab;
        public float lifetime = 1;

        [System.NonSerialized] public VFXInstancePool pool;
    }

    public class VFXInstance : PoolObject<VFXInstancePool, VFXInstance>, System.IComparable<VFXInstance>
    {
        public float expires;
        public Animation animation;
        public AudioSource audioSource;
        public ParticleSystem[] particleSystems;
        public Transform transform;
        public Transform parent;

        protected override void SetReferences()
        {
            transform = instance.transform;
            animation = instance.GetComponentInChildren<Animation>();
            audioSource = instance.GetComponentInChildren<AudioSource>();
            particleSystems = instance.GetComponentsInChildren<ParticleSystem>();
        }

        public override void WakeUp()
        {
            instance.SetActive(true);
            for (var i = 0; i < particleSystems.Length; i++)
                particleSystems[i].Play();
            if (animation != null)
            {
                animation.Rewind();
                animation.Play();
            }
            if (audioSource != null)
                audioSource.Play();
        }

        public override void Sleep()
        {
            for (var i = 0; i < particleSystems.Length; i++)
            {
                particleSystems[i].Stop();
            }
            if (animation != null)
                animation.Stop();
            if (audioSource != null)
                audioSource.Stop();
            instance.SetActive(false);
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public int CompareTo(VFXInstance other)
        {
            return expires.CompareTo(other.expires);
        }

    }

    public class VFXInstancePool : ObjectPool<VFXInstancePool, VFXInstance>
    {

    }

    public class VFXController : MonoBehaviour
    {
        public static VFXController Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = FindObjectOfType<VFXController> ();

                if (instance != null)
                    return instance;

                return CreateDefault ();
            }
        }

        protected static VFXController instance;

        public static VFXController CreateDefault ()
        {
            VFXController controllerPrefab = Resources.Load<VFXController> ("VFXController");
            instance = Instantiate (controllerPrefab);
            return instance;
        }

        struct PendingVFX : System.IComparable<PendingVFX>
        {
            public VFX vfx;
            public Vector3 position;
            public float startAt;
            public bool flip;
            public Transform parent;

            public int CompareTo(PendingVFX other)
            {
                return startAt.CompareTo(other.startAt);
            }
        }


        public VFX[] vfxConfig;

        Dictionary<int, VFX> m_FxPools = new Dictionary<int, VFX>();
        PriorityQueue<VFXInstance> m_RunningFx = new PriorityQueue<VFXInstance>();
        PriorityQueue<PendingVFX> m_PendingFx = new PriorityQueue<PendingVFX>();

        public void Awake()
        {
            if (Instance != this)
            {
                Destroy (gameObject);
                return;
            }

            DontDestroyOnLoad (gameObject);

            foreach (var vfx in vfxConfig)
            {
                vfx.pool = gameObject.AddComponent<VFXInstancePool>();
                vfx.pool.initialPoolCount = 2;
                vfx.pool.prefab = vfx.prefab;
 
                m_FxPools[StringToHash(vfx.prefab.name)] = vfx;
            }
        }

        public void Trigger(string name, Vector3 position, float startDelay, bool flip, Transform parent)
        {
            Trigger(StringToHash(name), position, startDelay, flip, parent);
        }

        public void Trigger(int hash, Vector3 position, float startDelay, bool flip, Transform parent)
        {
            VFX vfx;
            if (!m_FxPools.TryGetValue(hash, out vfx))
            {
                Debug.LogWarning("VFX does not exist.");
            }
            else
            {
                if (startDelay > 0)
                {
                    m_PendingFx.Push(new PendingVFX() { vfx = vfx, position = position, startAt = Time.time + startDelay, flip = flip, parent = parent });
                }
                else
                    CreateInstance(vfx, position, flip, parent);
            }
        }

        void Update()
        {
            while (!m_RunningFx.Empty && m_RunningFx.First.expires <= Time.time)
            {
                var instance = m_RunningFx.Pop();
                instance.objectPool.Push(instance);
            }
            while (!m_PendingFx.Empty && m_PendingFx.First.startAt <= Time.time)
            {
                var task = m_PendingFx.Pop();
                CreateInstance(task.vfx, task.position, task.flip, task.parent);
            }
            var instances = m_RunningFx.items;
            for (var i = 0; i < instances.Count; i++)
            {
                var vfx = instances[i];
                if (vfx.parent != null)
                    vfx.transform.position = vfx.parent.position;
            }
        }

        void CreateInstance(VFX vfx, Vector3 position, bool flip, Transform parent)
        {
            VFXInstancePool poolToUse = vfx.pool;

            var instance = poolToUse.Pop();

            instance.expires = Time.time + vfx.lifetime;
            if (flip)
                instance.transform.localScale = new Vector3(-1, 1, 1);
            else
                instance.transform.localScale = new Vector3(1, 1, 1);
            instance.parent = parent;
            instance.SetPosition(position);
            m_RunningFx.Push(instance);
        }

        public static int StringToHash(string name)
        {
            return name.GetHashCode();
        }


    }
}