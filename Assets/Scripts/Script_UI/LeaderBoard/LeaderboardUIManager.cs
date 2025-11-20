using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataStructure;
using ScriptableObjects.GameEvents;

namespace UI.LeaderBoard
{
    public class LeaderboardUIManager : MonoBehaviour
    {
        [SerializeField] private NullEvent OnDataLoaded;
        [SerializeField] private RectTransform content; 
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int visibleCount = 20; 
        [SerializeField] private float itemHeight = 80f;

        private List<GameObject> itemPool = new List<GameObject>();
        private int totalCount;

        private void OnEnable()
        {
            OnDataLoaded.OnEventRaised += LoadUIFromData;
        }

        private void OnDisable()
        {
            OnDataLoaded.OnEventRaised -= AutoScrollToUser;
            scrollRect.onValueChanged.RemoveAllListeners();
        }

        private void LoadUIFromData()
        {

            foreach (var go in itemPool)
                Destroy(go);
            itemPool.Clear();

            totalCount = Data.LeaderboardData.Instance.players.Count;


            content.sizeDelta = new Vector2(content.sizeDelta.x, totalCount * itemHeight);


            for (int i = 0; i < visibleCount; i++)
            {
                GameObject go = Instantiate(itemPrefab, content);
                itemPool.Add(go);
            }

            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);


            AutoScrollToUser();
        }


        private void AutoScrollToUser()
        {
            int userIndex = Data.LeaderboardData.Instance.players.FindIndex(p => p.isUser);
            if (userIndex == -1) return; 


            int startIndex = Mathf.Clamp(userIndex - visibleCount / 2, 0, totalCount - visibleCount);

            UpdateItems(startIndex);
            
            float normalized = 1f - ((float)startIndex / (totalCount - visibleCount));
            scrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalized);
        }



        private void OnScrollValueChanged(Vector2 pos)
        {
            if (totalCount <= visibleCount) return;


            int firstIndex = Mathf.FloorToInt((1 - pos.y) * (totalCount - visibleCount));
            firstIndex = Mathf.Clamp(firstIndex, 0, totalCount - visibleCount);

            UpdateItems(firstIndex);
        }

        private void UpdateItems(int startIndex)
        {
            var players = Data.LeaderboardData.Instance.players;

            for (int i = 0; i < visibleCount; i++)
            {
                int dataIndex = startIndex + i;

                if (dataIndex >= totalCount)
                {
                    itemPool[i].SetActive(false);
                    continue;
                }

                PlayerInfo player = players[dataIndex];
                itemPool[i].SetActive(true);

                var leaderBoardItem = itemPool[i].GetComponent<LeaderBoardItem>();
                leaderBoardItem.Initialize(player.rank, player.name, player.score, player.isUser);


                var rt = itemPool[i].GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(0, -dataIndex * itemHeight);
            }
        }
    }
}
