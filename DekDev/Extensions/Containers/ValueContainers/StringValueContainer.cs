namespace DekDev.Extensions.Containers.ValueContainers
{
    using UnityEngine;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// String value container.
    /// </summary>
    [CreateAssetMenu(menuName = "DekDev/Extensions/ValueContainers/" + nameof(StringValueContainer))]
    public class StringValueContainer : AbstractValueContainer<string>
    {
        protected override string LoadValueFromPrefs() => PlayerPrefs.GetString(prefsKey, val);

        protected override void SaveValueInPrefs(string targetValue)
        {
            PlayerPrefs.SetString(prefsKey, targetValue);
            PlayerPrefs.Save();
        }
    }
}