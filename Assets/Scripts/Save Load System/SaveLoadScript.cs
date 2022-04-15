using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class SaveLoadScript
{
    /// <summary>
    /// Serialize and store data into a file
    /// </summary>
    /// <param name="objectToSave"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool Save(object objectToSave, string fileName)
    {
        BinaryFormatter binaryFormatter = GetBinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "saves");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(Application.persistentDataPath, "saves", fileName + ".sav");
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (fileStream)
        {
            binaryFormatter.Serialize(fileStream, objectToSave);
        }
        return true;
    }

    /// <summary>
    /// Return data from a file after deserializing
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static object Load(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, "saves", fileName + ".sav");
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter binaryFormatter = GetBinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Open);

        using (fileStream)
        {
            object objectToLoad = binaryFormatter.Deserialize(fileStream);
            return objectToLoad;
        }
    }

    /// <summary>
    /// Delete a save file of given name
    /// </summary>
    /// <param name="fileName"></param>
    public static void DeleteSaveGame(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, "saves", fileName + ".sav");
        if (SaveExists(fileName))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Return a binary formatter object with support to serialize vector3 and quaternions
    /// </summary>
    /// <returns></returns>
    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        // add support to serialize vector3 and quarternions
        SurrogateSelector selector = new SurrogateSelector();
        Vector3Surrogate vector3Surrogate = new Vector3Surrogate();
        QuaternionSurrogate quaternionSurrogate = new QuaternionSurrogate();
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }

    /// <summary>
    /// Check if a save file exists of given name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool SaveExists(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, "saves", fileName + ".sav");
        return File.Exists(path);
    }
}
