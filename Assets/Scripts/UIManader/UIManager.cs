using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private LevelSelectionScreen levelSelectionScreen;
    [SerializeField] private LevelFinalScreen levelFinalScreen;

    private void Awake ()
    {
        DontDestroyOnLoad (gameObject);
        Instance = this;
    }

    public void ShowLevelSelectionScreen (Action<int> onClick)
    {
        levelSelectionScreen.Show (onClick);
    }
    public void HideLevelSelectionScreen ()
    {
        levelSelectionScreen.Hide ();
    }

    public void ShowLevelFinalScreen (Action onSelect, Action onRestart, Action onNext)
    {
        levelFinalScreen.Show (onSelect, onRestart, onNext);
    }
    public void HideLevelFinalScreen ()
    {
        levelFinalScreen.Hide ();
    }
}