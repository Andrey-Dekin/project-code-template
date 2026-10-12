namespace DekDev.Extensions.Objects
{
    using UnityEngine;

    /// <summary>
    /// Класс с методами расширения
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Переставить объект в нулевые координаты с нулевым поворотом
        /// </summary>
        /// <param name="target"></param>
        public static void ResetLocalPosition(this GameObject target, GameObject parent)
        {
            target.transform.SetParent(parent.transform);
            target.transform.localPosition = Vector3.zero;
            target.transform.localEulerAngles = Vector3.zero;
            target.transform.SetParent(null);
        }

        /// <summary>
        /// Сбросить скорость падения и вращения
        /// </summary>
        /// <param name="target"></param>
        public static void ResetVelocity(this GameObject target)
        {
            if (target.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        /// <summary>
        /// Сбросить размер объекта в (1,1,1)
        /// </summary>
        public static void ResetScaleOne(this GameObject target)
        {
            target.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Отбросить объект с точки в прямом направлении
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="rb"></param>
        /// <param name="point"></param>
        public static void AddForceFromEntity(this GameObject obj, Transform point, float force)
        {
            if (obj.TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(point.transform.forward * force, ForceMode.Impulse);
                obj.transform.position = point.position;
            }
        }
    }
}