namespace DekDev.WindowAgregator
{
    using UnityEngine;

    /// <summary>
    /// Lock/unlock cursor on window. 
    /// </summary>
    public class CursorWindow : MonoBehaviour
    {
        [SerializeField] protected bool lockCursorOnDisable = true;
        protected CursorLockMode previousCursorLockState;

        protected virtual void OnEnable()
        {
            previousCursorLockState = Cursor.lockState;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        protected virtual void OnDisable()
        {
            if (lockCursorOnDisable)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = previousCursorLockState;

                if (previousCursorLockState == CursorLockMode.None)
                {
                    Cursor.visible = true;
                }
            }
        }
    }
}