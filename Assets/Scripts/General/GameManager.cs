using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public bool singlePlayer, localCoop;

    void Awake() {
        if (_instance == null)
            _instance = this;
    }

    public void SetGameState(int numPlayers) {
        if (numPlayers == 1) {
            singlePlayer = true;
        } else if (numPlayers == 2) {
            localCoop = true;
        }
    }

    public int GetGameState() {
        if (singlePlayer) 
            return 1;
        else if (localCoop)
            return 2;
        
        return 1;
    }
}
