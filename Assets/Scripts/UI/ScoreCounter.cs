using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private int _score;

    public void ChangeScore(int score)
    {
        _score += score;
        _text.text = _score.ToString();
    }
}
