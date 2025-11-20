using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace UI
{
    public class HomePanelAnimator : MonoBehaviour
    {
        [Header("Refs")]
        public RectTransform playerName;
        public RectTransform gameName;
        public RectTransform startButton;

        [Header("Settings")]
        public float moveDistance = 200f;
        public float moveDuration = 0.5f;
        public float scaleDuration = 0.5f;
        public float flipRotation = 90f;
        
        private Vector2 playerNameOriginal;
        private Vector2 gameNameOriginal;
        private Vector2 startButtonOriginal;
        private Quaternion gameNameRotationOriginal;
        
        private Sequence playerNameSeq;
        private Sequence gameNameSeq;
        private Sequence startButtonSeq;

        private void Awake()
        {
            playerNameOriginal = playerName.anchoredPosition;
            gameNameOriginal = gameName.anchoredPosition;
            startButtonOriginal = startButton.anchoredPosition;
            gameNameRotationOriginal = gameName.localRotation;
        }

        private void Start()
        {
            AnimatePlayerName();
            AnimateGameName();
            AnimateStartButton();
        }

        private void AnimatePlayerName()
        {
            playerNameSeq = DOTween.Sequence().SetAutoKill(false);
            
            playerName.anchoredPosition = playerNameOriginal + new Vector2(-moveDistance, 0);
            
            playerNameSeq.Append(
                playerName.DOAnchorPos(playerNameOriginal, moveDuration)
                    .SetEase(Ease.OutCubic)
            );
        }

        private void AnimateGameName()
        {
            gameNameSeq = DOTween.Sequence().SetAutoKill(false);
            
            gameName.anchoredPosition = gameNameOriginal + new Vector2(moveDistance, 0);
            gameName.localRotation = Quaternion.Euler(0, 0, flipRotation);
            
            gameNameSeq.Append(
                gameName.DOAnchorPos(gameNameOriginal + new Vector2(-40f, 0), moveDuration * 0.6f)
                    .SetEase(Ease.OutBack)
            );
            
            gameNameSeq.Join(
                gameName.DOLocalRotate(new Vector3(0, 0, -flipRotation), moveDuration * 0.6f)
            );
            
            gameNameSeq.Append(
                gameName.DOAnchorPos(gameNameOriginal, moveDuration * 0.4f)
                    .SetEase(Ease.OutBack)
            );
            gameNameSeq.Join(
                gameName.DOLocalRotate(gameNameRotationOriginal.eulerAngles, moveDuration * 0.4f)
            );
        }

        private void AnimateStartButton()
        {
            startButtonSeq = DOTween.Sequence().SetAutoKill(false);

            startButton.anchoredPosition = startButtonOriginal + new Vector2(0, -moveDistance);

            startButtonSeq.Append(
                startButton.DOAnchorPos(startButtonOriginal, moveDuration)
                    .SetEase(Ease.OutBack)
            );
        }

        [Button]
        public void ReverseAnimations()
        {
            if (playerNameSeq != null && playerNameSeq.IsActive())
                playerNameSeq.PlayBackwards();

            if (gameNameSeq != null && gameNameSeq.IsActive())
                gameNameSeq.PlayBackwards();

            if (startButtonSeq != null && startButtonSeq.IsActive())
                startButtonSeq.PlayBackwards();
        }
    }
}
