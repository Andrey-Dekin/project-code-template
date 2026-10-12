namespace DekDev.Extensions.Containers.ValueContainers
{
    using UnityEngine;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Bool value container.
    /// </summary>
    [CreateAssetMenu(menuName = "DekDev/Extensions/ValueContainers/" + nameof(BoolValueContainer))]
    public class BoolValueContainer : AbstractValueContainer<bool>
    {
        protected override bool LoadValueFromPrefs() => PlayerPrefs.GetInt(prefsKey, val ? 1 : 0) == 1;

        protected override void SaveValueInPrefs(bool targetValue)
        {
            PlayerPrefs.SetInt(prefsKey, targetValue ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}