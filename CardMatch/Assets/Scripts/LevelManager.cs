using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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


    private void Awake()
    {
        Instance = this;
        _cardSprites = Resources.LoadAll<Sprite>("Cards"); // Get all sprites which is in Resources/Cards folder.
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
        var rows = gridSize.x;
        var columns = gridSize.y;
        _shuffledCardsIndex = new int[rows * columns];
        for (var i = 0; i < _shuffledCardsIndex.Length; i++)
        {
            _shuffledCardsIndex[i] = i / 2; // Setup pair of cards.
        }

        ShuffleCards();

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
                card.Setup(cardId, frontImage);
                card.OnFlipped += CardFlipped;
                _cards.Add(card);
            }
        }
    }

    private void CardFlipped(Card selectedCard)
    {
        selectedCard.ShowFront();
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
