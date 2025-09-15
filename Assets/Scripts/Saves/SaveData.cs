using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
   public List<int>highScores = new List<int>();
   public int sceneIndex;
   public float playerX, playerY, playerZ;
   public int score;
   public int coins;
}
