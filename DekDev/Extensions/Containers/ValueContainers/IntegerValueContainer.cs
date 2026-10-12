namespace DekDev.Extensions.Containers.ValueContainers
{
    using UnityEngine;

#if PlayerStats_yg && UNITY_WEBGL
    using YG;
#endif

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Integer value container.
    /// </summary>
    [CreateAssetMenu(menuName = "DekDev/Extensions/ValueContainers/" + nameof(IntegerValueContainer))]
    public class IntegerValueContainer : AbstractValueContainer<int>
    {
        protected override int LoadValueFromPrefs()
        {
#if PlayerStats_yg && UNITY_WEBGL
            return YG2.GetState(prefsKey);
#else
            return PlayerPrefs.GetInt(prefsKey, val);
#endif
        }

        protected override void SaveValueInPrefs(int targetValue)
        {
            if (targetValue > int.MinValue && targetValue < int.MaxValue)
            {
                SaveValue(targetValue);
            }
        }

        protected virtual void SaveValue(int targetValue)
        {
#if PlayerStats_yg && UNITY_WEBGL
            YG2.SetState(prefsKey, targetValue);
#else
            PlayerPrefs.SetInt(prefsKey, targetValue);
                PlayerPrefs.Save();
#endif
        }
    }
}