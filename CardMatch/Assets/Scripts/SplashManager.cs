using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClickLevel(string gridSize)
    {
        PlayerPrefs.SetString("Level", gridSize);
        SceneManager.LoadScene(1);
    }
}
