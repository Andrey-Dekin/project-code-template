namespace WinterDrift.Features.Cameras
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DekDev.Extensions.Coroutines;
    using DekDev.WindowAgregator;
    using UnityEngine;

    /// <summary>
    /// Change position and rotation camera on change window.
    /// </summary>
    public class CameraWindowTransition : MonoBehaviour
    {
        [Serializable]
        public class CameraTransition
        {
            public string windowId = default;
            public Vector3 targetPosition = Vector3.zero;
            public Vector3 targetRotation = Vector3.zero;
        }

        [SerializeField, Header("Settings:")]
        [Min(0f)] protected float delay = 0f;
        [SerializeField, Min(0f)] protected float duration = 1f;

        [SerializeField, Header("Settings for target windows:")]
        protected List<CameraTransition> cameraTransitions = new List<CameraTransition>();

        protected CoroutineExtension coroutineExtension = default;
        protected Vector3 initialPosition = Vector3.zero;
        protected Vector3 initialRotation = Vector3.zero;

        protected float t = 0f;
        protected float newX = 0f;
        protected float newY = 0f;
        protected float newZ = 0f;

        protected virtual void Awake()
        {
            coroutineExtension = new CoroutineExtension(this);
            initialPosition = transform.position;
            initialRotation = transform.localEulerAngles;
        }

        protected virtual void OnEnable() => WindowAgregator.onWindowChanged += OnWindowChanged;

        protected virtual void OnDisable()
        {
            WindowAgregator.onWindowChanged -= OnWindowChanged;

            StopTransition();
        }

        /// <summary>
        /// Stop transition.
        /// </summary>
        [ContextMenu("Stop Transition")]
        public virtual void StopTransition()
        {
            if (coroutineExtension != null)
            {
                coroutineExtension.Coroutine = null;
            }
        }

        /// <summary>
        /// Reset camera in init position.
        /// </summary>
        [ContextMenu("Reset Camera")]
        public virtual void ResetCamera()
        {
            StopTransition();
            transform.position = initialPosition;
            transform.localEulerAngles = initialRotation;
        }

        /// <summary>
        /// Play target window transition.
        /// </summary>
        public virtual void PlayTransition(string windowId)
        {
            CameraTransition transition = cameraTransitions.Find(x => x.windowId == windowId);

            if (transition != null)
            {
                coroutineExtension.Coroutine = StartCoroutine(TransitionRoutine(transition.targetPosition, transition.targetRotation));
            }
        }
        protected virtual void OnWindowChanged(string windowId)
        {
            CameraTransition transition = cameraTransitions.Find(x => x.windowId == windowId);

            if (transition != null)
            {
                coroutineExtension.Coroutine = StartCoroutine(TransitionRoutine(transition.targetPosition, transition.targetRotation));
            }
        }

        protected virtual IEnumerator TransitionRoutine(Vector3 targetPosition, Vector3 targetRotation)
        {
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }

            Vector3 startPosition = transform.position;
            Vector3 startRotation = transform.localEulerAngles;

            float elapsed = 0f;

            while (isActiveAndEnabled && elapsed < duration)
            {
                elapsed += Time.deltaTime;
                t = Mathf.Clamp01(elapsed / duration);

                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                newX = Mathf.LerpAngle(startRotation.x, targetRotation.x, t);
                newY = Mathf.LerpAngle(startRotation.y, targetRotation.y, t);
                newZ = Mathf.LerpAngle(startRotation.z, targetRotation.z, t);
                transform.localEulerAngles = new Vector3(newX, newY, newZ);

                yield return null;
            }

            transform.position = targetPosition;
            transform.localEulerAngles = targetRotation;
        }
    }
}