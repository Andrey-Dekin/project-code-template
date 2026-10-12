namespace DekDev.Extensions.Containers.ValueContainers
{
    using System;
    using UnityEngine;
    using DekDev.CDebug;

    /// <summary>
    /// Abstract generic container of value type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractValueContainer<T> : ScriptableObject
    {
        /// <summary>
        /// Value changed event.
        /// </summary>
        public event Action onValueChanged = delegate { };

        /// <summary>
        /// Current value of target type.
        /// </summary>
        public T Value
        {
            get
            {
                if (isSaveInPrefs)
                {
                    if (!IsPrefsKeySet())
                    {
                        CDebug.LogError($"{name}: Unable to load value, PrefsKey is null");

                        return val;
                    }

                    return LoadValueFromPrefs();
                }

                return val;
            }

            set
            {
                val = value;

                if (isSaveInPrefs)
                {
                    if (!IsPrefsKeySet())
                    {
                        CDebug.LogError($"{name}: Unable to save value, PrefsKey is null");

                        onValueChanged();
                        return;
                    }

                    SaveValueInPrefs(val);
                }

                onValueChanged();
            }
        }

        [SerializeField] protected bool isSaveInPrefs = false;
        [SerializeField, Header("Can't be empty if selected save in prefs")]
        protected string prefsKey = string.Empty;
        protected T val = default;

        /// <summary>
        /// Save some type in prefs.
        /// </summary>
        /// <param name="targetValue"></param>
        protected abstract void SaveValueInPrefs(T targetValue);

        /// <summary>
        /// Load some type from prefs.
        /// </summary>
        /// <returns></returns>
        protected abstract T LoadValueFromPrefs();

        protected bool IsPrefsKeySet() => !string.IsNullOrWhiteSpace(prefsKey);
    }
}