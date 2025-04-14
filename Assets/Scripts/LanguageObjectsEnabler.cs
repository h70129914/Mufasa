using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageObjectsEnabler : MonoBehaviour
{
    public List<GameObject> objects;
    public string enableFor;

    void Start()
    {
        var selectedLocale = LocalizationSettings.SelectedLocale;
        bool enableForLanguage = selectedLocale != null && selectedLocale.Identifier.Code == enableFor;

        foreach (GameObject obj in objects)
        {
            obj.SetActive(enableForLanguage);
        }
    }
}
