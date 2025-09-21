using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<int> highScores = new List<int>();
    public int sceneIndex;
    public float playerX, playerY, playerZ;
    public int score;
    public int coins;
    public float musicVolume;
    public float sfxVolume;
    public List<bool> collectables = new List<bool>();
    public List<BoostSaveData> boosts = new List<BoostSaveData>();
}

[System.Serializable]
public class BoostSaveData
{
    public string boostName;
    public int currentLevel;
}
