namespace WinterDrift.Features.PlayerFacade.Views
{
    using DekDev.SaveSystem;
    using UnityEngine;
    using UnityEngine.UI;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Min speed fill indicator view.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class MinSpeedFillView : GenericSelectionProviderCallback<BaseLevelItem>
    {
        [SerializeField, Min(1f)] protected float maxSpeed = 50f;
        protected Image fillImage = default;
        protected float targetFillAmount = 0f;
        protected BaseLevelItem currentLevelItem = default;
        protected LevelConditionConfig currentLevelCondition = default;

        protected virtual void Awake()
        {
            fillImage = GetComponent<Image>();
        }

        protected override void OnSelectionDataChanged(BaseLevelItem selectedItem)
        {
            currentLevelItem = selectedItem;

            UpdateFillView();
        }

        protected virtual void UpdateFillView()
        {
            currentLevelCondition = currentLevelItem.Conditions.Find(x => x.ConditionType == LevelType.MinSpeedMode);

            if (currentLevelCondition != null)
            {
                fillImage.fillAmount = currentLevelCondition.TargetValue / maxSpeed;
            }
        }
    }
}
