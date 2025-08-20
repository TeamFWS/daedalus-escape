using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoreButtonManager : MonoBehaviour
{
    [SerializeField] public Button[] loreButtons;
    [SerializeField] public string[] collectibleIDs;
    private readonly string _lockedText = "???";

    private void Start()
    {
        if (loreButtons.Length != collectibleIDs.Length)
        {
            Debug.LogError("Mismatch between lore buttons and collectible IDs!");
            return;
        }

        for (var i = 0; i < loreButtons.Length; i++) UpdateButtonState(loreButtons[i], collectibleIDs[i]);
    }

    private void UpdateButtonState(Button button, string collectibleID)
    {
        var isUnlocked = PlayerPrefs.GetInt(collectibleID, 0) == 1;
        button.interactable = isUnlocked;

        var localizeStringEvent = button.GetComponentInChildren<LocalizeStringEvent>();
        var buttonText = button.GetComponentInChildren<TMP_Text>();
        if ((buttonText != null) & (isUnlocked == false))
        {
            localizeStringEvent.enabled = false;
            buttonText.text = _lockedText;
        }
    }

    public void ReloadMenu()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}