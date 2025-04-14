using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    public GameObject MainScreenAr;
    public GameObject MainScreenEn;

    void Start()
    {
        SetLanguage(LocalizationSettings.SelectedLocale.Identifier.Code);
    }

    public void SelectLanguage(string languageCode)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == languageCode);
        SetLanguage(languageCode);
    }

    private void SetLanguage(string languageCode)
    {
        if (languageCode == "ar")
        {
            MainScreenAr.SetActive(true);
            MainScreenEn.SetActive(false);
        }
        else if (languageCode == "en")
        {
            MainScreenAr.SetActive(false);
            MainScreenEn.SetActive(true);
        }
    }
}
