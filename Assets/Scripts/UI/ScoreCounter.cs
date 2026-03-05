using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _ultimateScoreText;
    [SerializeField] private UnityEngine.UI.Image _sunBurstImage;

    private int _score;
    private int _ultimateScore;

    public int Score => _score;
    public int UltimateScore => _ultimateScore;

    private void UpdateText(TextMeshProUGUI text, int score)
    {
        text.text = score.ToString();
    }

    public void LoadProgress(int score, int watermelonScore)
    {
        _score = score;
        _ultimateScore = watermelonScore;
        UpdateText(_scoreText, _score);
        UpdateText(_ultimateScoreText, _ultimateScore);

        if(_ultimateScore > 0)
        {
            _sunBurstImage.gameObject.SetActive(true);
        }
    }

    public void AddUltimateScore()
    {
        _ultimateScore++;

        if(_ultimateScore > 0)
        {
            _sunBurstImage.gameObject.SetActive(true);
        }
        UpdateText(_ultimateScoreText, _ultimateScore);

    }

    public void AddScore(int score)
    {
        _score += score;
        UpdateText(_scoreText, _score);
    }
}
