namespace DekDev.LoadSceneManager
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// View загрузки уровня с использоваием слайдера.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class LoadingSliderView : AbstractLoadView
    {
        private Slider _sliderView = default;
        private float _target = 0.5f;

        protected virtual void Start()
        {
            _sliderView = GetComponent<Slider>();
            _target = 0f;
            _sliderView.value = 0f;
        }

        protected virtual void Update() => _sliderView.value = Mathf.Lerp(_sliderView.value, _target, 1.5f * Time.unscaledDeltaTime);

        public override void OnUpdateProgressValue(float inputValue)
        {
            if (inputValue > _target)
            {
                _target = inputValue;
            }
        }
    }
}