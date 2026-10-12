namespace WinterDrift.Features.UI
{
    using DekDev.Extensions.Objects;
    using UnityEngine;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Show objects once with check prefs.
    /// </summary>
    public class ShowObjectsOnce : BaseObjectSwitcher
    {
        [SerializeField] protected string playerPrefsKey = "ObjectState";
        protected bool isShowed = false;

        protected virtual void Awake()
        {
            isShowed = PlayerPrefs.HasKey(playerPrefsKey);
        }

        protected virtual void OnEnable() => UpdateObjectsView();

        /// <summary>
        /// Set shown status.
        /// </summary>
        public void SetShown()
        {
            PlayerPrefs.SetInt(playerPrefsKey, 1);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Update objects view.
        /// </summary>
        public void UpdateObjectsView()
        {
            if (!isShowed)
            {
                isShowed = true;
                SetShown();

                SwitchObjects(true);
            }
            else
            {
                SwitchObjects(false);
            }
        }
    }
}