namespace WinterDrift.Features.AudioManager
{
    using DekDev.CDebug;

    using UnityEngine;
    using UnityEngine.Audio;

    /// <summary>
    /// Контроллер снепшотов.
    /// </summary>
    public class AudioMixerSnapshots : MonoBehaviour
    {
        protected const float TRANSITION_TIME = 1f;

        [SerializeField] protected AudioMixer audioMixer = default;

        protected AudioMixerSnapshot normalSnapshot = default;
        protected AudioMixerSnapshot UIWindowSnapshot = default;

        protected virtual void Awake()
        {
            if (audioMixer != null)
            {
                normalSnapshot = audioMixer.FindSnapshot("NormalState");
                UIWindowSnapshot = audioMixer.FindSnapshot("UIState");
            }
        }

        /// <summary>
        /// Переход к снимку UI окна.
        /// </summary>
        [ContextMenu("test UI open")]
        public void TransitionToUIWindowState()
        {
            if (UIWindowSnapshot != null)
            {
                UIWindowSnapshot.TransitionTo(TRANSITION_TIME);
                return;

            }

            CDebug.LogError("Не найден нужный снэпшот");
        }

        /// <summary>
        /// Переход к нормальному снимку.
        /// </summary>
        [ContextMenu("test close UI")]
        public void TransitionToNormalState()
        {
            if (normalSnapshot != null)
            {
                normalSnapshot.TransitionTo(TRANSITION_TIME);
                return;
            }

            CDebug.LogError("Не найден нужный снэпшот");
        }
    }
}