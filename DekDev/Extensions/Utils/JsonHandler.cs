namespace DekDev.Extensions.Utils
{
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// Обработчик json для сохранения и загрузки
    /// </summary>
    public static class JsonHandler
    {
        public static void SaveToJson<T>(T data, string filePath)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, json);
        }

        public static T LoadFromJson<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.LogWarning($"Файл не найден: {filePath}");
                return default;
            }
        }
    }
}