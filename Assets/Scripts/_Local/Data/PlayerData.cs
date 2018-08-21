using System.IO;
using UnityEngine;

/// <summary>
/// Save and load player data
/// </summary>
public class PlayerData : MonoBehaviour {


    #region Singleton
    public static PlayerData Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<PlayerData>();

            if (s_Instance != null)
                return s_Instance;

            CreateNew();

            return s_Instance;
        }
    }

    private static PlayerData s_Instance;

    private static void CreateNew()
    {
        //Create new 
        GameObject playerDataObject = new GameObject("PlayerData");
        s_Instance = playerDataObject.AddComponent<PlayerData>();
        if (!s_Instance.IsLoaded)
        {
            s_Instance.Load();
        }
    }

    #endregion

    [HideInInspector]
    public string currentCharacter;
    [HideInInspector]
    public string currentLevel;
    [HideInInspector]
    public string currentDifficulty;
    [HideInInspector]
    public bool currentMuteMusicState;
    [HideInInspector]
    public bool isFirstTimePlaying = true;

    public bool IsLoaded { get; private set; }

    private DataSaver m_DataSaver = new DataSaver();

    private const string m_Path = "path.dat";

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (!IsLoaded)
        {
            Instance.Load();
        }
    }


    public void Save()
    {
        m_DataSaver.ClearAll();

        m_DataSaver.Set("character", currentCharacter);
        m_DataSaver.Set("level", currentLevel);
        m_DataSaver.Set("difficulty", currentDifficulty);
        m_DataSaver.Set("mute", currentMuteMusicState);

        m_DataSaver.Save(m_Path);
    }

    public void Load()
    {
        if (m_DataSaver.Load(m_Path))
        {
            currentCharacter = m_DataSaver.GetString("character");
            currentLevel = m_DataSaver.GetString("level");
            currentDifficulty = m_DataSaver.GetString("difficulty");
            currentMuteMusicState = m_DataSaver.GetBool("mute");
        }
        IsLoaded = true;
    }

    public void DeleteAllData()
    {
        DataSaver.DeleteData(m_Path);
    }

}
