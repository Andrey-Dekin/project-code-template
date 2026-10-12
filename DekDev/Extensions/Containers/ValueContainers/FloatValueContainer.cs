namespace DekDev.Extensions.Containers.ValueContainers
{
    using UnityEngine;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Float value container.
    /// </summary>
    [CreateAssetMenu(menuName = "DekDev/Extensions/ValueContainers/" + nameof(FloatValueContainer))]
    public class FloatValueContainer : AbstractValueContainer<float>
    {
        protected override float LoadValueFromPrefs() => PlayerPrefs.GetFloat(prefsKey, val);

        protected override void SaveValueInPrefs(float targetValue)
        {
            PlayerPrefs.SetFloat(prefsKey, targetValue);
            PlayerPrefs.Save();
        }
    }
}