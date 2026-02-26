using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _waterMelonText;

    private int _score;
    private int _watermelonScore;

    public void AddWatermelonScore()
    {
        _watermelonScore++;

        _waterMelonText.text = _watermelonScore.ToString();
    }

    public void AddScore(int score)
    {
        _score += score;
        _text.text = _score.ToString();
    }
}
