namespace WinterDrift.Features.QualitySettigns
{
    using UnityEngine;
    using System.Collections.Generic;
    using DekDev.CDebug;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Контролле настройки качества графики.
    /// </summary>
    public class GraphicsQualityController : MonoBehaviour
    {
        [System.Serializable]
        public class QualityPreset
        {
            public string displayName;    // Имя для отображения в UI
            public string qualityName;    // Имя уровня качества в Unity Quality Settings
            public bool useIndex = false; // Использовать индекс вместо имени
            public int qualityIndex = 0;  // Альтернатива: индекс уровня качества
        }

        [Header("Настройки качества")]
        [SerializeField]
        private QualityPreset[] qualityPresets = new QualityPreset[]
        {
        new QualityPreset { displayName = "Very Low", qualityName = "Very Low" },
        new QualityPreset { displayName = "High", qualityName = "High" },
        new QualityPreset { displayName = "Ultra", qualityName = "Ultra" }
        };

        private const string QUALITY_PREF_KEY = "GraphicsQualityKey";
        private int currentQualityIndex = 1; // По умолчанию High
        private Dictionary<string, int> qualityNameToIndexMap = new Dictionary<string, int>();

        // Событие для уведомления об изменении качества
        public System.Action<int> OnQualityChanged;

        private void Awake()
        {
            BuildQualityIndexMap();
            LoadAndApplyQualitySettings();
        }

        private void BuildQualityIndexMap()
        {
            // Создаем карту соответствия имен качеств их индексам
            string[] qualityNames = QualitySettings.names;
            for (int i = 0; i < qualityNames.Length; i++)
            {
                qualityNameToIndexMap[qualityNames[i]] = i;
            }
        }

        private void LoadAndApplyQualitySettings()
        {
            LoadQualitySettings();
            ApplyQualitySettings(currentQualityIndex, false);
        }

        /// <summary>
        /// Установить уровень качества по индексу пресета
        /// </summary>
        /// <param name="presetIndex">Индекс в массиве qualityPresets</param>
        public void SetQualityLevel(int presetIndex)
        {
            if (presetIndex >= 0 && presetIndex < qualityPresets.Length)
            {
                ApplyQualitySettings(presetIndex, true);
                SaveQualitySettings(presetIndex);
                OnQualityChanged?.Invoke(presetIndex);
            }
            else
            {
                CDebug.LogWarning($"Некорректный индекс качества: {presetIndex}. Доступно: 0-{qualityPresets.Length - 1}");
            }
        }

        /// <summary>
        /// Установить уровень качества по имени пресета
        /// </summary>
        /// <param name="presetName">Имя пресета (displayName)</param>
        public void SetQualityLevel(string presetName)
        {
            for (int i = 0; i < qualityPresets.Length; i++)
            {
                if (qualityPresets[i].displayName.Equals(presetName, System.StringComparison.OrdinalIgnoreCase))
                {
                    SetQualityLevel(i);
                    return;
                }
            }

            CDebug.LogWarning($"Пресет качества '{presetName}' не найден");
        }

        /// <summary>
        /// Получить текущий индекс качества
        /// </summary>
        public int GetCurrentQualityIndex()
        {
            return currentQualityIndex;
        }

        /// <summary>
        /// Получить текущее имя качества
        /// </summary>
        public string GetCurrentQualityName()
        {
            if (currentQualityIndex >= 0 && currentQualityIndex < qualityPresets.Length)
            {
                return qualityPresets[currentQualityIndex].displayName;
            }
            return "Unknown";
        }

        /// <summary>
        /// Получить количество доступных уровней качества
        /// </summary>
        public int GetQualityLevelsCount()
        {
            return qualityPresets.Length;
        }

        /// <summary>
        /// Получить имя уровня качества по индексу
        /// </summary>
        public string GetQualityName(int index)
        {
            if (index >= 0 && index < qualityPresets.Length)
            {
                return qualityPresets[index].displayName;
            }
            return "Invalid Index";
        }

        /// <summary>
        /// Сбросить настройки качества к значению по умолчанию
        /// </summary>
        public void ResetToDefault()
        {
            SetQualityLevel(1); // High как качество по умолчанию
        }

        private void ApplyQualitySettings(int presetIndex, bool logMessage = true)
        {
            QualityPreset preset = qualityPresets[presetIndex];
            int targetQualityIndex = -1;

            // Определяем какой индекс качества использовать
            if (preset.useIndex)
            {
                // Используем явно указанный индекс
                if (preset.qualityIndex >= 0 && preset.qualityIndex < QualitySettings.names.Length)
                {
                    targetQualityIndex = preset.qualityIndex;
                }
            }
            else
            {
                // Ищем по имени
                if (qualityNameToIndexMap.ContainsKey(preset.qualityName))
                {
                    targetQualityIndex = qualityNameToIndexMap[preset.qualityName];
                }
            }

            // Применяем настройки качества
            if (targetQualityIndex != -1)
            {
                QualitySettings.SetQualityLevel(targetQualityIndex, true);
                currentQualityIndex = presetIndex;

                if (logMessage)
                {
                    CDebug.Log($"Качество графики установлено: {preset.displayName} " +
                             $"(Unity: {QualitySettings.names[targetQualityIndex]})");
                }
            }
            else
            {
                CDebug.LogWarning($"Не удалось найти уровень качества: {preset.qualityName}");

                // Пытаемся использовать индекс как запасной вариант
                if (presetIndex >= 0 && presetIndex < QualitySettings.names.Length)
                {
                    QualitySettings.SetQualityLevel(presetIndex, true);
                    currentQualityIndex = presetIndex;
                    if (logMessage)
                    {
                        CDebug.Log($"Использован индекс по умолчанию: {presetIndex}");
                    }
                }
            }
        }

        private void SaveQualitySettings(int presetIndex)
        {
            PlayerPrefs.SetInt(QUALITY_PREF_KEY, presetIndex);
            PlayerPrefs.Save();
        }

        private void LoadQualitySettings()
        {
            if (PlayerPrefs.HasKey(QUALITY_PREF_KEY))
            {
                int savedQuality = PlayerPrefs.GetInt(QUALITY_PREF_KEY);
                if (savedQuality >= 0 && savedQuality < qualityPresets.Length)
                {
                    currentQualityIndex = savedQuality;
                }
            }
        }

        /// <summary>
        /// Автоматическое определение доступных уровней качества по именам
        /// </summary>
        public void AutoDetectQualityLevels()
        {
            string[] allQualityLevels = QualitySettings.names;
            List<QualityPreset> detectedPresets = new List<QualityPreset>();

            // Ищем наши целевые уровни в доступных настройках
            foreach (var targetName in new string[] { "Very Low", "High", "Ultra" })
            {
                bool found = false;
                for (int i = 0; i < allQualityLevels.Length; i++)
                {
                    if (allQualityLevels[i].ToLower().Contains(targetName.ToLower()))
                    {
                        detectedPresets.Add(new QualityPreset
                        {
                            displayName = targetName,
                            qualityName = allQualityLevels[i]
                        });
                        found = true;
                        break;
                    }
                }

                // Если не нашли по имени, используем индексы
                if (!found)
                {
                    int index = targetName switch
                    {
                        "Very Low" => 0,
                        "High" => 3,
                        "Ultra" => 5,
                        _ => 0
                    };

                    if (index < allQualityLevels.Length)
                    {
                        detectedPresets.Add(new QualityPreset
                        {
                            displayName = targetName,
                            qualityName = allQualityLevels[index],
                            useIndex = true,
                            qualityIndex = index
                        });
                    }
                }
            }

            if (detectedPresets.Count > 0)
            {
                qualityPresets = detectedPresets.ToArray();
                CDebug.Log($"Автоматически определены уровни качества: {detectedPresets.Count}");
            }
        }
    }
}