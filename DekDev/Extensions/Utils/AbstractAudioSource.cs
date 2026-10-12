namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine;

    /// <summary>
    /// Абстрактный класс источника звука
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public abstract class AbstractAudioSource : MonoBehaviour
    {
        /// <summary>
        /// Источник звука
        /// </summary>
        public AudioSource Audio => audioSource;
        protected AudioSource audioSource = default;

        protected virtual void Awake() => audioSource = GetComponent<AudioSource>();
    }
}