using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalRide.Gameplay.Vehicles;
using Assets.Scripts.Enums;

namespace FinalRide.Gameplay.Level
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Prefabs Pool")]
        [SerializeField] private List<GameObject> enemyPrefabs; // Сюда закинем префабы Specter, Thug, Crawler

        [Header("Spawn Settings")]
        [SerializeField] private float spawnInterval = 3f;      // Раз во сколько секунд прилетает новый враг
        [SerializeField] private float spawnOffsetY = 8f;       // На сколько юнитов выше игрока спавнить врага (за экраном)
        [SerializeField] private float roadWidth = 3f;          // Ширина дороги для случайного разброса по оси X

        private Transform _playerTransform;
        private bool _canSpawn = true;

        private void Start()
        {
            // Находим игрока на сцене, чтобы спавнить врагов строго перед ним
            var playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                _playerTransform = playerComponent.transform;
            }

            // Запускаем бесконечный цикл генерации волн врагов
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            // Ждем секунду после старта игры, чтобы игрок успел разогнаться
            yield return new WaitForSeconds(1f);

            while (_canSpawn)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnEnemy()
        {
            if (enemyPrefabs == null || enemyPrefabs.Count == 0 || _playerTransform == null) return;

            // 1. Выбираем случайного врага из доступного пула префабов
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // 2. Рассчитываем случайную позицию по ширине дороги, но строго выше игрока (за экраном)
            float randomX = Random.Range(-roadWidth, roadWidth);
            float spawnY = _playerTransform.position.y + spawnOffsetY;
            Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

            // 3. Физически создаем врага на сцене
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0f, 0f, 180f)); // Разворачиваем лицом к игроку (на 180 град)
            newEnemy.gameObject.SetActive(true);

            Debug.Log($"[Spawner] На трассу вылетел враг: {newEnemy.name}");
        }

        /// <summary>
        /// Публичный метод для Режиссера уровня (LevelDirector), чтобы ставить спавн на паузу перед Боссом
        /// </summary>
        public void SetSpawnActive(bool active)
        {
            _canSpawn = active;
            if (active)
            {
                StartCoroutine(SpawnRoutine());
            }
            else
            {
                StopAllCoroutines();
            }
        }
    }
}