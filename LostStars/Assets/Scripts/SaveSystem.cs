using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using StarterAssets;

public static class SaveSystem
{
    public static void SaveGame(FirstPersonController player, int saveSlot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        string path = Application.persistentDataPath + "/Saves/Slot" + saveSlot + ".xaq";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame(int saveSlot)
    {
        string path = Application.persistentDataPath + "/Saves/Slot" + saveSlot + ".xaq";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found at [" + path + "]");
            return null;
        }
    }
}
