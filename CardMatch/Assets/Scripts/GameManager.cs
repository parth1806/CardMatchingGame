using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gridRows;
    public int gridColumns;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.Instance.OnLevelFinished += OnLevelFinished;
        LevelManager.Instance.StartLevel(new Vector2Int(gridRows, gridColumns));
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnLevelFinished -= OnLevelFinished;
    }

    private void OnLevelFinished(Vector2Int vector2Int)
    {
        Debug.Log("Level Completed");
    }
}
