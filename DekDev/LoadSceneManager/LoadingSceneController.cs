namespace DekDev.LoadSceneManager
{
    using System;
    using UnityEngine;
    using System.Collections;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Контроллер загрузки сцен
    /// </summary>
    public class LoadingSceneController : MonoBehaviour
    {
        /// <summary>
        /// Собыие изменения прогресса загрузки сцены.
        /// </summary>
        public static event Action<float> onSceneProgressLoadChanged = delegate { };

        /// <summary>
        /// Название следующей сцены.
        /// </summary>
        public static string NameNextScene = "Menu";

        protected float loadProgress = 0f;

        /// <summary>
        /// Прогресс загрузки.
        /// </summary>
        public float LoadProgress
        {
            get => loadProgress;
            protected set
            {
                if (loadProgress != value)
                {
                    loadProgress = value;
                    onSceneProgressLoadChanged(loadProgress);
                }
            }
        }

        [SerializeField, Min(0)] protected float startDelay = 1.0f;
        [SerializeField, Min(0)] protected float loadingTimeout = 3.0f;
        
        /// <summary>
        /// Начать загрузку
        /// </summary>
        public static void StartLoad() => SceneManager.LoadSceneAsync("Loading");

        protected IEnumerator Start()
        {
            Time.timeScale = 1f;
            yield return new WaitForSecondsRealtime(startDelay);
            //Вызывает сборщик мусора (под капотом отработает GC.Collect)
            Resources.UnloadUnusedAssets();
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NameNextScene);

            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone && isActiveAndEnabled)
            {
                LoadProgress = asyncOperation.progress;
                if (asyncOperation.progress >= 0.9f && Time.timeSinceLevelLoad > loadingTimeout)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}