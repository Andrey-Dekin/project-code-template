namespace DekDev.Extensions.Coroutines
{
    using UnityEngine;

    /// <summary>
    /// Расширение для корутины.
    /// </summary>
    public class CoroutineExtension
    {
        /// <summary>
        /// Класс владеющий текущей корутиной.
        /// </summary>
        public MonoBehaviour Owner => owner;

        /// <summary>
        /// Корутина.
        /// </summary>
        public Coroutine Coroutine
        {
            get => coroutine;

            set
            {
                if (coroutine != null)
                {
                    owner.StopCoroutine(coroutine);
                }

                coroutine = value;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="owner"></param>
        public CoroutineExtension(MonoBehaviour owner) => this.owner = owner;

        protected Coroutine coroutine = default;
        protected readonly MonoBehaviour owner;
    }
}
