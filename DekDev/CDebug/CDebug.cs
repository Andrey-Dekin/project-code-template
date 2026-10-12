namespace DekDev.CDebug
{
    using UnityEngine;

    /// <summary>
    /// Дебагер для редактора.
    /// </summary>
    public class CDebug
    {
        /// <summary>
        /// Лог ошибки в консоль.
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }

        /// <summary>
        /// Лог в консоль.
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        /// <summary>
        /// Лог предупреждения в консоль.
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }
    }
}
