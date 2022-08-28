namespace ProtoRoguelite.Statistics
{
    using ProtoRoguelite.Characters;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    [CreateAssetMenu(fileName = "NewStatisticModifierSO", menuName = "ScriptableObjects/Statistics/StatisticModifierSO")]
    public class StatisticModifierSO : ScriptableObject
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Image _image;

        [SerializeField] private float _value;
        [SerializeField] private float _duration;
        #endregion Serialized Fields

        #region Private Fields
        [SerializeField, HideInInspector] private int _statisticIndex;
        #endregion Private Fields

        #region Properties
        public string Name => _name;
        public string Description => _description;
        public Image Image => _image;
        public float Value => _value;
        public float Duration => _duration;

        public int StatisticIndex { get => _statisticIndex; set => _statisticIndex = value; }
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        #endregion Public Methods

        #endregion Methods

#if UNITY_EDITOR
        [CustomEditor(typeof(StatisticModifierSO))]
        public class StatisticModifierEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                StatisticModifierSO statModSO = target as StatisticModifierSO;

                if (statModSO == null)
                    return;

                int pptIndex = statModSO.StatisticIndex;

                PropertyInfo[] pptInfos = ReflectionManager.GetProperties<Character>(new List<System.Type>() { typeof(Statistic) });
                string[] pptNames = ReflectionManager.GetPropertyNames(pptInfos);

                if (pptNames.Length == 0)
                    return;

                pptIndex = EditorGUILayout.Popup("Statistic modified :", pptIndex, pptNames);

                statModSO.StatisticIndex = pptIndex;

                EditorUtility.SetDirty(target);
            }
        }
#endif //UNITY_EDITOR
    }
}