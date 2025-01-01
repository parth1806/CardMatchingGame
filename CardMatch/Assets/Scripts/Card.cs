using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardId { get; private set; }

    private Sprite _cardFrontImage;
    private Image _cardImage;

    [SerializeField]
    private Sprite cardBackImage;

    public Action<Card> OnFlipped;

    // Start is called before the first frame update
    void Start()
    {
        _cardImage = GetComponent<Image>();
    }

    public void Setup(int cardId, Sprite frontImage)
    {
        CardId = cardId;
        _cardFrontImage = frontImage;
        gameObject.name = cardId.ToString();
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        OnFlipped?.Invoke(this);
    }

    public void ShowFront()
    {
        _cardImage.sprite = _cardFrontImage;
    }

    public void ShowBack()
    {
        _cardImage.sprite = cardBackImage;
    }
}
