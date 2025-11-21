using System;
using Enums;
using ScriptableObjects.GameEvents;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameSceneManager : MonoBehaviour
    {
       [SerializeField] private IntEvent OnScoreChanged;
       [SerializeField] private NullEvent OnGameEnded;
       [SerializeField] private NullEvent OnGameStarted;
       
       [SerializeField] private TextMeshProUGUI playerNameText;
       [SerializeField] private TextMeshProUGUI playerScoreText;
       [SerializeField] private TextMeshProUGUI playerHighScoreText;
       
       private int playerScore;
       private int playerHighScore;

       void Start()
       {
           playerNameText.text = PlayerPrefsSaveService.Main.LoadString("PlayerName","Honey Drops");
           playerScoreText.text = "Score : " + 0;
           playerScore = 0;
           playerHighScore = PlayerPrefsSaveService.Main.LoadInt("PlayerScore",5000);
           playerHighScoreText.text = "HighScore : " + playerHighScore;
       }

       private void OnEnable()
       {
           OnScoreChanged.OnEventRaised += OnScoreChange;
           OnGameStarted.OnEventRaised += Start;
       }

       private void OnScoreChange(int score)
       {
           playerScore = score;
           playerScoreText.text = "Score : " + score;
           if (playerScore > playerHighScore)
           {
               AudioManger.AudioManager.Instance.PlaySFX(SoundType.HighScore);
               playerHighScore = playerScore;
               PlayerPrefsSaveService.Main.SaveInt("PlayerScore", playerHighScore);
           }
       }
    }
}