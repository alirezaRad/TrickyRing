using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Enums;
using ScriptableObjects.GameEvents;
using DataStructure;


namespace Data
{
    public class LeaderboardData : MonoBehaviour
    {
        public static LeaderboardData Instance { get; private set; }

        [SerializeField] private TextAsset namesFile;
        [SerializeField] private IntEvent OnPanelSelected;
        [SerializeField] private NullEvent OnDataLoaded;

        public List<PlayerInfo> players;
        [SerializeField] private int loadFrameCount = 1;
        [SerializeField] private int insertChunkSize = 5000;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            OnPanelSelected.OnEventRaised += CheckForLoad;
        }

        private void OnDisable()
        {
            OnPanelSelected.OnEventRaised -= CheckForLoad;
        }

        private void CheckForLoad(int tabType)
        {
            StartCoroutine(LoadLeaderboardCoroutine());
        }

        private IEnumerator LoadLeaderboardCoroutine()
        {
            string[] lines = namesFile.text.Split('\n');
            int total = lines.Length;
            int chunkSize = Mathf.CeilToInt((float)total / loadFrameCount);

            players = new List<PlayerInfo>(total);


            for (int f = 0; f < loadFrameCount; f++)
            {
                int start = f * chunkSize;
                int end = Mathf.Min(start + chunkSize, total);

                for (int i = start; i < end; i++)
                {
                    string nameOfUser = lines[i].Trim();
                    players.Add(new PlayerInfo
                    {
                        name = nameOfUser,
                        rank = i + 1,
                        score = Random.Range(0, 100000),
                        isUser = false
                    });
                }

                yield return null; 
            }


            players.Sort((a, b) => b.score.CompareTo(a.score));


            PlayerInfo user = new PlayerInfo
            {
                name = PlayerPrefsSaveService.Main.LoadString("PlayerName", "Honey Drops"),
                score = PlayerPrefsSaveService.Main.LoadInt("PlayerScore", 5000),
                isUser = true
            };

            yield return StartCoroutine(InsertUserCoroutine(user));

            Debug.Log("Leaderboard loaded: " + players.Count + " players");
        }

        private IEnumerator InsertUserCoroutine(PlayerInfo user)
        {
            int index = players.BinarySearch(user, new PlayerScoreComparer());
            if (index < 0) index = ~index;

            players.Insert(index, user);
            user.rank = index + 1;

            int total = players.Count;
            
            for (int i = index + 1; i < total; i += insertChunkSize)
            {
                int end = Mathf.Min(i + insertChunkSize, total);
                for (int j = i; j < end; j++)
                {
                    players[j].rank += 1;
                }
                yield return null;
            }
            OnDataLoaded.Raise();
        }
    }
}


