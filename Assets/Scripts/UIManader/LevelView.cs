using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Button button;

    [Space]
    [SerializeField] private List<GameObject> starIcons;

    private void OnEnable ()
    {
        button.onClick.AddListener (OnClick);
    }

    private void OnDisable ()
    {
        button.onClick.RemoveListener (OnClick);
    }

    private void OnClick ()
    {
        onClick?.Invoke (number);
    }

    private int number;
    private int stars;
    private Action<int> onClick;

    public void Reinit (int number, int stars, Action<int> onClick)
    {
        this.number = number;
        this.stars = stars;
        this.onClick = onClick;

        numberText.text = $"Level {this.number}";

        for (int i = 0; i < starIcons.Count; i++)
            starIcons[i].SetActive (i < this.stars);

        button.interactable = this.stars >= 0;
    }
}