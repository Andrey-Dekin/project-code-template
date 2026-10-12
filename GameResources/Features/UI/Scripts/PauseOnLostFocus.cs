namespace WinterDrift.Features.UI
{
    using DekDev.WindowAgregator;
    using UnityEngine;

    /// <summary>
    /// Call pause window on application focus lost.
    /// </summary>
    public class PauseOnLostFocus : MonoBehaviour
    {
        [SerializeField] protected Window pauseWindow = default;

        protected virtual void OnApplicationFocus(bool hasFocus) => OnFocusChanged(hasFocus);

        protected virtual void OnApplicationPause(bool pauseStatus) => OnFocusChanged(!pauseStatus);

        protected virtual void OnFocusChanged(bool isFocuse)
        {
            if (!isFocuse)
            {
                WindowAgregator.onWindowOpen(pauseWindow.Id);
            }
        }
    }
}