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

    // Start is called before the first frame update
    void Start()
    {
        _cardSprites = Resources.LoadAll<Sprite>("Cards"); // Get all sprites which is in Resources/Cards folder.
        SpawnCards(2, 2);
    }

    private void SpawnCards(int rows, int columns) // Instantiate card prefabs as per the gridSize
    {
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var cell = i * columns + j;
                var card = Instantiate(cardPrefab, gridLayoutGroup.transform);
                var cardId = cell;
                var frontImage = _cardSprites[cardId];
                card.Setup(cardId, frontImage);
                card.OnFlipped += CardFlipped;
            }
        }
    }

    private void CardFlipped(Card selectedCard)
    {
        selectedCard.ShowFront();
    }
}
