using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardId { get; private set; }
    public bool IsFlipped { get; private set; }

    private Sprite _cardFrontImage;
    private Image _cardImage;

    [SerializeField]
    private Sprite cardBackImage;

    public Action<Card> OnFlipped;

    void Awake()
    {
        _cardImage = GetComponent<Image>();
    }

    public void Setup(int cardId, Sprite frontImage, bool isFlipped)
    {
        CardId = cardId;
        _cardFrontImage = frontImage;
        gameObject.name = cardId.ToString();
        GetComponent<Button>().onClick.AddListener(OnClicked);
        if (isFlipped)
        {
            ShowFront();
        }
        else 
        {
            ShowBack();
        }
    }

    private void OnClicked()
    {
        OnFlipped?.Invoke(this);
    }

    public void ShowFront(bool isInit = false)
    {
        _cardImage.sprite = _cardFrontImage;
        if (!isInit)
        {
            IsFlipped = true;
        }
    }

    public void ShowBack(bool isInit = false)
    {
        _cardImage.sprite = cardBackImage;
        if (!isInit)
        {
            IsFlipped = false;
        }
    }
}
