using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public int gridRows;
    public int gridColumns;

    // Start is called before the first frame update
    void Start()
    {
        ShowPausePanel(false);
        LevelManager.Instance.OnLevelCreated += OnLevelCreated;
        LevelManager.Instance.OnLevelFinished += OnLevelFinished;
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
    }

    private void OnLevelFinished(Vector2Int vector2Int)
    {
        // TODO level finished
        Debug.Log("Level Completed");
    }
    private void OnLevelCreated(Vector2Int vector2Int, List<Card> cards)
    {
        // TODO level finished constructing

        // TODO start timer etc etc

        // TODO start counting how many moves player did to complete the level
    }

    public void ShowPausePanel(bool isActive)
    {
        pausePanel.SetActive(isActive);
    }

    public void OnClickRestart()
    {
        ShowPausePanel(false);
    }

    public void OnClickHome()
    {
        SceneManager.LoadScene(0);
    }
}
