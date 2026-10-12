namespace WinterDrift.Features.SaveSystem.Levels
{
    using System.Linq;
    using DekDev.CDebug;
    using DekDev.SaveSystem;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Next level button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class NextLevelButton : SelectionProviderCallback
    {
        [SerializeField] protected SelectionDatabase database = default;
        [SerializeField] protected SavableSelectionInstance saveControllerContainer = default;
        protected SelectionItem currentLevel = default;
        protected Button buttonView = default;
        protected bool isButtonClicked = false;

        protected virtual void Awake() => buttonView = GetComponent<Button>();

        protected override void OnEnable()
        {
            base.OnEnable();

            buttonView.onClick.AddListener(OnButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            buttonView.onClick.RemoveListener(OnButtonClick);
        }

        protected override void OnSelectionDataChanged(SelectionItem selectedItem)
        => currentLevel = selectedItem;

        /// <summary>
        /// Last unlocked and not completed or random.
        /// </summary>
        protected virtual void OnButtonClick()
        {
            if (isButtonClicked) return;

            int currentLevelIndex = database.Items.ToList().IndexOf(currentLevel);

            if (currentLevelIndex < 0)
            {
                CDebug.LogError("Can't find index of current level in DB");
                return;
            }

            if (currentLevelIndex + 1 < database.Items.Length)
            {
                BaseLevelItem nextLevel = database.Items[currentLevelIndex + 1] as BaseLevelItem;

                if (nextLevel != null && nextLevel.IsUnlocked)
                {
                    saveControllerContainer.Instance.Select(nextLevel.Id);
                }
                else
                {
                    nextLevel = database.Items.LastOrDefault(x => x is BaseLevelItem levelItem && levelItem.IsUnlocked && !levelItem.IsCompleted) as BaseLevelItem;

                    saveControllerContainer.Instance.Select(nextLevel != null ? nextLevel.Id : database.Items[Random.Range(0, database.Items.Length)].Id);
                }

                CDebug.Log("Selected next level: " + nextLevel.Id);
            }


            isButtonClicked = true;
        }
    }
}