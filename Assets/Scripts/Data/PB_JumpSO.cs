using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump", menuName ="PermanentBoost")]
public class PB_JumpSO : PermanentBoostObject
{
    public float multiplier;
    public float levelOne = 1.1f;
    public float levelTwo = 1.5f;
    public float levelThree = 1.8f;

    public override void GrowLevel()
    {
        if (currentLevel >= maxLevel)
            return;

        currentLevel++;
        RestoreBoostState();
    }

    public override void RestoreBoostState()
    {
        switch (currentLevel)
        {
            case 0:
                multiplier = 1f;
                cost = 10;
                break;
            case 1:
                multiplier = levelOne;
                cost = 10;
                break;
            case 2:
                multiplier = levelTwo;
                cost = 20;
                break;
            case 3:
                multiplier = levelThree;
                cost = 60;
                break;
        }
    }
    public override void PassiveEffect(GameObject playerObj)
    {
        if (playerObj != null)
        {
            var controller = playerObj.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.JumpForce *= multiplier;
            }
        }
    }

}
