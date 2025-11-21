using System;
using DG.Tweening;
using ScriptableObjects.GameEvents;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

namespace UI
{ 
    public class GameSceneAnimationManager : MonoBehaviour
    {
        public NullEvent OnBallStartMoving;
        [Header("UI References")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI highScoreText;
        public TextMeshProUGUI playerNameText;
        public GameObject pauseButton;

        [Header("Game Objects")]
        public GameObject ball;
        public GameObject ring;

        [Header("Animation Settings")]
        public float moveDuration = 0.6f;
        public float scaleDuration = 0.6f;
        public float staggerDelay = 0.1f;

        private Vector3 ballStartPos;
        private Vector3 ringStartScale;
        private Vector3 pauseButtonStartPos;
        private Vector3 scoreTextStartPos;
        private Vector3 highScoreTextStartPos;
        private Vector3 playerNameStartPos;

        private void Awake()
        {
            if (ball != null) ballStartPos = ball.transform.position;
            if (ring != null) ringStartScale = ring.transform.localScale;
            if (pauseButton != null) pauseButtonStartPos = pauseButton.transform.position;
            if (scoreText != null) scoreTextStartPos = scoreText.transform.position;
            if (highScoreText != null) highScoreTextStartPos = highScoreText.transform.position;
            if (playerNameText != null) playerNameStartPos = playerNameText.transform.position;
        }

        public void Start()
        {
            PlayStartAnimation();
        }

        public void PlayStartAnimation()
        {
            if (scoreText != null) scoreText.transform.position = scoreTextStartPos + Vector3.left * 800;
            if (highScoreText != null) highScoreText.transform.position = highScoreTextStartPos + Vector3.left * 800;
            if (playerNameText != null) playerNameText.transform.position = playerNameStartPos + Vector3.left * 800;
            if (pauseButton != null) pauseButton.transform.position = pauseButtonStartPos + Vector3.right * 800;
            if (ball != null) ball.transform.position = ballStartPos + Vector3.right * 10 ;
            if (ring != null) ring.transform.localScale = Vector3.zero;

            Sequence startSeq = DOTween.Sequence();

            
            if (playerNameText != null)
                startSeq.Append(playerNameText.transform.DOMoveX(playerNameStartPos.x, moveDuration).SetEase(Ease.OutBack));

            if (pauseButton != null)
                startSeq.Join(pauseButton.transform.DOMoveX(pauseButtonStartPos.x, moveDuration).SetEase(Ease.OutBack));
            

            if (scoreText != null)
                startSeq.Join(scoreText.transform.DOMoveX(scoreTextStartPos.x, moveDuration).SetEase(Ease.OutBack).SetDelay(0.1f));
            
            if (ball != null)
                startSeq.Join(ball.transform.DOMoveX(ballStartPos.x, moveDuration*3f).SetEase(Ease.InBack));
            
            if (highScoreText != null)
                startSeq.Join(highScoreText.transform.DOMoveX(highScoreTextStartPos.x, moveDuration).SetEase(Ease.OutBack).SetDelay(0.1f));
            

            
            if (ring != null)
                startSeq.Join(ring.transform.DOScale(ringStartScale, scaleDuration).SetEase(Ease.OutBack).SetDelay(moveDuration+0.1f));
            
            
            startSeq.OnComplete(() =>
            {
                OnBallStartMoving.Raise();
            });

            startSeq.Play();
        }
        
        public void PlayEndAnimation()
        {
            Sequence endSeq = DOTween.Sequence();


            if (ball != null)
                endSeq.Append(ball.transform.DOMoveX(ballStartPos.x + 800, moveDuration).SetEase(Ease.InBack));

 
            if (ring != null)
                endSeq.Join(ring.transform.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.InBack));


            if (pauseButton != null)
                endSeq.Join(pauseButton.transform.DOMoveX(pauseButtonStartPos.x + 800, moveDuration).SetEase(Ease.InBack));


            if (scoreText != null)
                endSeq.Join(scoreText.transform.DOMoveX(scoreTextStartPos.x - 800, moveDuration).SetEase(Ease.InBack));

            if (highScoreText != null)
                endSeq.Join(highScoreText.transform.DOMoveX(highScoreTextStartPos.x - 800, moveDuration).SetEase(Ease.InBack));

            if (playerNameText != null)
                endSeq.Join(playerNameText.transform.DOMoveX(playerNameStartPos.x - 800, moveDuration).SetEase(Ease.InBack));

            endSeq.Play();
        }
    }
}