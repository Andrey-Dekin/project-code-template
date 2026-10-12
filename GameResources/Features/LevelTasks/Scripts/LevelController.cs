namespace WinterDrift.Features.LevelTasks
{
    using UnityEngine;
    using WinterDrift.Features.SaveSystem.Levels;
    using DekDev.CDebug;
    using System;
    using DekDev.SaveSystem;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Level task controller.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        /// <summary>
        /// Level controller initializated event.
        /// </summary>
        public event Action onLevelInited = delegate { };

        /// <summary>
        /// Lose event.
        /// </summary>
        public event Action onLose = delegate { };

        /// <summary>
        /// Win event.
        /// </summary>
        public event Action onWin = delegate { };

        /// <summary>
        /// Level controller initializated.
        /// </summary>
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Current active level.
        /// </summary>
        public BaseLevelItem CurrentLevel => currentLevel;

        /// <summary>
        /// Conditions for all game modes.
        /// </summary>
        public List<AbstractLevelCondition> Conditions => conditions;

        [SerializeField] protected SavableSelectionInstance saveControllerContainer = default;
        [SerializeField] protected List<AbstractLevelCondition> conditions = new List<AbstractLevelCondition>();
        protected BaseLevelItem currentLevel = default;
        protected bool isInitialized = false;
        protected bool isWaitingForSaveController = false;
        protected bool isAllConditionsMet = false;
        protected bool isLevelFinished = false;

        #region Initialization
        /// <summary>
        /// Initialization.
        /// </summary>
        public virtual void Initialize()
        {
            if (saveControllerContainer == null)
            {
                CDebug.LogError("SaveControllerContainer is Null!");
                return;
            }

            if (!saveControllerContainer.IsInited)
            {
                WaitForSaveControllerInitialization();
                return;
            }

            CompleteInitialization();
        }

        protected virtual void CompleteInitialization()
        {
            isLevelFinished = false;
            currentLevel = GetCurrentLevelItem();

            if (currentLevel == null)
            {
                CDebug.LogError("Can't get current level item - " + nameof(BaseLevelItem));
                return;
            }

            InitializeConditions();

            isInitialized = true;

            onLevelInited();
        }

        protected void WaitForSaveControllerInitialization()
        {
            isWaitingForSaveController = true;
            saveControllerContainer.onInstanceInited += HandleSaveControllerInitialized;
        }

        protected void HandleSaveControllerInitialized()
        {
            saveControllerContainer.onInstanceInited -= HandleSaveControllerInitialized;
            isWaitingForSaveController = false;

            Initialize();
        }

        protected void InitializeConditions()
        {
            isAllConditionsMet = false;

            conditions.ForEach(x => x.Initialize(this));

            conditions.RemoveAll(x => !x.IsInProgress);

            SubscribeConditions();
        }
        #endregion

        /// <summary>
        /// Set best score in level data.
        /// </summary>
        /// <param name="bestScore"></param>
        public void SetBestScore(int bestScore) => currentLevel.TrySetBestScore(bestScore);

        /// <summary>
        /// Set best time in level data.
        /// </summary>
        /// <param name="bestTime"></param>
        public void SetBestTime(TimeSpan bestTime) => currentLevel.TrySetBestTime(bestTime);

        /// <summary>
        /// Set best clipping zone score in level data.
        /// </summary>
        /// <param name="bestScore"></param>
        public void SetBestClippingZoneScore(int bestScore) => currentLevel.TrySetBestClippingZone(bestScore);

        /// <summary>
        /// Player finished action.
        /// </summary>
        public void Finish()
        {
            if (isLevelFinished) { return; }

            UnsubscribeConditions();

            conditions.ForEach(x => x.UpdateConditionMet());

            isAllConditionsMet = IsAllConditionsMet();

            if (isAllConditionsMet)
            {
                OnWin();
            }
            else
            {
                OnLose();
            }
        }

        protected virtual void Start() => Initialize();

        protected virtual void OnDisable()
        {
            if (isWaitingForSaveController && saveControllerContainer != null)
            {
                saveControllerContainer.onInstanceInited -= HandleSaveControllerInitialized;
                isWaitingForSaveController = false;
            }

            UnsubscribeConditions();
        }

        protected void SubscribeConditions() => conditions.ForEach(x => x.onConditionStatusChanged += CheckConditionsMet);

        protected void UnsubscribeConditions() => conditions.ForEach(x => x.onConditionStatusChanged -= CheckConditionsMet);

        protected void CheckConditionsMet()
        {
            conditions.ForEach(x =>
            {
                if (x.IsConditionMet && !x.IsInProgress)
                {
                    x.onConditionStatusChanged -= CheckConditionsMet;
                }
            });

            isAllConditionsMet = IsAllConditionsMet();

            if (isAllConditionsMet)
            {
                OnWin();
            }
        }

        protected void OnLose()
        {
            UnsubscribeConditions();

            isLevelFinished = true;

            CDebug.Log("Level lost");

            onLose();
        }

        protected void OnWin()
        {
            UnsubscribeConditions();

            SetCompletedStatus(true);

            conditions.ForEach(x => x.CallUpdateBestScore());
            UpdateLevelStars();

            isLevelFinished = true;

            CDebug.Log("Level win");

            onWin();
        }

        protected bool IsAllConditionsMet() => conditions.TrueForAll(x => x.IsConditionMet);

        protected void SetCompletedStatus(bool isCompleted) => currentLevel.SetCompletedStatus(isCompleted);

        protected BaseLevelItem GetCurrentLevelItem() =>
    (saveControllerContainer != null && saveControllerContainer.IsInited) ?
         (saveControllerContainer.Instance.GetCurrentSelection() as BaseLevelItem) : null;

        protected void UpdateLevelStars()
        {
            AbstractLevelCondition[] ratedConditions = conditions.FindAll(x => x is IRatedByStars).ToArray();

            if (ratedConditions != null)
            {
                List<int> currentStars = new List<int>();

                foreach (AbstractLevelCondition ratedCondition in ratedConditions)
                {
                    currentStars.Add(ratedCondition.StarsForCondition);
                }

                currentLevel.TrySetStars(currentStars.Min());
            }
        }
    }
}
