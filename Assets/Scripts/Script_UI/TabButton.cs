using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Enums;
using ScriptableObjects.GameEvents;

namespace UI
{
    public class TabButton : MonoBehaviour
    {
        public TabType tab;     
        public IntEvent onTabSelected;      

        [Header("Button References")]
        public Button button;
        public Image icon;       

        [Header("Animation Settings")]
        public float scaleDuration = 0.2f;
        public float activeScale = 1.2f;
        public float inactiveScale = 1f;
        public Color activeColor = Color.white;
        public Color inactiveColor = Color.gray;

        private void Awake()
        {
            if (button)
                button.onClick.AddListener(OnClick);
        }

        private void OnEnable()
        {
            onTabSelected.OnEventRaised += HandleTabSelected;
        }

        private void OnDisable()
        {
            onTabSelected.OnEventRaised -= HandleTabSelected;
        }

        private void OnClick()
        {
            onTabSelected.Raise((int)tab);
        }

        private void HandleTabSelected(int selectedIndex)
        {
            bool isActive = (int)tab == selectedIndex;
            
            Animate(isActive);
            
            if (isActive)
                transform.SetAsLastSibling();
        }


        private void Animate(bool isActive)
        {
            transform.DOKill();
            transform.DOScale(isActive ? activeScale : inactiveScale, scaleDuration);
            
            if (icon != null)
            {
                icon.DOKill();
                icon.DOColor(isActive ? activeColor : inactiveColor, scaleDuration);
            }
        }
    }
}