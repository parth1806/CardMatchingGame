using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            LevelManager.Instance.OnLevelCreated += SaveLevel;
            LevelManager.Instance.OnLevelFinished += OnLevelFinished;
            LevelManager.Instance.OnCardFlipped += SaveLevel;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.OnLevelCreated -= SaveLevel;
            LevelManager.Instance.OnLevelFinished -= OnLevelFinished;
            LevelManager.Instance.OnCardFlipped -= SaveLevel;
        }

        private void SaveLevel(Vector2Int gridSize, List<Card> cards)
        {
            var cardData = from card in cards
                select new CardData(card.CardId, card.IsFlipped, cards.Count(c => c.IsFlipped && c.CardId == card.CardId) >= 2);

            // TODO save level
            var data = new SaveData()
            {
                gridSize = gridSize,
                cards = cardData.ToList()
            };

            Debug.Log("data:" + data);
            SaveData(data);
        }

        private string GetPath(Vector2Int gridSize)
        {
            return Path.Combine(Application.persistentDataPath, gridSize.ToString());
        }

        private void SaveData(SaveData data)
        {
            var savePath = GetPath(data.gridSize);
            var jsonString = JsonUtility.ToJson(data);
            File.WriteAllText(savePath, jsonString);
        }

        public bool LoadData(Vector2Int gridSize, out SaveData loadedData)
        {
            var loadPath = GetPath(gridSize);

            var loadFileInfo = new FileInfo(loadPath);
            if (loadFileInfo.Exists)
            {
                var savedJson = File.ReadAllText(loadPath);
                loadedData = JsonUtility.FromJson<SaveData>(savedJson);
                Debug.Log("loaded data" + loadedData);
                return true;
            }

            loadedData = null;
            return false;
        }

        private void OnLevelFinished(Vector2Int gridSize)
        {
            // TODO level finished
            var loadPath = GetPath(gridSize);
            var loadFileInfo = new FileInfo(loadPath);
            if (loadFileInfo.Exists)
            {
                loadFileInfo.Delete();
            }
        }
    }
}