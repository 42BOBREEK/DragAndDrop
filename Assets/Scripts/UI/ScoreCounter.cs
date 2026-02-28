using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _waterMelonText;
    [SerializeField] private UnityEngine.UI.Image _sunBurstImage;

    private int _score;
    private int _watermelonScore;

    public int Score => _score;
    public int WatermelonScore => _watermelonScore;

    private void UpdateText(TextMeshProUGUI text, int score)
    {
        text.text = score.ToString();
    }

    public void LoadProgress(int score, int watermelonScore)
    {
        _score = score;
        _watermelonScore = watermelonScore;
        UpdateText(_scoreText, _score);
        UpdateText(_waterMelonText, _watermelonScore);

        if(_watermelonScore > 0)
        {
            _sunBurstImage.gameObject.SetActive(true);
        }
    }

    public void AddWatermelonScore()
    {
        _watermelonScore++;

        if(_watermelonScore > 0)
        {
            _sunBurstImage.gameObject.SetActive(true);
        }
        UpdateText(_waterMelonText, _watermelonScore);

    }

    public void AddScore(int score)
    {
        _score += score;
        UpdateText(_scoreText, _score);
    }
}
