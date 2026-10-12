namespace DekDev.LoadSceneManager
{
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;

    /// <summary>
    /// Кнопка загрузки следующей сцены
    /// </summary>
    public class LoadButtonView : AbstractButtonView
    {
        [SerializeField] protected string targetScene = default;

        protected bool isAlreadyClicked = false;

        protected override void OnButtonClicked()
        {
            if (isAlreadyClicked) return;
            LoadingSceneController.NameNextScene = targetScene;
            isAlreadyClicked = true;
            LoadingSceneController.StartLoad();
        }
    }
}