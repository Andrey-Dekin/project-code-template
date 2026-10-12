namespace WinterDrift.Features.SaveSystem.Cars
{
    using System;
    using DekDev.SaveSystem;
    using UnityEngine;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Дата для машины.
    /// </summary>
    [CreateAssetMenu(menuName = "WinterDrift/SaveSystem/Cars/" + nameof(CarSelectionData))]
    public class CarSelectionData : SelectionItem
    {
        protected string UNLOCKED_KEY = "CarUnlocked";

        /// <summary>
        /// Машина разблокирована.
        /// </summary>
        public event Action onLockStateChanged = delegate { };

        /// <summary>
        /// Доступность машины.
        /// </summary>
        public bool IsUnlocked
        {
            get => PlayerPrefs.GetInt(Id + UNLOCKED_KEY, 0) == 1;

            set
            {
                PlayerPrefs.SetInt(Id + UNLOCKED_KEY, value ? 1 : 0);
                PlayerPrefs.Save();

                onLockStateChanged();
            }
        }

        /// <summary>
        /// Количество уровней которые нужно пройти для разблокировки машины.
        /// </summary>
        public int TargetCompletedLevelsCount => targetCompletedLevelsCount;

        [SerializeField] protected int targetCompletedLevelsCount = 0;
    }
}