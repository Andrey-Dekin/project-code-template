namespace WinterDrift.Features.LevelTasks.Views
{
    using DekDev.WindowAgregator;
    using UnityEngine;

    /// <summary>
    /// Call result windows on level finished.
    /// </summary>
    public class LevelResultWindowCaller : MonoBehaviour
    {
        [SerializeField] protected LevelController levelController = default;
        [SerializeField] protected Window winWindowPrefab = default;
        [SerializeField] protected Window loseWindowPrefab = default;

        protected virtual void OnEnable()
        {
            if (levelController != null)
            {
                levelController.onWin += OnWin;
                levelController.onLose += OnLose;
            }
        }

        protected virtual void OnDisable()
        {
            if (levelController != null)
            {
                levelController.onWin -= OnWin;
                levelController.onLose -= OnLose;
            }
        }

        protected virtual void OnWin()
        {
            if (winWindowPrefab != null)
            {
                WindowAgregator.onWindowOpen(winWindowPrefab.Id);
            }
        }

        protected virtual void OnLose()
        {
            if (loseWindowPrefab)
            {
                WindowAgregator.onWindowOpen(loseWindowPrefab.Id);
            }
        }
    }
}