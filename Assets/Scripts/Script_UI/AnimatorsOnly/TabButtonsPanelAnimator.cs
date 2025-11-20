namespace UI
{
    using UnityEngine;
    using DG.Tweening;

    public class TabButtonsPanelAnimator : MonoBehaviour
    {
        [Header("Refs")]
        public RectTransform targetPanel;

        [Header("Settings")]
        public float moveDuration = 0.4f;

        [Header("Animation Offset")]
        public float moveOffsetY = -150f;

        private Vector2 originalPos;

        private void Awake()
        {
            originalPos = targetPanel.anchoredPosition;
        }

        private void Start()
        {
            targetPanel.DOKill();
            targetPanel.anchoredPosition = originalPos + new Vector2(0, moveOffsetY);
            targetPanel.DOAnchorPos(originalPos, moveDuration).SetEase(Ease.OutCubic);
        }
    }
}