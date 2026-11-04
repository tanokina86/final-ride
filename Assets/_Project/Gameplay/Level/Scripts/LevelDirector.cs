using UnityEngine;
using FinalRide.Gameplay.Level;

namespace FinalRide.Gameplay.Level
{
    public enum LevelPhase { Intro, MiddleGame, SurvivalStorm, BossFight }

    public class LevelDirector : MonoBehaviour
    {
        [Header("Linked Systems")]
        [SerializeField] private RoadGenerator roadGenerator;
        [SerializeField] private EnemySpawner enemySpawner;

        [Header("Level Dimensions")]
        [SerializeField] private float targetLevelDistance = 500f;

        private float _currentDistance = 0f;
        private bool _subBossSpawned = false;
        private bool _isBossFight = false;

        public LevelPhase CurrentPhase { get; private set; } = LevelPhase.Intro;
        public float Progress => Mathf.Clamp01(_currentDistance / targetLevelDistance);

        private void Start()
        {
            if (roadGenerator == null) roadGenerator = FindObjectOfType<RoadGenerator>();
            if (enemySpawner == null) enemySpawner = FindObjectOfType<EnemySpawner>();

            Debug.Log("[Director] Level Director initialized. Distance target: " + targetLevelDistance);
        }

        private void Update()
        {
            if (_isBossFight) return;

            if (roadGenerator != null && roadGenerator.ScrollSpeed > 0)
            {
                _currentDistance += roadGenerator.ScrollSpeed * Time.deltaTime;
            }

            HandleLevelPhases();
        }

        private void HandleLevelPhases()
        {
            float currentProgress = Progress;

            if (currentProgress < 0.3f && CurrentPhase != LevelPhase.Intro)
            {
                CurrentPhase = LevelPhase.Intro;
                Debug.Log("[Director] Phase 1: Intro stage.");
            }
            else if (currentProgress >= 0.3f && currentProgress < 0.6f)
            {
                if (CurrentPhase != LevelPhase.MiddleGame)
                {
                    CurrentPhase = LevelPhase.MiddleGame;
                    Debug.Log("[Director] Phase 2: Middle game stage.");
                }

                // Midpoint trigger for sub-boss event
                if (currentProgress >= 0.5f && !_subBossSpawned)
                {
                    TriggerSubBossEvent();
                }
            }
            else if (currentProgress >= 0.6f && currentProgress < 0.85f && CurrentPhase != LevelPhase.SurvivalStorm)
            {
                CurrentPhase = LevelPhase.SurvivalStorm;
                Debug.Log("[Director] Phase 3: Survival storm stage active.");

                if (enemySpawner != null) enemySpawner.SetSpawnActive(true);
            }
            else if (currentProgress >= 1.0f)
            {
                TriggerFinalBossFight();
            }
        }

        private void TriggerSubBossEvent()
        {
            _subBossSpawned = true;
            Debug.Log("[Director] >>> MIDPOINT REACHED (50%)! SUB-BOSS ENCOUNTER <<<");

            if (enemySpawner != null) enemySpawner.SetSpawnActive(false);

            // TODO: Instantiate Sub-Boss prefab here
        }

        private void TriggerFinalBossFight()
        {
            _isBossFight = true;
            CurrentPhase = LevelPhase.BossFight;
            Debug.Log("[Director] >>> GOAL REACHED (100%)! FINAL BOSS BATTLE <<<");

            if (roadGenerator != null) roadGenerator.ScrollSpeed = 0f;
            if (enemySpawner != null) enemySpawner.SetSpawnActive(false);

            // TODO: Instantiate Final Boss prefab here
        }
    }
}