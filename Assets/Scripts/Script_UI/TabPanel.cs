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
        private static int activeIndex = 1; // default center

        private Vector2 originalPos; // store the anchored position

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            panelSequence = PanelSequence.Main;

            originalPos = rect.anchoredPosition; // save original anchored position
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
            UpdatePosition(activeIndex, instant: true);
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
                rect.anchoredPosition = targetPos; // set instantly for Start()
            else
                rect.DOAnchorPos(targetPos, animDuration).SetEase(Ease.OutCubic);
        }
    }
}
