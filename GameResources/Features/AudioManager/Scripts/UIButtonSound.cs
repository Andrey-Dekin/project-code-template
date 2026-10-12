namespace WinterDrift.Features.AudioManager
{
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Компонент для добавления звукового сопровождения кнопкам пользовательского интерфейса
    /// </summary>
    public class UIButtonSound : AbstractButtonView, IPointerEnterHandler
    {
        [Header("Настройки воспроизведения звуков")]
        [SerializeField] protected bool playClickSound = true;
        [SerializeField] protected bool playHoverSound = true;

        protected UISoundController uiSoundController;

        protected virtual void Start()
        {
            FindUISoundController();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (playHoverSound && uiSoundController != null)
            {
                uiSoundController.PlayButtonHover();
            }
        }

        protected override void OnButtonClicked()
        {
            if (playClickSound && uiSoundController != null)
            {
                uiSoundController.PlayButtonClick();
            }
        }

        protected virtual void FindUISoundController()
        {
            if (SoundManager.Instance != null)
            {
                uiSoundController = SoundManager.Instance.GetComponent<UISoundController>();
            }
        }
    }
}