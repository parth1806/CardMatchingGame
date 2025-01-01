using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource cardFlip;
    public AudioSource cardMatch;
    public AudioSource cardNotMatch;
    public AudioSource gameComplete;

    private void Awake()
    {
        Instance = this;
    }

    public void CardflipSfx()
    {
        cardFlip.Play();
    }

    public void CardMatchSfx()
    {
        cardMatch.Play();
    }

    public void CardNotMatchSfx()
    {
        cardNotMatch.Play();
    }

    public void GameCompleteSfx()
    {
        gameComplete.Play();
    }
}
