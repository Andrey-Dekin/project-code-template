namespace DekDev.WindowAgregator
{
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;

    /// <summary>
    /// Open window button.
    /// </summary>
    public class ButtonOpen : AbstractButtonView
    {
        [SerializeField] protected Window windowPrefab = default;

        /// <summary>
        /// Call open window event.
        /// </summary>
        public void OpenWindow() => WindowAgregator.onWindowOpen(windowPrefab.Id);

        protected override void OnButtonClicked() => OpenWindow();
    }
}