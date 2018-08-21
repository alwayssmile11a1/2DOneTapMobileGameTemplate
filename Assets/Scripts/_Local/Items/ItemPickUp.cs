using Gamekit2D;
using UnityEngine;

public class ItemPickUp : MonoBehaviour {

    public LayerMask PickableLayer;
    public IntVariable itemPickUpCount;
    public string vfxName = "ItemPickUp";

    [HideInInspector]
    public DefaultPoolObject poolObject;

    private int VFX_HASH;


    private void Awake()
    {
        VFX_HASH = VFXController.StringToHash(vfxName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PickableLayer.Contains(collision.gameObject))
        {
            gameObject.SetActive(false);
            VFXController.Instance.Trigger(VFX_HASH, transform.position, 0, false, null);
            itemPickUpCount.Value++;
        }
    }

}
