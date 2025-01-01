using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveSystem
{

    [Serializable]
    public class CardData
    {
        public int cardId;
        public bool isFlipped;
        public bool isMatched;

        public CardData(int cardId, bool isFlipped, bool isMatched)
        {
            this.cardId = cardId;
            this.isFlipped = isFlipped;
            this.isMatched = isMatched;
        }

    }

    [Serializable]
    public class SaveData
    {
        public Vector2Int gridSize;
        public List<CardData> cards;
    }
}