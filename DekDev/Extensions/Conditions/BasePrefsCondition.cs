namespace DekDev.Extensions.Conditions
{
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Кондишен с сохранением значения в префсы.
    /// </summary>
    [CreateAssetMenu(menuName = "DekDev/Extensions/Conditions/" + nameof(BasePrefsCondition))]
    public class BasePrefsCondition : AbstractCondition
    {
        [SerializeField, Header("Обязательно к заполнению для сохранения в префсах")]
        protected string prefsKey = string.Empty;

        protected bool isConditionMet
        {
            get
            {
                return PlayerPrefs.GetInt(prefsKey, 0) > 0;
            }

            set
            {
                PlayerPrefs.SetInt(prefsKey, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public override bool IsConditionMet() => isConditionMet;

        public override void SetConditionStatus(bool status)
        {
            isConditionMet = status;

            OnConditionStatusChange();
        }
    }
}