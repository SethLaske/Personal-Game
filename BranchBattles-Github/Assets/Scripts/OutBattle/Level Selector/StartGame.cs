using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Unit initMiner;
    public Unit initFighter;
    public Unit initSpear;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = -1; i < 11; i++) {
            PlayerInfo.LevelKeys.Add(i, false);
            PlayerInfo.TroopKeys.Add(i, false);
        }
        PlayerInfo.TroopSpaces = 0;

        //PlayerPrefs.SetInt("CompletedLevels", -1);
        //PlayerPrefs.SetInt("UnlockedTroops", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame(string LevelSelect) {
        SceneManager.LoadScene(LevelSelect);
        //PlayerInfo.PlayerTroops[0] = initMiner;
        //PlayerInfo.PlayerTroops[1] = initFighter;
        //PlayerInfo.PlayerTroops[2] = initSpear;
    }
}
