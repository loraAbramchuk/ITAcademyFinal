using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_KEY = "user_save";

    public static SaveData SaveData { get; private set; }

    [SerializeField] private bool clearSave;

    private void Awake ()
    {
        DontDestroyOnLoad (gameObject);

        if (clearSave)
            PlayerPrefs.DeleteAll ();

        Load ();
    }

    private void Start ()
    {
        SceneManager.LoadScene (1);
    }

    private void OnApplicationFocus (bool hasFocus)
    {
        if (hasFocus == false)
            Save ();
    }

    private void OnApplicationQuit ()
    {
        Save ();
    }

    private static void Load ()
    {
        string saveValue = PlayerPrefs.HasKey (SAVE_KEY)
            ? PlayerPrefs.GetString (SAVE_KEY)
            : string.Empty;

        SaveData = string.IsNullOrWhiteSpace (saveValue) == false
            ? JsonUtility.FromJson<SaveData> (saveValue)
            : new SaveData ();
    }

    private static void Save ()
    {
        string saveValue = SaveData != null
            ? JsonUtility.ToJson (SaveData)
            : string.Empty;

        PlayerPrefs.SetString (SAVE_KEY, saveValue);
    }
}