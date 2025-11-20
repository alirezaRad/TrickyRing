using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace UI
{
    public class TabButtonsPanelAnimator : MonoBehaviour
    {
        [Header("Refs")]
        public RectTransform targetPanel;

        [Header("Settings")]
        public float moveDuration = 0.4f;

        [Header("Animation Offset")]
        public float moveOffsetY = -150f;

        private Vector2 originalPos;
        private Sequence panelSeq;

        private void Awake()
        {
            originalPos = targetPanel.anchoredPosition;
        }

        private void Start()
        {
            AnimateForward();
        }

        private void AnimateForward()
        {
            panelSeq = DOTween.Sequence().SetAutoKill(false);
            
            targetPanel.anchoredPosition = originalPos + new Vector2(0, moveOffsetY);
            
            panelSeq.Append(
                targetPanel.DOAnchorPos(originalPos, moveDuration)
                    .SetEase(Ease.OutCubic)
            );
        }

        [Button] 
        public void ReverseAnimation()
        {
            if (panelSeq != null && panelSeq.IsActive())
                panelSeq.PlayBackwards();
        }
        
    }
}