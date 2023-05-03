using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionScreen : MonoBehaviour
{
    private const int TOTAL_LEVELS_COUNT = 10;

    [SerializeField] private Transform content;
    [SerializeField] private LevelView levelViewAsset;

    private Action<int> onClick;

    public void Show (Action<int> onClick)
    {
        this.onClick = onClick;

        gameObject.SetActive (true);
        Reconstruct ();
    }

    public void Hide ()
    {
        gameObject.SetActive (false);
    }

    private void Reconstruct ()
    {
        List<LevelView> existingViews = new List<LevelView> (content.GetComponentsInChildren<LevelView> (true));
        List<LevelSaveData> completedLevels = SaveManager.SaveData.CompletedLevels;

        bool lastAvailableLevelIsReady = false;
        for (int i = 0; i < TOTAL_LEVELS_COUNT; i++)
        {
            LevelView levelView;

            if (existingViews.Count > i)
                levelView = existingViews[i];

            else
            {
                levelView = Instantiate (levelViewAsset, content);
                existingViews.Add (levelView);
            }

            LevelSaveData levelSaveData = completedLevels.Count > i ? completedLevels[i] : null;

            int levelViewStars = -1;

            if (levelSaveData != null)
                levelViewStars = levelSaveData.Stars;

            else if (lastAvailableLevelIsReady == false)
            {
                lastAvailableLevelIsReady = true;
                levelViewStars = 0;
            }

            levelView.gameObject.SetActive (true);
            levelView.Reinit (i + 1, levelViewStars, OnClick);

            if (i == TOTAL_LEVELS_COUNT - 1)
            {
                for (int j = TOTAL_LEVELS_COUNT; j < existingViews.Count; j++)
                {
                    existingViews[j].gameObject.SetActive (false);
                }
            }
        }
    }

    private void OnClick (int number)
    {
        onClick?.Invoke (number);
    }
}