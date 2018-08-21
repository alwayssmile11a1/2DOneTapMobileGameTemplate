using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetDatabase : MonoBehaviour
{

    #region Singleton
    public static AssetDatabase Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<AssetDatabase>();

            if (s_Instance != null)
                return s_Instance;

            CreateNew();

            return s_Instance;
        }
    }

    private static AssetDatabase s_Instance;

    private static void CreateNew()
    {
        //Create new 
        GameObject playerDataObject = new GameObject("AssetDatabase");
        s_Instance = playerDataObject.AddComponent<AssetDatabase>();
        s_Instance.LoadDatabase();
    }

    #endregion

    public bool IsLoaded { get; private set; }
    public bool IsLoading { get; private set; }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if(!IsLoaded)
        {
            s_Instance.LoadDatabase();
        }
    }

    public void LoadDatabase()
    {

    }
  

}
