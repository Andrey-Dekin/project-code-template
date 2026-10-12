namespace DekDev.WindowAgregator
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Контроллер окон
    /// </summary>
    public class WindowAgregator : MonoBehaviour
    {
        /// <summary>
        /// Оповещает о смене текущего окна
        /// </summary>
        public static event Action<string> onWindowChanged = delegate { };

        /// <summary>
        /// Вызов открытия окна по id
        /// </summary>
        public static Action<string> onWindowOpen = delegate { };

        /// <summary>
        /// Вызов закрытия текущего окна
        /// </summary>
        public static Action onWindowClose = delegate { };

        /// <summary>
        /// Текущее активное окно
        /// </summary>
        public Window CurrentWindow
        {
            get => currentWindow;
            set
            {
                if (currentWindow != value)
                {
                    if (currentWindow != null)
                    {
                        currentWindow.IsFocused = false;
                    }
                    currentWindow = value;
                    currentWindow.IsFocused = true;
                    onWindowChanged(currentWindow.Id);
                }
            }
        }
        protected Window currentWindow = default;

        [SerializeField, Header("Префабы всех окон для сцены:")] protected List<Window> prefabsWindows = new List<Window>();
        [SerializeField, Header("Окна которые были ранее заспаунены")] protected List<Window> pooledWindows = new List<Window>();
        [SerializeField, Header("Ссылки на все открытые ранее окна:")] protected List<Window> historyWindows = new List<Window>();
        [SerializeField] protected Window startWindowPrefab = default;


        protected virtual void Awake()
        {
            onWindowOpen += OnWindowOpened;
            onWindowClose += OnWindowClosed;
            SpawnFirstWindow();
        }

        protected virtual void OnDestroy()
        {
            onWindowOpen -= OnWindowOpened;
            onWindowClose -= OnWindowClosed;
        }

        private void SpawnFirstWindow()
        {
            CurrentWindow = Instantiate(startWindowPrefab, transform);
            pooledWindows.Add(CurrentWindow);
            historyWindows.Add(CurrentWindow);
        }

        private void OnWindowOpened(string nameWindow)
        {
            CurrentWindow.gameObject.SetActive(false);
            Window res = pooledWindows.Find(x => x.Id == nameWindow);
            if (res != null)
            {
                res.gameObject.SetActive(true);
                CurrentWindow = res;
            }
            else
            {
                res = prefabsWindows.Find(x => x.Id == nameWindow);
                if (res)
                {
                    CurrentWindow = Instantiate(res, transform);
                    pooledWindows.Add(CurrentWindow);
                }
                else
                {
                    Debug.LogError($"Нет запрашиваемого окна с id {nameWindow}");
                }
            }
            historyWindows.Add(CurrentWindow);
        }

        private void OnWindowClosed()
        {
            CurrentWindow.gameObject.SetActive(false);
            historyWindows.RemoveAt(FindLastIdInHistory(CurrentWindow.Id));
            CurrentWindow = historyWindows[^1];
            CurrentWindow.gameObject.SetActive(true);
        }

        private int FindLastIdInHistory(string IdWindow)
        {
            for (int i = historyWindows.Count - 1; i > 0; i--)
            {
                if (historyWindows[i].Id == IdWindow)
                {
                    return i;
                }
            }
            return historyWindows.Count - 1;
        }
    }
}