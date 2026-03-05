using UnityEngine;

public class ProgressSaver : MonoBehaviour
{
    [SerializeField] private MergeableObjectDeleter _deleter;
    [SerializeField] private CopyPaster _copyPaster;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private MusicManager _musicManager;

    [SerializeField] private readonly string _scoreKey = "Score";
    [SerializeField] private readonly string _ultimateScoreKey = "UltimateScore";
    [SerializeField] private readonly string _copyPasterChargesKey = "CopyPasterCharges";
    [SerializeField] private readonly string _deleterChargesKey = "DeleterCharges";
    [SerializeField] private readonly string _musicStateKey = "MusicState";

    private void Start()
    {
        LoadProgress();
    }

    private void LoadProgress()
    {
        int savedScore = PlayerPrefs.GetInt(_scoreKey, 0);
        int savedUltimateScore = PlayerPrefs.GetInt(_ultimateScoreKey, 0);
        int savedCopyPasterCharges = PlayerPrefs.GetInt(_copyPasterChargesKey, 0);
        int savedDeleterCharges = PlayerPrefs.GetInt(_deleterChargesKey, 0);
        bool savedMusicState = PlayerPrefs.GetInt(_musicStateKey, 1) == 1;

        if(_scoreCounter != null)  
            _scoreCounter.LoadProgress(savedScore, savedUltimateScore);

        if(_musicManager != null)
            _musicManager.SetMusicState(savedMusicState);

        if(_copyPaster != null)
            _copyPaster.SetCharges(savedCopyPasterCharges);

        if(_deleter != null)
            _deleter.SetCharges(savedDeleterCharges);
    }

    public void SaveMusicState(bool isOn)
    {
        PlayerPrefs.SetInt(_musicStateKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt(_scoreKey, score);
        PlayerPrefs.Save();
    }

    public void SaveUltimateScore(int ultimateScore)
    {
        PlayerPrefs.SetInt(_ultimateScoreKey, ultimateScore);
        PlayerPrefs.Save();
    }

    public void SaveCopyPasterCharges(int charges)
    {
        PlayerPrefs.SetInt(_copyPasterChargesKey, charges);
        PlayerPrefs.Save();
    }

    public void SaveDeleterCharges(int charges)
    {
        PlayerPrefs.SetInt(_deleterChargesKey, charges);
        PlayerPrefs.Save();
    }
}