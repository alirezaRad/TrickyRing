using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HomePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerHighScoreText;
        private void Start()
        {
            playerNameText.text = PlayerPrefsSaveService.Main.LoadString("PlayerName","HoneyDrops");
            playerHighScoreText.text = "HighScore : " + PlayerPrefsSaveService.Main.LoadInt("PlayerHighScore", 5000).ToString();
        }
    }
}