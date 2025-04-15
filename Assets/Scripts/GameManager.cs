using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Dictionary<string, int> userScores = new();
    public string currentPlayerName { get; set; }

    // Define the ScoreUpdated event
    public static event Action OnScoreUpdated;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPlayer(string playerName)
    {
        currentPlayerName = playerName;

        if (!userScores.ContainsKey(playerName))
        {
            userScores.Add(playerName, 0);
        }
    }

    public void UpdateScore(int loudness)
    {
        if (string.IsNullOrEmpty(currentPlayerName)) return;

        userScores[currentPlayerName] = loudness;

        // Fire the ScoreUpdated event
        OnScoreUpdated?.Invoke();
    }
}
