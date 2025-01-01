using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using SaveSystem;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Card cardPrefab;

    private Sprite[] _cardSprites;
    private List<Card> _cards;
    private List<Card> _clearedCards;
    private int[] _shuffledCardsIndex;
    private Card _firstSelectedCard;
    private Card _secondSelectedCard;
    private Vector2Int _gridSize;

    public Action<Vector2Int> OnLevelFinished;
    public Action<Vector2Int, List<Card>> OnLevelCreated;
    public Action<Vector2Int, List<Card>> OnCardFlipped;

    private void Awake()
    {
        Instance = this;
        _cardSprites = Resources.LoadAll<Sprite>("Cards"); // Get all sprites which is in Resources/Cards folder.

    }

    private void Start()
    {
        //_cardSprites = Resources.LoadAll<Sprite>("Cards"); // Get all sprites which is in Resources/Cards folder.
    }
    public void StartLevel(Vector2Int gridSize)
    {
        if (!Validate(gridSize))
        {
            return;
        }

        _gridSize = gridSize;
        SpawnCards(_gridSize); // rows, columns
    }

    private bool Validate(Vector2Int gridSize)
    {
        var cellCount = gridSize.x * gridSize.y;
        if (cellCount > 0 && cellCount % 2 != 1)
        {
            return true;
        }
        else
        {
            Debug.LogError("GridSize should be multiple of 2");
        }
        return false;
    }

    private void SpawnCards(Vector2Int gridSize) // Instantiate card prefabs as per the gridSize
    {
        Debug.Log("SaveManager.Instance-> " + SaveManager.Instance);
        var rows = gridSize.x;
        var columns = gridSize.y;
        _shuffledCardsIndex = new int[rows * columns];
        for (var i = 0; i < _shuffledCardsIndex.Length; i++)
        {
            _shuffledCardsIndex[i] = i / 2; // Setup pair of cards.
        }

        List<CardData> savedCardData = null;
        // if we have saved level info, start level from file otherwise create new one
        if (SaveManager.Instance.LoadData(gridSize, out var savedData))
        {
            savedCardData = savedData.cards;
            _shuffledCardsIndex = savedCardData.Select(card => card.cardId).ToArray();
        }
        else
        {
            ShuffleCards();
        }

        // update grid layout
        gridLayoutGroup.constraintCount = rows;

        _cards = new List<Card>(rows * columns);
        _clearedCards = new List<Card>(_cards.Capacity);

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var cell = i * columns + j;
                var card = Instantiate(cardPrefab, gridLayoutGroup.transform);
                var cardId = _shuffledCardsIndex[cell];
                var frontImage = _cardSprites[cardId];
                var isFlipped = savedCardData != null && savedCardData[cell].isFlipped;
                var isMatched = savedCardData != null && savedCardData[cell].isMatched;
                card.Setup(cardId, frontImage,isFlipped);
                card.OnFlipped += CardFlipped;
                _cards.Add(card);

                if (isMatched)
                {
                    _clearedCards.Add(card);
                }
            }
        }

        // Update First selected in case player selected from last session
        FindFirstSelected();

        OnLevelCreated?.Invoke(gridSize, _cards);
    }

    private void FindFirstSelected()
    {
        foreach (var card in _cards)
        {
            if (card.IsFlipped && !_clearedCards.Contains(card))
            {
                _firstSelectedCard = card;
                break;
            }
        }
    }

    private void CardFlipped(Card selectedCard)
    {
        if (selectedCard.IsFlipped)
        {
            return;
        }
        selectedCard.ShowFront();
        OnCardFlipped?.Invoke(_gridSize, _cards);

        if (_firstSelectedCard == null)
        {
            _firstSelectedCard = selectedCard;
        }
        else if (_secondSelectedCard == null)
        {
            _secondSelectedCard = selectedCard;
            StartCoroutine(CheckCardMatch(_firstSelectedCard, _secondSelectedCard));
        }
    }

    private void ShuffleCards() // Randomly setup cards.
    {
        for (var i = 0; i < _shuffledCardsIndex.Length; i++)
        {
            var randomIndex = Random.Range(0, _shuffledCardsIndex.Length);
            (_shuffledCardsIndex[i], _shuffledCardsIndex[randomIndex]) = (_shuffledCardsIndex[randomIndex], _shuffledCardsIndex[i]);
        }
    }

    IEnumerator CheckCardMatch(Card firstSelection, Card secondSelection) // Check is card match or not.
    {
        _firstSelectedCard = null;
        _secondSelectedCard = null;
        if (firstSelection.CardId == secondSelection.CardId)
        {
            Debug.Log("Card Match");
            _clearedCards.Add(firstSelection);
            _clearedCards.Add(secondSelection);

            CheckForLevelFinish();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Card Not Match");
            firstSelection.ShowBack();
            secondSelection.ShowBack();

            OnCardFlipped?.Invoke(_gridSize, _cards);
        }
    }

    private void CheckForLevelFinish()
    {
        if (_clearedCards.Count == _cards.Count)
        {
            _clearedCards.Clear();
            _cards.Clear();

            OnLevelFinished?.Invoke(_gridSize);
        }
    }
}
