using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    [SerializeField] private List<LevelSaveData> completedLevels = new List<LevelSaveData> ();
    public List<LevelSaveData> CompletedLevels => completedLevels;
}

[Serializable]
public class LevelSaveData
{
    [SerializeField] private int stars;
    public int Stars
    {
        get => stars;
        set
        {
            if (stars < value)
                stars = value;
        }
    }
}