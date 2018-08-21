using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Serializable data
/// For the ease of use, use DataSaver instead 
/// </summary>
[System.Serializable]
public class SerializableSavedData
{

    public List<string> stringKeys;
    public List<string> stringData;

    public List<string> intKeys;
    public List<int> intData;

    public List<string> floatKeys;
    public List<float> floatData;

    public List<string> boolKeys;
    public List<bool> boolData;

    public List<string> vectorKeys;
    public List<SerializableVector3> vectorData;

}

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

}

public class DataSaver
{
    protected Dictionary<string, string> m_StringSavedData;
    protected Dictionary<string, bool> m_BoolSavedData;
    protected Dictionary<string, int> m_IntSavedData;
    protected Dictionary<string, float> m_FloatSavedData;
    protected Dictionary<string, SerializableVector3> m_Vector3SavedData;


    public DataSaver()
    {
        m_StringSavedData = new Dictionary<string, string>();
        m_BoolSavedData = new Dictionary<string, bool>();
        m_IntSavedData = new Dictionary<string, int>();
        m_FloatSavedData = new Dictionary<string, float>();
        m_Vector3SavedData = new Dictionary<string, SerializableVector3>();
    }

    public string GetString(string key)
    {
        string value;
        if (m_StringSavedData.TryGetValue(key, out value))
            return value;
        throw new UnityException("No string data was found with the key - " + key);
    }

    public bool GetBool(string key)
    {
        bool value;

        if (m_BoolSavedData.TryGetValue(key, out value))
            return value;
        throw new UnityException("No bool data was found with the key - " + key);
    }

    public int GetInt(string key)
    {
        int value;
        if (m_IntSavedData.TryGetValue(key, out value))
            return value;
        throw new UnityException("No int data was found with the key - " + key);
    }

    public float GetFloat(string key)
    {
        float value;
        if (m_FloatSavedData.TryGetValue(key, out value))
            return value;
        throw new UnityException("No float data was found with the key - " + key);
    }

    public Vector3 GetVector3(string key)
    {
        SerializableVector3 value;
        if (m_Vector3SavedData.TryGetValue(key, out value))
            return new Vector3(value.x, value.y, value.z);
        throw new UnityException("No Vector3 data was found with the key - " + key);
    }


    public void Set(string key, string value)
    {
        if (m_StringSavedData.ContainsKey(key))
            m_StringSavedData[key] = value;
        else
            m_StringSavedData.Add(key, value);
    }

    public void Set(string key, bool value)
    {
        if (m_BoolSavedData.ContainsKey(key))
            m_BoolSavedData[key] = value;
        else
            m_BoolSavedData.Add(key, value);
    }

    public void Set(string key, int value)
    {
        if (m_IntSavedData.ContainsKey(key))
            m_IntSavedData[key] = value;
        else
            m_IntSavedData.Add(key, value);
    }

    public void Set(string key, float value)
    {
        if (m_FloatSavedData.ContainsKey(key))
            m_FloatSavedData[key] = value;
        else
            m_FloatSavedData.Add(key, value);
    }

    public void Set(string key, Vector3 value)
    {
        SerializableVector3 serializableVector3 = new SerializableVector3();
        serializableVector3.x = value.x;
        serializableVector3.y = value.y;
        serializableVector3.z = value.z;

        if (m_Vector3SavedData.ContainsKey(key))
        {
            m_Vector3SavedData[key] = serializableVector3;
        }
        else
        {
            m_Vector3SavedData.Add(key, serializableVector3);
        }
    }

    public void ClearAll()
    {
        m_StringSavedData.Clear();
        m_BoolSavedData.Clear();
        m_IntSavedData.Clear();
        m_FloatSavedData.Clear();
        m_Vector3SavedData.Clear();
    }

    public SerializableSavedData GetSeralizableData()
    {
        //Since Unity doesn't support serializing Dictionary, we have to convert it to List 
        SerializableSavedData serializableData = new SerializableSavedData();

        serializableData.stringKeys = new List<string>(m_StringSavedData.Keys);
        serializableData.stringData = new List<string>(m_StringSavedData.Values);

        serializableData.intKeys = new List<string>(m_IntSavedData.Keys);
        serializableData.intData = new List<int>(m_IntSavedData.Values);

        serializableData.floatKeys = new List<string>(m_FloatSavedData.Keys);
        serializableData.floatData = new List<float>(m_FloatSavedData.Values);

        serializableData.boolKeys = new List<string>(m_BoolSavedData.Keys);
        serializableData.boolData = new List<bool>(m_BoolSavedData.Values);

        serializableData.vectorKeys = new List<string>(m_Vector3SavedData.Keys);
        serializableData.vectorData = new List<SerializableVector3>(m_Vector3SavedData.Values);


        return serializableData;
    }


    public void FromSerializableData(SerializableSavedData serializableData)
    {
        //String data
        m_StringSavedData = new Dictionary<string, string>();
        for (int i = 0; i < serializableData.stringKeys.Count; i++)
        {
            m_StringSavedData.Add(serializableData.stringKeys[i], serializableData.stringData[i]);
        }

        //Int data
        m_IntSavedData = new Dictionary<string, int>();
        for (int i = 0; i < serializableData.intKeys.Count; i++)
        {
            m_IntSavedData.Add(serializableData.intKeys[i], serializableData.intData[i]);
        }

        //Float data
        m_FloatSavedData = new Dictionary<string, float>();
        for (int i = 0; i < serializableData.floatKeys.Count; i++)
        {
            m_FloatSavedData.Add(serializableData.floatKeys[i], serializableData.floatData[i]);
        }

        //Bool data
        m_BoolSavedData = new Dictionary<string, bool>();
        for (int i = 0; i < serializableData.boolKeys.Count; i++)
        {
            m_BoolSavedData.Add(serializableData.boolKeys[i], serializableData.boolData[i]);
        }

        //Vector data
        m_Vector3SavedData = new Dictionary<string, SerializableVector3>();
        for (int i = 0; i < serializableData.vectorKeys.Count; i++)
        {
            m_Vector3SavedData.Add(serializableData.vectorKeys[i], serializableData.vectorData[i]);
        }
    }


    public void Save(string path)
    {

        BinaryFormatter bf = new BinaryFormatter();
        path = Path.Combine(Application.persistentDataPath, path);
        FileStream file = File.Create(path);

        //Since Unity doesn't support serializing Dictionary, we have to convert it to List 
        SerializableSavedData serializableData = this.GetSeralizableData();

        bf.Serialize(file, serializableData);

        file.Close();
    }

    /// <summary>
    /// Load data and return true if load succeeded
    /// </summary>
    /// <returns></returns>
    public bool Load(string path)
    {
        path = Path.Combine(Application.persistentDataPath, path);

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            SerializableSavedData serializableData = (SerializableSavedData)bf.Deserialize(file);

            this.FromSerializableData(serializableData);

            file.Close();

            return true;
        }

        return false;

    }

    public static void DeleteData(string path)
    {
        path = Path.Combine(Application.persistentDataPath, path);
        if (File.Exists(path))
        {
            File.Delete(path);

        }
    }

}