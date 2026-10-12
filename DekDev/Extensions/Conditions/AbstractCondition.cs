namespace DekDev.Extensions.Conditions
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Абстрактный кондишен.
    /// </summary>
    public abstract class AbstractCondition : ScriptableObject
    {
        /// <summary>
        /// Событие смены статуса кондишена.
        /// </summary>
        public event Action onConditionStatusChanged = delegate { };

        /// <summary>
        /// Проверка выполнения условия.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsConditionMet();

        /// <summary>
        /// Установка значения статуса кондишена.
        /// </summary>
        /// <param name="status"></param>
        public abstract void SetConditionStatus(bool status);

        protected void OnConditionStatusChange() => onConditionStatusChanged();
    }
}
