using System;
using DG.Tweening;
using NaughtyAttributes;
using ScriptableObjects.GameEvents;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverPanelAnimator : MonoBehaviour
    {
        public NullEvent onGameOver;
        public NullEvent onGameReset;
        public NullEvent onGoHome;
        public NullEvent onGameStart;
        [Header("UI References")]
        public GameObject gameOverPanel;
        public TextMeshProUGUI playerNameText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI highScoreText;

        public GameObject resetButton;
        public GameObject homeButton;
        public GameObject shareButton;

        [Header("Animation Settings")]
        public float moveDuration = 0.8f;
        public float staggerDelay = 0.2f;

        private Vector3 playerNameStartPos;
        private Vector3 scoreStartPos;
        private Vector3 highScoreStartPos;

        private Vector3 resetButtonStartPos;
        private Vector3 homeButtonStartPos;
        private Vector3 shareButtonStartPos;

        private void Start()
        {
            if (playerNameText != null) playerNameStartPos = playerNameText.transform.position;
            if (scoreText != null) scoreStartPos = scoreText.transform.position;
            if (highScoreText != null) highScoreStartPos = highScoreText.transform.position;

            if (resetButton != null) resetButtonStartPos = resetButton.transform.position;
            if (homeButton != null) homeButtonStartPos = homeButton.transform.position;
            if (shareButton != null) shareButtonStartPos = shareButton.transform.position;
        }

        private void OnEnable()
        {
            onGameOver.OnEventRaised += PlayShowAnimation;
            onGameReset.OnEventRaised += GameReset;
            onGoHome.OnEventRaised += GoHome;
        }

        private void GoHome()
        {
            PlayHideAnimation(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);}
                );
        }

        private void GameReset()
        {
            PlayHideAnimation(()=>{ onGameStart.Raise();}
                ,true);
        }

        private void OnDisable()
        {
            onGameOver.OnEventRaised -= PlayShowAnimation;
            onGameReset.OnEventRaised -= GameReset;
            onGameReset.OnEventRaised -= GoHome;
        }


        private void PlayShowAnimation()
        {
            gameOverPanel.SetActive(true);
            if (playerNameText != null) playerNameText.transform.position = playerNameStartPos + Vector3.up * 1500;
            if (scoreText != null) scoreText.transform.position = scoreStartPos + Vector3.up * 1500;
            if (highScoreText != null) highScoreText.transform.position = highScoreStartPos + Vector3.up * 1500;

            if (resetButton != null) resetButton.transform.position = resetButtonStartPos + Vector3.down * 1500;
            if (homeButton != null) homeButton.transform.position = homeButtonStartPos + Vector3.right * 1500;
            if (shareButton != null) shareButton.transform.position = shareButtonStartPos + Vector3.left * 1500;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(1.5f);
            
            if (playerNameText != null)
                seq.Append(playerNameText.transform.DOMoveY(playerNameStartPos.y, moveDuration));
            if (scoreText != null)
                seq.Join(scoreText.transform.DOMoveY(scoreStartPos.y, moveDuration).SetDelay(staggerDelay));
            if (highScoreText != null)
                seq.Join(highScoreText.transform.DOMoveY(highScoreStartPos.y, moveDuration).SetDelay(staggerDelay * 2));

  
            if (resetButton != null)
                seq.Join(resetButton.transform.DOMoveY(resetButtonStartPos.y, moveDuration).SetDelay(staggerDelay));
            if (homeButton != null)
                seq.Join(homeButton.transform.DOMoveX(homeButtonStartPos.x, moveDuration).SetDelay(staggerDelay));
            if (shareButton != null)
                seq.Join(shareButton.transform.DOMoveX(shareButtonStartPos.x, moveDuration));

            seq.Play();
        }
        
        private void PlayHideAnimation(Action onComplete,bool reset = false)
        {

            Sequence seq = DOTween.Sequence();


            if (playerNameText != null)
                seq.Append(playerNameText.transform.DOMoveY(playerNameStartPos.y + 1500, moveDuration).SetEase(Ease.InCubic));
            if (scoreText != null)
                seq.Join(scoreText.transform.DOMoveY(scoreStartPos.y + 1500, moveDuration).SetEase(Ease.InCubic));
            if (highScoreText != null)
                seq.Join(highScoreText.transform.DOMoveY(highScoreStartPos.y + 1500, moveDuration).SetEase(Ease.InCubic));
            if (resetButton != null)
                seq.Join(resetButton.transform.DOMoveY(resetButtonStartPos.y - 1500, moveDuration).SetEase(Ease.InCubic));
            if (homeButton != null)
                seq.Join(homeButton.transform.DOMoveX(homeButtonStartPos.x + 1500, moveDuration).SetEase(Ease.InCubic));
            if (shareButton != null)
                seq.Join(shareButton.transform.DOMoveX(shareButtonStartPos.x - 1500, moveDuration).SetEase(Ease.InCubic));

            if(reset) seq.AppendCallback(() => onComplete.Invoke());
            else
                seq.OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            seq.Play();
        }
    }
}
