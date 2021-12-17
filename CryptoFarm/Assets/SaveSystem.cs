using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string gameControllerFile = "/gameController.gc";
    private static string minerControllerFile = "/minerController.gc";
    private static string powerControllerFile = "/powerController.gc";

    public static void SaveGameController(GameController gameController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + gameControllerFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        DataGameController data = new DataGameController(gameController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataGameController LoadGameController()
    {
        string path = Application.persistentDataPath + gameControllerFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataGameController data = formatter.Deserialize(stream) as DataGameController;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveMinerController(MinerController minerController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + minerControllerFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        DataMinerController data = new DataMinerController(minerController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataMinerController LoadMinerController()
    {
        string path = Application.persistentDataPath + minerControllerFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataMinerController data = formatter.Deserialize(stream) as DataMinerController;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SavePowerController(PowerController powerController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + powerControllerFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        DataPowerController data = new DataPowerController(powerController);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataPowerController LoadPowerController()
    {
        string path = Application.persistentDataPath + powerControllerFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataPowerController data = formatter.Deserialize(stream) as DataPowerController;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
