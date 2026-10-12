namespace DekDev.LoadSceneManager
{
    using UnityEngine;

    /// <summary>
    /// Базовая абстрактная view для загрузки
    /// </summary>
    public abstract class AbstractLoadView : MonoBehaviour
    {
        protected virtual void OnEnable() => LoadingSceneController.onSceneProgressLoadChanged += OnUpdateProgressValue;

        protected virtual void OnDisable() => LoadingSceneController.onSceneProgressLoadChanged -= OnUpdateProgressValue;

        /// <summary>
        /// Что делать view по факту получения сигнала о проценте загрузки
        /// </summary>
        /// <param name="inputValue"></param>
        public abstract void OnUpdateProgressValue(float inputValue);
    }
}