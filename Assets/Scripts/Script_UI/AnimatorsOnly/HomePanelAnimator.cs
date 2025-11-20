using UnityEngine;
using DG.Tweening;
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

        private void Start()
        {
            AnimatePlayerName();
            AnimateGameName();
            AnimateStartButton();
        }

        private void AnimatePlayerName()
        {
            Vector2 original = playerName.anchoredPosition;
            playerName.anchoredPosition = original + new Vector2(-moveDistance, 0);

            playerName.DOAnchorPos(original, moveDuration)
                .SetEase(Ease.OutCubic);
        }

        private void AnimateGameName()
        {
            Vector2 originalPos = gameName.anchoredPosition;
            
            gameName.anchoredPosition = originalPos + new Vector2(moveDistance, 0);
            
            gameName.localRotation = Quaternion.Euler(0, 0, flipRotation);  
            
            Sequence seq = DOTween.Sequence();
            
            seq.Append(
                gameName.DOAnchorPos(originalPos + new Vector2(-40f, 0), moveDuration * 0.6f)
                    .SetEase(Ease.OutBack)
            );
            seq.Append(
                gameName.DOAnchorPos(originalPos, moveDuration * 0.4f)
                    .SetEase(Ease.OutBack)
            );

            seq.Join(
                gameName.DOLocalRotate(Vector3.zero, moveDuration * 0.4f)
            );
        }

        
        private void AnimateStartButton()
        {
            Vector2 original = startButton.anchoredPosition;
            startButton.anchoredPosition = original + new Vector2(0, -moveDistance);

            startButton.DOAnchorPos(original, moveDuration)
                .SetEase(Ease.OutBack);
        }
    }
}