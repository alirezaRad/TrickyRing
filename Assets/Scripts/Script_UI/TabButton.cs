using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Enums;
using ScriptableObjects.GameEvents;

namespace UI
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private TabType tab;     
        [SerializeField] private IntEvent onTabSelected;      

        [Header("Button References")]
        [SerializeField] private Button button;
        [SerializeField] private Image icon;       

        [Header("Animation Settings")]
        [SerializeField] private float scaleDuration = 0.2f;
        [SerializeField] private float activeScale = 1.2f;
        [SerializeField] private float inactiveScale = 1f;
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = Color.gray;

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