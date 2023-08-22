using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class DataSerializer
{
    public static void SaveJson(SaveData saveData, string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        string jsonString = JsonConvert.SerializeObject(saveData);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(jsonString);
        }
    }

    public static SaveData LoadJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string jsonString = reader.ReadToEnd();

                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(jsonString);

                return saveData;
            }
        }
        return null;
    }
}