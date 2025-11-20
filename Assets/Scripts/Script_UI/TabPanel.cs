using UnityEngine;
using DG.Tweening;
using Enums;
using Script_ScriptableObjects.UIConfigs;
using ScriptableObjects.GameEvents;

namespace UI
{
    public class TabPanel : MonoBehaviour
    {
        public TabType tab;
        public IntEvent onTabSelected;
        private PanelSequence panelSequence;

        [Header("Animation")]
        public float animDuration = 0.4f;
        public float offscreenX = 1080f;

        private RectTransform rect;
        private static int activeIndex = 0; 

        private Vector2 originalPos;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            panelSequence = PanelSequence.Main;
            activeIndex = (int)panelSequence.firstActiveTab;

            originalPos = rect.anchoredPosition;
        }

        void OnEnable()
        {
            onTabSelected.OnEventRaised += HandleTabSelected;
        }

        void OnDisable()
        {
            onTabSelected.OnEventRaised -= HandleTabSelected;
        }

        void Start()
        {
            onTabSelected.Raise(activeIndex);
        }

        private void HandleTabSelected(int selectedIndex)
        {
            activeIndex = selectedIndex;
            UpdatePosition(selectedIndex);
        }

        private void UpdatePosition(int centerIndex, bool instant = false)
        {
            int myIndex = System.Array.IndexOf(panelSequence.sequence, tab);
            Vector2 targetPos;

            if (myIndex < centerIndex)
                targetPos = originalPos + new Vector2(-offscreenX, 0);
            else if (myIndex > centerIndex)
                targetPos = originalPos + new Vector2(offscreenX, 0);
            else
                targetPos = originalPos;

            rect.DOKill();
            if (instant)
                rect.anchoredPosition = targetPos;
            else
                rect.DOAnchorPos(targetPos, animDuration).SetEase(Ease.OutCubic);
        }
    }
}
