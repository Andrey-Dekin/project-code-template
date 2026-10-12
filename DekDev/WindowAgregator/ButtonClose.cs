namespace DekDev.WindowAgregator
{
    using DekDev.Extensions.AbstractViews;

    /// <summary>
    /// Кнопка закрытия окна.
    /// </summary>
    public class ButtonClose : AbstractButtonView
    {
        protected override void OnButtonClicked() => WindowAgregator.onWindowClose();
    }
}