using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private bool _active;

    private void ChangeLocale(int localeID)
    {
        if (_active) return;
        StartCoroutine(SetLocale(localeID));
    }

    public IEnumerator SetLocale(int localeID)
    {
        _active = true;
        PlayerPrefs.Save();
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        _active = false;
    }
}