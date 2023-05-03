using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinalScreen : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;

    private void Awake ()
    {
        selectButton.onClick.AddListener (SelectLevel);
        restartButton.onClick.AddListener (RestartLevel);
        nextButton.onClick.AddListener (NextLevel);
    }

    private Action onSelect;
    private Action onRestart;
    private Action onNext;

    public void Show (Action onSelect, Action onRestart, Action onNext)
    {
        this.onSelect = onSelect;
        this.onRestart = onRestart;
        this.onNext = onNext;

        selectButton.gameObject.SetActive (this.onSelect != null);
        restartButton.gameObject.SetActive (this.onRestart != null);
        nextButton.gameObject.SetActive (this.onNext != null);

        gameObject.SetActive (true);
    }

    public void Hide ()
    {
        gameObject.SetActive (false);
    }

    private void SelectLevel ()
    {
        onSelect?.Invoke ();
    }

    private void RestartLevel ()
    {
        onRestart?.Invoke ();
    }

    private void NextLevel ()
    {
        onNext?.Invoke ();
    }
}