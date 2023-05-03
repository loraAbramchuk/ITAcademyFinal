using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;

    private int currentLevelNumber;
    private GameObject currentLevel;

    private void OnEnable ()
    {
        Target.TargetReachedEvent += Target_OnTargetReached;
        Killer.PlayerKilledEvent += Killer_OnPlayerKilled;
    }

    private void OnDisable ()
    {
        Target.TargetReachedEvent -= Target_OnTargetReached;
        Killer.PlayerKilledEvent += Killer_OnPlayerKilled;
    }

    public void Start ()
    {
        SelectLevel ();
    }

    private void Target_OnTargetReached (int starsCounter)
    {
        LevelSaveData currentLevelSaveData;

        if (SaveManager.SaveData.CompletedLevels.Count > currentLevelNumber - 1)
        {
            currentLevelSaveData = SaveManager.SaveData.CompletedLevels[currentLevelNumber - 1];
        }
        else
        {
            currentLevelSaveData = new LevelSaveData ();
            SaveManager.SaveData.CompletedLevels.Add (currentLevelSaveData);
        }

        currentLevelSaveData.Stars = starsCounter;

        UIManager.Instance.ShowLevelFinalScreen (SelectLevel, RestartLevel, NextLevel);
    }

    private void Killer_OnPlayerKilled ()
    {
        UIManager.Instance.ShowLevelFinalScreen (SelectLevel, RestartLevel, null);
    }

    private void SelectLevel ()
    {
        UIManager.Instance.HideLevelFinalScreen ();
        UIManager.Instance.ShowLevelSelectionScreen (levelNumber =>
        {
            UIManager.Instance.HideLevelSelectionScreen ();

            currentLevelNumber = levelNumber;
            LoadLevel (currentLevelNumber);

            if (currentLevel == false)
            {
                currentLevelNumber = Mathf.Max (1, SaveManager.SaveData.CompletedLevels.Count);
                LoadLevel (currentLevelNumber);
            }
        });
    }

    private void RestartLevel ()
    {
        UIManager.Instance.HideLevelFinalScreen ();
        LoadLevel (currentLevelNumber);
    }

    private void NextLevel ()
    {
        UIManager.Instance.HideLevelFinalScreen ();

        currentLevelNumber++;
        LoadLevel (currentLevelNumber);

        if (currentLevel == false)
        {
            currentLevelNumber--;
            LoadLevel (currentLevelNumber);
        }
    }

    private void LoadLevel (int number)
    {
        Destroy (currentLevel);
        currentLevel = null;

        GameObject level = Resources.Load<GameObject> ($"Level_{number}");

        if (level)
            currentLevel = Instantiate (level, rootObject.transform);
        else
            Debug.LogError ("Нет такого уровня!");
    }
}