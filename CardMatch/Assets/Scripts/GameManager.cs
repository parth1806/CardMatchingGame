using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject levelCompletedPanel;
    public Text scoreText;
    public Text scoreComboText;

    public int gridRows;
    public int gridColumns;

    private int _levelScore;

    // Start is called before the first frame update
    void Start()
    {
        levelCompletedPanel.SetActive(false);
        ShowPausePanel(false);
        LevelManager.Instance.OnLevelCreated += OnLevelCreated;
        LevelManager.Instance.OnLevelFinished += OnLevelFinished;
        LevelManager.Instance.OnLevelRestart += OnLevelRestart;
        LevelManager.Instance.OnScoreAdd += OnScoreAdd;
        LevelManager.Instance.OnScoreCombo += OnShowScoreCombo;

        ShowScore();
        string level = PlayerPrefs.GetString("Level");
        LoadLevel(level);
    }

    public void LoadLevel(string gridSizeStr)
    {
        var gridSize = gridSizeStr.Split('x');
        gridRows = int.Parse(gridSize[0]);
        gridColumns = int.Parse(gridSize[1]);
        LevelManager.Instance.StartLevel(new Vector2Int(gridRows, gridColumns));
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnLevelCreated -= OnLevelCreated;
        LevelManager.Instance.OnLevelFinished -= OnLevelFinished;
        LevelManager.Instance.OnScoreAdd -= OnScoreAdd;
        LevelManager.Instance.OnScoreCombo -= OnShowScoreCombo;
    }

    private void OnLevelFinished(Vector2Int vector2Int)
    {
        // TODO level finished
        Debug.Log("Level Completed");
        StartCoroutine(ShowLevelCompletePanel());
    }
    private void OnLevelCreated(Vector2Int vector2Int, List<Card> cards)
    {
        // TODO level finished constructing

        // TODO start timer etc etc

        // TODO start counting how many moves player did to complete the level
    }
    private void OnLevelRestart(Vector2Int vector2Int)
    {
        // TODO level restart
    }

    private void OnScoreAdd(int score)
    {
        _levelScore += score;
        ShowScore();
    }

    private void ShowScore()
    {
        scoreText.text = "Score: " + _levelScore;
    }

    private void OnShowScoreCombo(int combo)
    {
        scoreComboText.text = "Combo: "+ combo + " X";
    }

    public void ShowPausePanel(bool isActive)
    {
        pausePanel.SetActive(isActive);
    }

    IEnumerator ShowLevelCompletePanel()
    {
        yield return new WaitForSeconds(1);
        levelCompletedPanel.SetActive(true);
    }

    public void OnClickRestart()
    {
        ShowPausePanel(false);
        LevelManager.Instance.ShowAllCardsOnRestart();
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene(0);
    }
}
