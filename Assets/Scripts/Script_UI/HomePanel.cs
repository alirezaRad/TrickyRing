using System;
using ScriptableObjects.GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class HomePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerHighScoreText;
        [SerializeField] private Button startGameButton;
        [SerializeField] private NullEvent onStartGame;
        private void Start()
        {
            playerNameText.text = PlayerPrefsSaveService.Main.LoadString("PlayerName","HoneyDrops");
            playerHighScoreText.text = "HighScore : " + PlayerPrefsSaveService.Main.LoadInt("PlayerHighScore", 5000).ToString();
            startGameButton.onClick.AddListener(StartGame);
        }

        private void OnEnable()
        {
            onStartGame.OnEventRaised += NextScene;
        }

        private void NextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1,LoadSceneMode.Additive);
        }

        private void StartGame()
        {
            onStartGame.Raise();
        }
    }
}