namespace WinterDrift.Features.UI
{
    using System.Collections;
    using DekDev.Extensions.Coroutines;
    using UnityEngine;

    /// <summary>
    /// Check and lock cursor when windows open.
    /// </summary>
    public sealed class LockCursorChecker : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float _checkTimeInterval = 3f;
        [SerializeField] private ChangeCursorLockState _lockStateChanger = default;
        [SerializeField] private bool _isAlwaysLock = true;
        private CoroutineExtension _coroutineExtension = default;
        private WaitForSeconds waitCheck = default;

        private void Awake()
        {
            _coroutineExtension = new CoroutineExtension(this);
            waitCheck = new WaitForSeconds(_checkTimeInterval);
        }

        private void OnEnable()
        {
            _coroutineExtension.Coroutine = StartCoroutine(CheckRoutine());
        }

        private void OnDisable()
        {
            _coroutineExtension.Coroutine = null;
        }

        private IEnumerator CheckRoutine()
        {
            while (isActiveAndEnabled)
            {
                yield return waitCheck;

                if (_isAlwaysLock)
                {
                    _lockStateChanger.LockCursor();
                }
                else
                {
                    _lockStateChanger.UnlockCursor();
                }
            }
        }
    }
}