using Enums;

namespace Script_ScriptableObjects.UIConfigs
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "UI/PanelSequence")]
    public class PanelSequence : ScriptableObject
    {
        public TabType[] sequence;

        public static PanelSequence Main; 
        public TabType firstActiveTab;

        private void OnEnable()
        {
            Main = this;
        }
    }

}