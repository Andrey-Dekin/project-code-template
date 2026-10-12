namespace WinterDrift.Features.QualitySettigns
{
    using DekDev.CDebug;
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;

    /// <summary>
    /// Кнопка выбора настроек графики.
    /// </summary>
    public class QualityButton : AbstractButtonView
    {
        [Header("Настройки качества")]
        [SerializeField] protected int qualityPresetIndex = 0; // Индекс пресета в контроллере
        [SerializeField] protected string qualityPresetName = ""; // Альтернатива: имя пресета

        [Header("Визуальные настройки")]
        [SerializeField] protected Sprite selectedSprite = default;
        [SerializeField] protected Sprite normalSprite = default;

        protected GraphicsQualityController qualityController;

        protected override void Awake()
        {
            base.Awake();

            FindQualityController();
        }

        protected virtual void OnEnable()
        {
            if (qualityController != null)
            {
                qualityController.OnQualityChanged += OnQualityChanged;
                UpdateVisualState();
            }
        }

        protected virtual void OnDisable()
        {
            if (qualityController != null)
            {
                qualityController.OnQualityChanged -= OnQualityChanged;
            }
        }

        protected virtual void FindQualityController()
        {
            if (qualityController == null)
            {
                qualityController = FindObjectOfType<GraphicsQualityController>();

                if (qualityController == null)
                {
                    CDebug.LogWarning("GraphicsQualityController не найден на сцене!");
                }
            }
        }

        protected override void OnButtonClicked()
        {
            if (qualityController == null)
            {
                FindQualityController();
            }

            if (qualityController != null)
            {
                // Используем либо индекс, либо имя
                if (!string.IsNullOrEmpty(qualityPresetName))
                {
                    qualityController.SetQualityLevel(qualityPresetName);
                }
                else
                {
                    qualityController.SetQualityLevel(qualityPresetIndex);
                }
            }
        }

        protected virtual void OnQualityChanged(int newQualityIndex)
        {
            UpdateVisualState();
        }

        /// <summary>
        /// Обновить визуальное состояние кнопки
        /// </summary>
        public void UpdateVisualState()
        {
            if (qualityController == null) return;

            int currentQualityIndex = qualityController.GetCurrentQualityIndex();
            bool isSelected = false;

            if (!string.IsNullOrEmpty(qualityPresetName))
            {
                string currentName = qualityController.GetCurrentQualityName();
                isSelected = currentName.Equals(qualityPresetName, System.StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                isSelected = (qualityPresetIndex == currentQualityIndex);
            }

            buttonView.image.sprite = isSelected ? selectedSprite : normalSprite;
        }

        /// <summary>
        /// Принудительно установить качество через эту кнопку
        /// </summary>
        public void ForceSetQuality()
        {
            OnButtonClicked();
        }
    }
}