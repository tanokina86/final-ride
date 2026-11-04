#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FolderGenerator
{
    [MenuItem("Final Ride/Generate Project Structure")]
    public static void GenerateStructure()
    {
        // Главный корень проекта (символ '_' поднимает папку наверх списка)
        string root = Path.Combine(Application.dataPath, "_Project");

        // Список всех необходимых папок по нашей архитектуре
        string[] folders = new string[]
        {
            "Core",
            "Core/Scripts",
            "Gameplay",
            "Gameplay/Interfaces",
            "Gameplay/Vehicles",
            "Gameplay/Vehicles/Scripts",
            "Gameplay/Vehicles/Scripts/Base",
            "Gameplay/Vehicles/Prefabs",
            "Gameplay/Weapons",
            "Gameplay/Weapons/Scripts",
            "Gameplay/Weapons/Scripts/Base",
            "Gameplay/Weapons/Scripts/Projectiles",
            "Gameplay/Weapons/Prefabs",
            "SO",
            "SO/Weapons",
            "SO/Vehicles",
            "Resources",
            "Resources/Sprites",
            "Resources/Audio",
            "Scenes",
            "Settings",
            "UI"
        };

        foreach (string folder in folders)
        {
            string path = Path.Combine(root, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("[Final Ride] Структура папок успешно сгенерирована внутри Assets/_Project/!");
    }
}
#endif
