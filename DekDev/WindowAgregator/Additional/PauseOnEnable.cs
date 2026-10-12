namespace DekDev.WindowAgregator
{
    using UnityEngine;

    /// <summary>
    /// Pause timeScale on open window.
    /// </summary>
    public class PauseOnEnable : MonoBehaviour
    {
        [SerializeField] protected bool isPauseOnEnable = true;
        [SerializeField] protected bool isPauseOnDisable = false;

        protected virtual void OnEnable()
        {
            if (isPauseOnEnable)
            {
                Time.timeScale = 0f;
            }
        }

        protected virtual void OnDisable() => Time.timeScale = isPauseOnDisable ? 0f : 1;
    }
}