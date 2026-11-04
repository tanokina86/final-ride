using System.Collections.Generic;
using UnityEngine;

namespace FinalRide.Gameplay.Level
{
    public class RoadGenerator : MonoBehaviour
    {
        [Header("Road Templates (Tiles)")]
        [SerializeField] private List<GameObject> roadPrefabs;
        [SerializeField] private GameObject starterRoadPrefab;

        [Header("Obstacle Settings")]
        [SerializeField] private List<GameObject> obstaclePrefabs;
        [Range(0f, 1f)]
        [SerializeField] private float obstacleSpawnChance = 0.4f;

        [Header("Movement Settings")]
        [SerializeField] private float scrollSpeed = 5f;

        [Header("Generator Tuning")]
        [SerializeField] private int maxTilesOnScreen = 4;
        [SerializeField] private float tileLength = 10f;

        private List<GameObject> _activeTiles = new List<GameObject>();

        private bool _isForkActive = false;
        private Transform _leftBranchAnchor;
        private Transform _rightBranchAnchor;

        public float ScrollSpeed { get => scrollSpeed; set => scrollSpeed = value; }

        private void Start()
        {
            if (starterRoadPrefab != null)
            {
                SpawnTile(starterRoadPrefab);
            }
            else if (roadPrefabs != null && roadPrefabs.Count > 0)
            {
                // Fixes list-to-object mismatch by safely pulling the first index
                SpawnTile(roadPrefabs[0]);
            }

            for (int i = 1; i < maxTilesOnScreen; i++)
            {
                SpawnRandomTile();
            }
        }


        private void Update()
        {
            MoveRoad();
            CheckAndRecycleTiles();
        }

        private void MoveRoad()
        {
            foreach (GameObject tile in _activeTiles)
            {
                tile.transform.Translate(Vector3.down * (scrollSpeed * Time.deltaTime));
            }
        }

        /// <summary>
        /// Selects a random tile prefab from the pool and triggers spawn logic.
        /// </summary>
        public void SpawnRandomTile()
        {
            if (roadPrefabs == null || roadPrefabs.Count == 0) return;

            int randomIndex = Random.Range(0, roadPrefabs.Count);
            SpawnTile(roadPrefabs[randomIndex]);
        }

        private void SpawnTile(GameObject tilePrefab)
        {
            Vector3 spawnPosition = transform.position;

            if (_activeTiles.Count > 0)
            {
                GameObject lastTile = _activeTiles[_activeTiles.Count - 1];
                SpriteRenderer roadRenderer = lastTile.GetComponent<SpriteRenderer>();

                if (roadRenderer == null)
                {
                    roadRenderer = lastTile.GetComponentInChildren<SpriteRenderer>();
                }

                // Calculate the strict top visual edge of the previous sprite asset
                if (roadRenderer != null)
                {
                    spawnPosition.y = roadRenderer.bounds.max.y;
                }
                else
                {
                    spawnPosition.y = lastTile.transform.position.y + tileLength;
                }
            }

            GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);

            SpriteRenderer newRoadRenderer = newTile.GetComponent<SpriteRenderer>();
            if (newRoadRenderer == null)
            {
                newRoadRenderer = newTile.GetComponentInChildren<SpriteRenderer>();
            }

            // Align the bottom visual edge of the incoming sprite seamlessly with the target position
            if (newRoadRenderer != null && _activeTiles.Count > 0)
            {
                float visualPivotOffset = newTile.transform.position.y - newRoadRenderer.bounds.min.y;
                newTile.transform.position += new Vector3(0f, visualPivotOffset, 0f);
            }

            _activeTiles.Add(newTile);
            PopulateTileWithObstacles(newTile);
        }

        private void PopulateTileWithObstacles(GameObject tile)
        {
            if (obstaclePrefabs == null || obstaclePrefabs.Count == 0) return;

            foreach (Transform child in tile.transform)
            {
                if (child.name == "ObstaclePoint")
                {
                    if (Random.value <= obstacleSpawnChance)
                    {
                        int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
                        GameObject obstacle = Instantiate(obstaclePrefabs[randomObstacleIndex], child.position, Quaternion.identity, tile.transform);
                        obstacle.gameObject.SetActive(true);
                    }
                }
            }
        }

        /// <summary>
        /// Spawns a fork tile segment and prepares connected path exits.
        /// </summary>
        /// <param name="forkPrefab">The prefab containing multi-exit points.</param>
        public void SpawnForkTile(GameObject forkPrefab)
        {
            Vector3 spawnPosition = transform.position;

            if (_activeTiles.Count > 0)
            {
                GameObject lastTile = _activeTiles[_activeTiles.Count - 1];
                SpriteRenderer roadRenderer = lastTile.GetComponentInChildren<SpriteRenderer>();
                if (roadRenderer != null)
                {
                    spawnPosition.y = roadRenderer.bounds.max.y;
                }
            }

            GameObject newFork = Instantiate(forkPrefab, spawnPosition, Quaternion.identity, transform);
            _activeTiles.Add(newFork);

            Transform exitLeft = newFork.transform.Find("Exit_Left") ?? newFork.transform.Find("Exit_L");
            Transform exitRight = newFork.transform.Find("Exit_Right") ?? newFork.transform.Find("Exit_R");

            if (exitLeft != null && exitRight != null)
            {
                // Unified world coordinate translation to prevent tiles from flying sideways
                Vector3 worldLeft = newFork.transform.TransformPoint(exitLeft.localPosition);
                Vector3 worldRight = newFork.transform.TransformPoint(exitRight.localPosition);

                SpawnTileAtPosition(roadPrefabs[Random.Range(0, roadPrefabs.Count)], worldLeft);
                SpawnTileAtPosition(roadPrefabs[Random.Range(0, roadPrefabs.Count)], worldRight);
            }
        }


        private void SpawnTileAtPosition(GameObject tilePrefab, Vector3 position)
        {
            GameObject branchTile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
            _activeTiles.Add(branchTile);
        }


        private void CheckAndRecycleTiles()
        {
            if (_activeTiles.Count == 0) return;

            if (_activeTiles[0].transform.position.y < -tileLength)
            {
                GameObject oldTile = _activeTiles[0];
                _activeTiles.RemoveAt(0);
                Destroy(oldTile);

                SpawnRandomTile();
            }
        }

        /// <summary>
        /// Registers the player's branch choice and seamlessly transitions the main generation queue to the persistent side.
        /// </summary>
        /// <param name="leftChosen">True if the player chose the left path branch.</param>
        /// <param name="deadBranchContainer">The root anchor of the abandoned track side to be destroyed.</param>
        /// <summary>
        /// Registers player path choice and seamlessly transitions the main generation queue to the persistent side.
        /// </summary>
        public void RegisterBranchSelection(bool leftChosen, GameObject deadBranchContainer)
        {
            _isForkActive = false;

            if (deadBranchContainer != null)
            {
                // Safely clean up all elements belonging to the unchosen side track
                Collider2D[] childColliders = deadBranchContainer.GetComponentsInChildren<Collider2D>();
                foreach (var col in childColliders)
                {
                    if (col != null)
                    {
                        _activeTiles.Remove(col.gameObject);
                    }
                }
                Destroy(deadBranchContainer);
            }

            // Sync the active anchor to re-route the core sequence
            Transform activeAnchor = leftChosen ? _leftBranchAnchor : _rightBranchAnchor;
            if (activeAnchor != null && activeAnchor.childCount > 0)
            {
                GameObject lastBranchTile = activeAnchor.GetChild(activeAnchor.childCount - 1).gameObject;
                _activeTiles.Clear();
                _activeTiles.Add(lastBranchTile);
            }

            _leftBranchAnchor = null;
            _rightBranchAnchor = null;

            Debug.Log($"[Generator] Route synchronized successfully. Active branch locked.");
        }
    }
}