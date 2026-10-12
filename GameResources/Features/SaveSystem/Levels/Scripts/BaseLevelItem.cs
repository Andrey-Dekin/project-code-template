namespace WinterDrift.Features.SaveSystem.Levels
{
    using System;
    using System.Collections.Generic;
    using DekDev.SaveSystem;
    using UnityEngine;

#if PlayerStats_yg && UNITY_WEBGL
    using YG;
#endif

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Базовая дата для уровня.
    /// </summary>
    [CreateAssetMenu(menuName = "WinterDrift/SaveSystem/Levels/" + nameof(BaseLevelItem))]
    public class BaseLevelItem : SelectionItem
    {
        protected string COMPLETED_KEY = "IsCompleted";
        protected string BEST_SCORE_KEY = "BestScore";
        protected string BEST_TIME_KEY = "BestTime";
        protected string BEST_CLIPPING_ZONE_KEY = "BestClippingZone";
        protected string STARS_KEY = "CompleteStarCount";
        protected string RATING_KEY = "LevelRatingPoints";
        protected string UNLOCKED_KEY = "LevelUnlocked";

        /// <summary>
        /// Событие пройденного уровня.
        /// </summary>
        public event Action onCompleted = delegate { };

        /// <summary>
        /// Изменен лучший показатель очков.
        /// </summary>
        public event Action onBestScoreChanged = delegate { };

        /// <summary>
        /// Изменен лучший показатель времени.
        /// </summary>
        public event Action onBestTimeChanged = delegate { };

        /// <summary>
        /// Изменен лучший показатель собранных клиппинг зон.
        /// </summary>
        public event Action onBestClippingZoneChanged = delegate { };

        /// <summary>
        /// Изменен лучший показатель собранных клиппинг зон.
        /// </summary>
        public event Action onStarsChanged = delegate { };

        /// <summary>
        /// Изменен лучший показатель рейтинга.
        /// </summary>
        public event Action onRatingChanged = delegate { };

        /// <summary>
        /// Уровень разблокирован.
        /// </summary>
        public event Action onLockStateChanged = delegate { };

        /// <summary>
        /// Ключ локализации названия уровня.
        /// </summary>
        public string NameKey => nameKey;

        /// <summary>
        /// Дата гоночного трека.
        /// </summary>
        public TrackData Track => track;

        /// <summary>
        /// Префаб уровня с триггерами и спаунером авто.
        /// </summary>
        public GameObject LevelPrefab => levelPrefab;

        /// <summary>
        /// Пройден ли уровень.
        /// </summary>
        public bool IsCompleted
        {
            get => GetSavedIntegerValue(Id + COMPLETED_KEY) != 0;

            protected set
            {
                SaveIntegerValue(Id + COMPLETED_KEY, value ? 1 : 0);

                if (value)
                {
                    onCompleted();
                }
            }
        }

        /// <summary>
        /// Лучший показатель по очкам.
        /// </summary>
        public int BestScore
        {
            get => GetSavedIntegerValue(Id + BEST_SCORE_KEY);

            protected set
            {
                SaveIntegerValue(Id + BEST_SCORE_KEY, value);

                onBestScoreChanged();
            }
        }

        /// <summary>
        /// Лучший показатель по времени.
        /// </summary>
        public TimeSpan BestTime
        {
            get => TimeSpan.FromSeconds(GetSavedIntegerValue(Id + BEST_TIME_KEY));

            protected set
            {
                SaveIntegerValue(Id + BEST_TIME_KEY, (int)value.TotalSeconds);

                onBestTimeChanged();
            }
        }

        /// <summary>
        /// Лучший показатель по собранным клиппинг зонам.
        /// </summary>
        public int BestClippingZone
        {
            get => GetSavedIntegerValue(Id + BEST_CLIPPING_ZONE_KEY);

            protected set
            {
                SaveIntegerValue(Id + BEST_CLIPPING_ZONE_KEY, value);

                onBestClippingZoneChanged();
            }
        }

        /// <summary>
        /// Количество звезд набранных за выполнение заданий уровня.
        /// </summary>
        public int Stars
        {
            get => GetSavedIntegerValue(Id + STARS_KEY);

            protected set
            {
                SaveIntegerValue(Id + STARS_KEY, value);

                onStarsChanged();
            }
        }

        /// <summary>
        /// Количество очков рейтинга набранных за выполнение заданий уровня.
        /// </summary>
        public int Rating
        {
            get => GetSavedIntegerValue(Id + RATING_KEY);

            protected set
            {
                SaveIntegerValue(Id + RATING_KEY, value);

                onRatingChanged();
            }
        }

        /// <summary>
        /// Доступность уровня.
        /// </summary>
        public bool IsUnlocked
        {
            get => GetSavedIntegerValue(Id + UNLOCKED_KEY) == 1;

            set
            {
                SaveIntegerValue(Id + UNLOCKED_KEY, value ? 1 : 0);

                onLockStateChanged();
            }
        }

        /// <summary>
        /// Условия выполнения уровня.
        /// </summary>
        public List<LevelConditionConfig> Conditions => conditions;

        /// <summary>
        /// Количество уровней которые нужно пройти для разблокировки текущего.
        /// </summary>
        public int TargetCompletedLevelsCount => targetCompletedLevelsCount;

        [SerializeField] protected string nameKey = string.Empty;
        [SerializeField] protected TrackData track = default;
        [SerializeField] protected GameObject levelPrefab = default;
        [SerializeField] protected int targetCompletedLevelsCount = 0;
        [Header("Сonditions for completing the level")]
        [SerializeField] protected List<LevelConditionConfig> conditions = new List<LevelConditionConfig>();

        /// <summary>
        /// Установить статус уровню.
        /// </summary>
        public void SetCompletedStatus(bool status) => IsCompleted = status;

        /// <summary>
        /// Установка лучшего значения по очкам с проверкой.
        /// </summary>
        public void TrySetBestScore(int newScore)
        {
            if (newScore > BestScore)
            {
                Debug.Log($"Set best drift score {BestScore} -> {newScore}");

                BestScore = newScore;
            }
        }

        /// <summary>
        /// Установка лучшего значения по очкам с проверкой.
        /// </summary>
        public void TrySetBestTime(TimeSpan newTime)
        {
            if (newTime.TotalSeconds > 0 && (newTime < BestTime || BestTime == TimeSpan.FromSeconds(0)))
            {
                Debug.Log($"Set best time score {BestTime} -> {newTime}");

                BestTime = newTime;
            }
        }

        /// <summary>
        /// Установка лучшего значения по очкам с проверкой.
        /// </summary>
        public void TrySetBestClippingZone(int newScore)
        {
            if (newScore > BestClippingZone)
            {
                Debug.Log($"Set best clipping zone score {BestClippingZone} -> {newScore}");

                BestClippingZone = newScore;
            }
        }

        /// <summary>
        /// Установка лучшего значения по очкам с проверкой.
        /// </summary>
        public void TrySetStars(int newValue)
        {
            if (newValue > Stars)
            {
                Debug.Log($"Set stars for complete level {Stars} -> {newValue}");

                Stars = newValue;
            }
        }

        /// <summary>
        /// Установка лучшего значения по очкам с проверкой.
        /// </summary>
        public void TrySetRating(int newValue)
        {
            if (newValue > Rating)
            {
                Debug.Log($"Set rating for complete level {Rating} -> {newValue}");

                Rating = newValue;
            }
        }

        /// <summary>
        /// Сбросить прогресс уровня.
        /// </summary>
        public void ResetProgress()
        {
            IsCompleted = true; //оставляю уровень пройденным для тестирования контента
            BestScore = 0;
            BestTime = TimeSpan.Zero;
            BestClippingZone = 0;
            Stars = 0;
            Rating = 0;
            IsUnlocked = true; //оставляю уровень разблокированным для тестирования контента
        }

        protected virtual int GetSavedIntegerValue(string saveKey)
        {
#if PlayerStats_yg && UNITY_WEBGL
            return YG2.GetState(saveKey);
#else
            return PlayerPrefs.GetInt(saveKey, 0);
#endif
        }

        protected virtual void SaveIntegerValue(string saveKey, int saveValue)
        {
#if PlayerStats_yg && UNITY_WEBGL
            YG2.SetState(saveKey, saveValue);
#else
            PlayerPrefs.SetInt(saveKey, saveValue);
            PlayerPrefs.Save();
#endif
        }
    }
}