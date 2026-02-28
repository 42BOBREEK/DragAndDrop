using UnityEngine;

public class ProgressSaver : MonoBehaviour
{
    [SerializeField] private FruitsDeleter _deleter;
    [SerializeField] private CopyPaster _copyPaster;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private MusicManager _musicManager;

    private void Start()
    {
        LoadProgress();
    }

    private void LoadProgress()
    {
        int savedScore = PlayerPrefs.GetInt("Score", 0);
        int savedWatermelonScore = PlayerPrefs.GetInt("WatermelonScore", 0);
        int savedCopyPasterCharges = PlayerPrefs.GetInt("CopyPasterCharges", 0);
        int savedDeleterCharges = PlayerPrefs.GetInt("DeleterCharges", 0);
        bool savedMusicState = PlayerPrefs.GetInt("MusicState", 1) == 1;

        if(_scoreCounter != null)  
            _scoreCounter.LoadProgress(savedScore, savedWatermelonScore);

        if(_musicManager != null)
            _musicManager.SetMusicState(savedMusicState);

        if(_copyPaster != null)
            _copyPaster.SetCharges(savedCopyPasterCharges);

        if(_deleter != null)
            _deleter.SetCharges(savedDeleterCharges);
    }

    public void SaveMusicState(bool isOn)
    {
        PlayerPrefs.SetInt("MusicState", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    public void SaveWatermelonScore(int watermelonScore)
    {
        PlayerPrefs.SetInt("WatermelonScore", watermelonScore);
        PlayerPrefs.Save();
    }

    public void SaveCopyPasterCharges(int charges)
    {
        PlayerPrefs.SetInt("CopyPasterCharges", charges);
        PlayerPrefs.Save();
    }

    public void SaveDeleterCharges(int charges)
    {
        PlayerPrefs.SetInt("DeleterCharges", charges);
        PlayerPrefs.Save();
    }
}