namespace WinterDrift.Features.AudioManager
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using DekDev.Extensions.Coroutines;

    /// <summary>
    /// Компонент для управления воспроизведением музыки
    /// Обрабатывает плейлисты, фейды и переходы между треками
    /// </summary>
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] protected float fadeDuration = 1f;
        [SerializeField] protected float volume = 1f;
        protected AudioData audioData = default;
        protected AudioSource audioSource = default;
        protected List<AudioClip> currentPlaylist = new List<AudioClip>();
        protected int currentClipIndex = -1;
        protected CoroutineExtension fadeCoroutine = default;
        protected CoroutineExtension waitForClipEndCoroutine = default;

        protected virtual void OnDisable()
        {
            if (fadeCoroutine != null)
            {
                fadeCoroutine.Coroutine = null;
            }

            if (waitForClipEndCoroutine != null)
            {
                waitForClipEndCoroutine.Coroutine = null;
            }
        }

        /// <summary>
        /// Инициализация компонента MusicPlayer
        /// </summary>
        /// <param name="data">Данные аудио</param>
        /// <param name="source">Источник аудио</param>
        public virtual void Initialize(AudioData data, AudioSource source)
        {
            audioData = data;
            audioSource = source;
            fadeCoroutine = new CoroutineExtension(this);
            waitForClipEndCoroutine = new CoroutineExtension(this);

            if (audioSource != null)
            {
                audioSource.loop = false;
                audioSource.volume = 0f;
            }
        }

        /// <summary>
        /// Воспроизведение музыки для меню
        /// </summary>
        public virtual void PlayMenuMusic()
        {
            if (audioData == null)
            {
                return;
            }

            currentPlaylist = audioData.MenuMusicClips;
            PlayNextClip();
        }

        /// <summary>
        /// Воспроизведение музыки для игрового процесса
        /// </summary>
        public virtual void PlayGameplayMusic()
        {
            if (audioData == null)
            {
                return;
            }

            currentPlaylist = audioData.GameplayMusicClips;
            PlayNextClip();
        }

        /// <summary>
        /// Остановка воспроизведения музыки
        /// </summary>
        public virtual void StopMusic()
        {
            if (fadeCoroutine != null)
            {
                fadeCoroutine.Coroutine = null;
            }

            if (waitForClipEndCoroutine != null)
            {
                waitForClipEndCoroutine.Coroutine = null;
            }

            if (audioSource != null && audioSource.isPlaying)
            {
                StartCoroutine(FadeOutAndStop());
            }
        }

        /// <summary>
        /// Установка громкости музыки
        /// </summary>
        /// <param name="newVolume">Новое значение громкости от 0 до 1</param>
        public virtual void SetVolume(float newVolume)
        {
            volume = Mathf.Clamp01(newVolume);

            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.volume = volume;
            }
        }

        /// <summary>
        /// Плавный переход к следующему треку
        /// </summary>
        public virtual void FadeToNextClip()
        {
            if (currentPlaylist == null || currentPlaylist.Count == 0)
            {
                return;
            }

            if (waitForClipEndCoroutine != null && waitForClipEndCoroutine.Coroutine != null)
            {
                waitForClipEndCoroutine.Coroutine = null;
            }

            fadeCoroutine.Coroutine = StartCoroutine(FadeOutAndPlayNext());
        }

        protected virtual void PlayNextClip()
        {
            if (currentPlaylist == null)
            {
                Debug.LogWarning("Текущий плейлист не установлен!");
                return;
            }

            if (currentPlaylist.Count == 0)
            {
                Debug.LogWarning("В текущем плейлисте нет музыкальных клипов!");
                return;
            }

            int nextIndex = CalculateNextClipIndex();
            currentClipIndex = nextIndex;

            fadeCoroutine.Coroutine = StartCoroutine(FadeToNewClip(currentPlaylist[currentClipIndex]));
        }

        protected virtual void PlayNextClipWithoutFade()
        {
            if (currentPlaylist == null || currentPlaylist.Count == 0)
            {
                Debug.LogWarning("Текущий плейлист не установлен или пуст!");
                return;
            }

            int nextIndex = CalculateNextClipIndex();
            currentClipIndex = nextIndex;

            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = currentPlaylist[currentClipIndex];
                audioSource.volume = volume;
                audioSource.Play();

                waitForClipEndCoroutine.Coroutine = StartCoroutine(WaitForClipEnd());
            }
        }

        protected virtual int CalculateNextClipIndex()
        {
            if (currentPlaylist.Count == 0)
            {
                return -1;
            }

            if (currentPlaylist.Count == 1)
            {
                return 0;
            }

            int nextIndex;
            do
            {
                nextIndex = Random.Range(0, currentPlaylist.Count);
            }
            while (nextIndex == currentClipIndex && currentPlaylist.Count > 1);

            return nextIndex;
        }

        protected virtual IEnumerator FadeToNewClip(AudioClip newClip)
        {
            if (audioSource == null)
            {
                yield break;
            }

            if (audioSource.isPlaying)
            {
                yield return FadeOutCurrentClip();
            }

            yield return PlayNewClipWithFade(newClip);
        }

        protected virtual IEnumerator FadeOutCurrentClip()
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
        }

        protected virtual IEnumerator PlayNewClipWithFade(AudioClip newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(0f, volume, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = volume;

            waitForClipEndCoroutine.Coroutine = StartCoroutine(WaitForClipEnd());
        }

        protected virtual IEnumerator FadeOutAndPlayNext()
        {
            if (audioSource == null || !audioSource.isPlaying)
            {
                yield break;
            }

            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();

            PlayNextClip();
        }

        protected virtual IEnumerator FadeOutAndStop()
        {
            if (audioSource == null || !audioSource.isPlaying)
            {
                yield break;
            }

            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
        }

        protected virtual IEnumerator WaitForClipEnd()
        {
            if (audioSource == null || audioSource.clip == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(audioSource.clip.length - audioSource.time);

            PlayNextClipWithoutFade();
        }
    }
}