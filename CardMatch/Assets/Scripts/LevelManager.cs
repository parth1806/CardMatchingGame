using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Card cardPrefab;

    private Sprite[] _cardSprites;
    private List<Card> _cards;
    private int[] _shuffledCardsIndex;
    private Card _firstSelectedCard;
    private Card _secondSelectedCard;

    // Start is called before the first frame update
    void Start()
    {
        _cardSprites = Resources.LoadAll<Sprite>("Cards"); // Get all sprites which is in Resources/Cards folder.
        SpawnCards(2, 2);
    }

    private void SpawnCards(int rows, int columns) // Instantiate card prefabs as per the gridSize
    {
        _shuffledCardsIndex = new int[rows * columns];
        for (var i = 0; i < _shuffledCardsIndex.Length; i++)
        {
            _shuffledCardsIndex[i] = i / 2; // Setup pair of cards.
        }

        ShuffleCards();

        _cards = new List<Card>(rows * columns);
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

    IEnumerator CheckCardMatch(Card firstSelection, Card secondSelection)
    {
        _firstSelectedCard = null;
        _secondSelectedCard = null;
        if (firstSelection.CardId == secondSelection.CardId)
        {
            Debug.Log("Card Match");
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Card Not Match");
            firstSelection.ShowBack();
            secondSelection.ShowBack();
        }
    }
}
