using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += RefreshScore;
    }
    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= RefreshScore;
    }

    private void RefreshScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}
