using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private GameObject highscoreInputPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text currentTimeText;

    private List<Transform> highscoreEntryTransformList;
    private int pendingScore;
    private float pendingTime;

    private void Awake()
    {
        if (entryContainer == null)
            entryContainer = transform.Find("EntryNameScore");

        if (entryTemplate == null && entryContainer != null)
            entryTemplate = entryContainer.Find("text"); 

        if (entryTemplate != null)
            entryTemplate.gameObject.SetActive(false);

        RefreshHighscoreTable();
    }

    public void RefreshHighscoreTable()
    {
        // Clear existing entries
        if (highscoreEntryTransformList != null)
        {
            foreach (Transform entry in highscoreEntryTransformList)
            {
                if (entry != null && entry != entryTemplate)
                    Destroy(entry.gameObject);
            }
        }

        // Load highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null || highscores.highscoreEntryList == null || highscores.highscoreEntryList.Count == 0)
        {
            // Initialize with empty table if no scores exist
            Debug.Log("Initializing empty highscore table...");
            highscores = new Highscores();
            highscores.highscoreEntryList = new List<HighscoreEntry>();

            // Save empty table
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
            return;
        }

        highscores.highscoreEntryList.Sort((a, b) => {
            if (a.score != b.score)
                return b.score.CompareTo(a.score); // Higher score first
            else
                return a.time.CompareTo(b.time);   // Lower time first
        });

        // Create visual entries
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;

        // Find Name text
        Text nameText = entryTransform.Find("Name")?.GetComponent<Text>();
        if (nameText != null)
            nameText.text = highscoreEntry.name;

        // Find Score text  
        Text scoreText = entryTransform.Find("Score")?.GetComponent<Text>();
        if (scoreText != null)
            scoreText.text = highscoreEntry.score.ToString("N0");

        // Find Time text
        Text timeText = entryTransform.Find("Time")?.GetComponent<Text>();
        if (timeText != null)
            timeText.text = FormatTime(highscoreEntry.time);

        // Optional: Add rank if you want to show position
        Text rankText = entryTransform.Find("Rank")?.GetComponent<Text>();
        if (rankText != null)
        {
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "TH";
                    break;
                case 1:
                    rankString = "1ST";
                    break;
                case 2:
                    rankString = "2ND";
                    break;
                case 3:
                    rankString = "3RD";
                    break;
            }
            rankText.text = rankString;
        }

        transformList.Add(entryTransform);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100) % 100);

        if (minutes > 0)
            return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
        else
            return $"{seconds:00}.{milliseconds:00}s";
    }

    public void AddHighscoreEntry(string name, float time, int score)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry
        {
            score = score,
            time = time,
            name = name
        };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores();
            highscores.highscoreEntryList = new List<HighscoreEntry>();
        }

        // Add new entry
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Sort by score (highest first), then time (lowest first for ties)
        highscores.highscoreEntryList.Sort((a, b) => {
            if (a.score != b.score)
                return b.score.CompareTo(a.score);
            else
                return a.time.CompareTo(b.time);
        });

        // Keep only top 10 scores
        if (highscores.highscoreEntryList.Count > 10)
        {
            highscores.highscoreEntryList.RemoveRange(10, highscores.highscoreEntryList.Count - 10);
        }

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

        Debug.Log($"Highscore saved! Name: {name}, Score: {score}, Time: {time:F2}s");

        RefreshHighscoreTable();
    }

    public bool IsHighScore(int score)
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null || highscores.highscoreEntryList == null || highscores.highscoreEntryList.Count < 10)
            return true;

        // Find the lowest score in the top 10
        int lowestScore = int.MaxValue;
        foreach (var entry in highscores.highscoreEntryList)
        {
            if (entry.score < lowestScore)
                lowestScore = entry.score;
        }

        return score > lowestScore;
    }

    public void ShowHighScoreInput(int score, float time)
    {
        pendingScore = score;
        pendingTime = time;

        if (highscoreInputPanel != null)
        {
            if (currentScoreText != null)
                currentScoreText.text = $"Score: {score:N0}";
            if (currentTimeText != null)
                currentTimeText.text = $"Time: {FormatTime(time)}";

            highscoreInputPanel.SetActive(true);

            if (nameInputField != null)
            {
                nameInputField.text = "";
                nameInputField.Select();
            }
        }
        else
        {
            // If no input panel, use default name
            SubmitHighScore("Player");
        }
    }

    public void SubmitHighScore()
    {
        if (nameInputField != null)
        {
            string playerName = string.IsNullOrEmpty(nameInputField.text) ? "Player" : nameInputField.text;
            SubmitHighScore(playerName);
        }
        else
        {
            SubmitHighScore("Player");
        }
    }

    public void SubmitHighScore(string playerName)
    {
        AddHighscoreEntry(playerName, pendingTime, pendingScore);

        if (highscoreInputPanel != null)
            highscoreInputPanel.SetActive(false);
    }

    public void ClearAllHighScores()
    {
        PlayerPrefs.DeleteKey("highscoreTable");
        PlayerPrefs.Save();
        RefreshHighscoreTable();
        Debug.Log("All high scores cleared");
    }

    [System.Serializable]
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public float time;
        public string name;
    }
}
