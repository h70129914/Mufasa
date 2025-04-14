using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UTool.TabSystem;
using UnityEngine.Localization.Settings;

[HasTabField]
public class SoundRecorder : MonoBehaviour
{
    //[TabField]
    [Range(0f, 1f)]
    public float maxExpectedLoudness = 0.3f;
    //[TabField]
    public int SampleDuration = 5;

    [SerializeField] private RectRotationController volumeBarAr;
    [SerializeField] private RectRotationController volumeBarEn;
    [SerializeField] private UIFlowController mainWindowControllerAr;
    [SerializeField] private UIFlowController mainWindowControllerEn;
    [SerializeField] private UIFlowController secondWindowController;
    [SerializeField] private Button recordButton;
    [SerializeField] private LeaderboardDisplay leaderboardAr;
    [SerializeField] private LeaderboardDisplay leaderboardEn;

    private string microphone;
    private AudioClip recordedClip;

    void Start()
    {
        microphone = Microphone.devices.FirstOrDefault();
    }

    public void StartRecording()
    {
        if (microphone == null) return;

        //statusText.text = "Recording...";
        recordedClip = Microphone.Start(microphone, false, SampleDuration, 44100);
        StartCoroutine(StopAfterDelay());
    }

    private IEnumerator StopAfterDelay()
    {
        float startTime = Time.time;
        while (Time.time - startTime < SampleDuration)
        {
            UpdateLoudnessVisual();
            yield return null;
        }

        Microphone.End(microphone);
        //statusText.text = "Processing...";
        int loudness = CalculateLoudness(recordedClip);
        GameManager.Instance.UpdateScore(loudness);

        mainWindowControllerAr.ShowNext();
        mainWindowControllerEn.ShowNext();
        secondWindowController.ShowNext();

        bool isWin = loudness >= 50;
        var selectedLocale = LocalizationSettings.SelectedLocale;
        if (selectedLocale != null && selectedLocale.Identifier.Code == "ar")
        {
            leaderboardAr.GameFinish(isWin);
        }
        else
        {
            leaderboardEn.GameFinish(isWin);
        }
    }

    void UpdateLoudnessVisual()
    {
        int micPosition = Microphone.GetPosition(null) - 1024;
        if (micPosition < 0) return;

        float[] waveData = new float[1024];
        recordedClip.GetData(waveData, micPosition);

        float levelMax = waveData.Average(Mathf.Abs);

        // Normalize the levelMax based on maxExpectedLoudness
        float normalized = Mathf.Clamp01(levelMax);
        Debug.Log($"level max: {levelMax}, normalized is {normalized}");

        // Update the volume bar with the normalized value
        volumeBarAr.UpdateValue(normalized);
        volumeBarEn.UpdateValue(normalized);
    }

    int CalculateLoudness(AudioClip clip)
    {
        float[] data = new float[clip.samples * clip.channels];
        clip.GetData(data, 0);

        double sum = 0f;

        for (int i = 0; i < data.Length; i++)
        {
            sum += data[i] * data[i];
        }

        double rms = Mathf.Sqrt((float)(sum / data.Length));
        int loudness = Mathf.RoundToInt((float)(rms * 1000));

        return loudness;
    }
}
