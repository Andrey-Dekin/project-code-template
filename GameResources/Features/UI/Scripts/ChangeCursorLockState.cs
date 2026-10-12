namespace WinterDrift.Features.UI
{
    using UnityEngine;

    /// <summary>
    /// Change cursor lock state.
    /// </summary>
    public class ChangeCursorLockState : MonoBehaviour
    {
        public void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}