using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// This is the data structure we want to save
[System.Serializable]
public class GameData
{
    public Dictionary<int, bool> LevelKeys = new Dictionary<int, bool>();
    public Dictionary<int, bool> TroopKeys = new Dictionary<int, bool>();
    public int TroopSpaces;
    public int[] ActiveRoster = new int [5];
}

public class SaveManager : MonoBehaviour
{
    public List<Unit> UnitCoder = new List<Unit>();

    public int counter = 0;
    public Unit unit;
    private string saveFilePath;

    /*[SerializeField]
    private Dictionary<int, Unit> UnitCoder = new Dictionary<int, Unit>() {
        {1, unit}

    };*/

    private void Start()
    {
        // Set the file path to save data to
        saveFilePath = Application.persistentDataPath + "/savedata.dat";
    }

    // Save the game data to disk
    public void SaveGame(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFilePath, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Load the game data from disk
    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }

    public void Fuckyou() {
        GameData SaveFile = new GameData();

        //Saves most of the data directly from the static player info script
        SaveFile.LevelKeys = PlayerInfo.LevelKeys;
        SaveFile.TroopKeys = PlayerInfo.TroopKeys;
        SaveFile.TroopSpaces = PlayerInfo.TroopSpaces;

        //Codes the units into an array based on their numbers
        int [] CodedRoster = new int[PlayerInfo.PlayerTroops.Length];
        for (int i = 0; i < PlayerInfo.PlayerTroops.Length; i++) {
            if (PlayerInfo.PlayerTroops[i] != null)
            {
                CodedRoster[i] = PlayerInfo.PlayerTroops[i].UnitNumber;
                Debug.Log("Coded Roster is " + CodedRoster[i]);
            }
            else {
                CodedRoster[i] = -1;
            }
        }
        SaveFile.ActiveRoster = CodedRoster;

        //Saves game
        SaveGame(SaveFile);
    }


    public void UnFuckYou() {
        //Pulls the data out
        GameData SaveFile = LoadGame();

        //Recopies most of the data
        PlayerInfo.LevelKeys = SaveFile.LevelKeys;
        PlayerInfo.TroopKeys = SaveFile.TroopKeys;
        PlayerInfo.TroopSpaces = SaveFile.TroopSpaces;

        //I am using a list since it was easy to edit in the inspector but decodes the troops to the player for their use
        for (int i = 0; i < PlayerInfo.PlayerTroops.Length; i++)
        {
            Debug.Log("Coded Roster is " + SaveFile.ActiveRoster[i]);
            if (SaveFile.ActiveRoster[i] >= 0)
            {
                PlayerInfo.PlayerTroops[i] = UnitCoder[SaveFile.ActiveRoster[i]];
            }
            else
            {
                PlayerInfo.PlayerTroops[i] = null;
            }
        }
    }

    //Dev tester
    public void CounterPlus() {

        
        PlayerInfo.LevelKeys[counter] = true;
        PlayerInfo.TroopKeys[counter] = true;
        counter++;
        if (counter <= 4) {
            PlayerInfo.TroopSpaces = counter;
            Debug.Log("Counter at " + counter);
        }
        Debug.Log("Counter at " + counter);
    }
}
