using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Points")]
    [SerializeField] private int _currentScore = 0;
    public int currentScore => _currentScore;

    public static Action<int> OnScoreChanged = delegate { };

    private void Awake()
    {
        #region Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        #endregion
    }
    private void Start()
    {
        OnScoreChanged?.Invoke(_currentScore);
    }
    public void AddPoints(int points)
    {
        _currentScore += points;

        OnScoreChanged?.Invoke(_currentScore);
    }
    public void RemovePoints(int points)
    {
        _currentScore -= points;
        if(_currentScore < 0) _currentScore = 0;

        OnScoreChanged?.Invoke(_currentScore);
    }

    public void ResetScore()
    {
        _currentScore = 0;

        OnScoreChanged?.Invoke(_currentScore);
    }
}
