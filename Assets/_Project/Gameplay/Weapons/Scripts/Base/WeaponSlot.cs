using Assets.Scripts.Enums;
using FinalRide.Gameplay.Weapons.Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalRide.Gameplay.Weapons.Base
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class WeaponSlot : MonoBehaviour
    {
        [Header("Infrastructure Dependencies")]
        [SerializeField] private WeaponHandler weaponHandler;

        [Header("Runtime Weapon Data")]
        [SerializeField] private Weapon weaponData;

        private WeaponStatsBuilder _statsBuilder = new WeaponStatsBuilder();

        private SpriteRenderer _gunRenderer;
        private float _currentHeat = 0f;
        private float _nextShotTime = 0f;
        private bool _isOverheated = false;
        private float _overheatRecoveryTimer = 0f;

        public Bounds Bounds => _gunRenderer != null && _gunRenderer.sprite != null ? _gunRenderer.sprite.bounds : new Bounds();
        public int RendererOrder { get; set; } = 0;
        public string Name { get; set; }

        public AttackType AttackType => weaponData != null ? weaponData.AttackType : AttackType.Normal;
        public float RotationSpeed => weaponData != null ? weaponData.RotationSpeed : 0f;
        public Projectile Projectile => weaponData != null ? weaponData.ProjectilePrefab : null;
        public Sprite Sprite { get { return _gunRenderer.sprite; } set { _gunRenderer.sprite = value; } }
        public Weapon WeaponData => weaponData;

        public Rigidbody2D Body => GetComponent<Rigidbody2D>();
        public Vector3 ShootPosition { get; private set; } = Vector3.zero;

        /// <summary>
        /// Links the structural infrastructure owner references and prepares internal builder modules.
        /// </summary>
        public void InitializeSlot(WeaponHandler handler)
        {
            weaponHandler = handler;
            _statsBuilder = new WeaponStatsBuilder();
        }

        /// <summary>
        /// Hot-swaps the core structural weapon data container template references.
        /// </summary>
        public void ApplyWeaponData(Weapon newData)
        {
            weaponData = newData;
            if (weaponData != null && _gunRenderer != null)
            {
                _gunRenderer.sprite = weaponData.Sprite;
            }
        }

        /// <summary>
        /// Executes alignment rotation targeting an explicit angle coordinate profile.
        /// </summary>
        public virtual void Rotate(float angle)
        {
            Body.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        /// <summary>
        /// Executes directional rotation tracing an input navigation trajectory vector.
        /// </summary>
        public virtual void Rotate(Vector3 direction)
        {
            Body.transform.rotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, direction);
        }

        /// <summary>
        /// Increments turret rotation angles utilizing inputs modified by dynamic compilation velocity variables.
        /// </summary>
        public virtual void Rotate(int direction)
        {
            float dynamicSpeedMultiplier = 1f;
            if (weaponHandler != null && _statsBuilder != null)
            {
                WeaponStats stats = _statsBuilder.BuildFinalStats(weaponHandler.UpgradeStatus);
                if (stats != null)
                {
                    dynamicSpeedMultiplier = stats.RotationSpeedMultiplier;
                }
            }

            float zAxisValue = direction * (RotationSpeed * dynamicSpeedMultiplier) * Time.fixedDeltaTime;
            Body.transform.rotation = Quaternion.Euler(0, 0, Body.rotation += zAxisValue);
        }


        /// <summary>
        /// Evaluates heat limits and rate thresholds to execute single or fan-shaped multi-projectile shot logic.
        /// </summary>
        public virtual void Shoot(bool randomPosition = true)
        {
            if (Projectile == null || _isOverheated || Time.time < _nextShotTime) return;

            if (weaponHandler == null || _statsBuilder == null)
            {
                ExecuteSingleShot(randomPosition);
                return;
            }

            WeaponStats stats = _statsBuilder.BuildFinalStats(weaponHandler.UpgradeStatus);
            if (stats == null)
            {
                ExecuteSingleShot(randomPosition);
                return;
            }

            _nextShotTime = Time.time + stats.FireRate;

            ExecuteSpreadShot(stats, randomPosition);
            AccumulateHeat(stats);
        }

        /// <summary>
        /// Registers target data nodes for guided missile operations tracker components.
        /// </summary>
        public virtual void Shoot(GameObject target)
        {
        }

        private void ExecuteSingleShot(bool randomPosition)
        {
            Quaternion projectileRotation = Body.transform.rotation;
            if (randomPosition)
            {
                projectileRotation.z += UnityEngine.Random.Range(-.015f, .015f);
            }

            Projectile bulletInstance = Instantiate(Projectile, ShootPosition, projectileRotation);
            if (bulletInstance != null)
            {
                bulletInstance.gameObject.SetActive(true);
            }
        }

        private void ExecuteSpreadShot(WeaponStats stats, bool randomPosition)
        {
            int count = stats.ProjectileCount;
            if (count <= 1)
            {
                SpawnCompiledProjectile(Body.transform.rotation, stats, randomPosition);
                return;
            }

            float startAngle = -stats.SpreadAngle / 2f;
            float angleStep = stats.SpreadAngle / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Quaternion spreadRotation = Body.transform.rotation * Quaternion.Euler(0f, 0f, currentAngle);
                SpawnCompiledProjectile(spreadRotation, stats, randomPosition);
            }
        }

        private void SpawnCompiledProjectile(Quaternion targetRotation, WeaponStats stats, bool randomPosition)
        {
            if (randomPosition)
            {
                targetRotation.z += UnityEngine.Random.Range(-.015f, .015f);
            }

            Projectile bulletInstance = Instantiate(Projectile, ShootPosition, targetRotation);
            if (bulletInstance != null)
            {
                bulletInstance.transform.localScale *= stats.ProjectileScale;

                // INJECTION: Transfer dynamic combo stats directly into the spawned physics handler
                bulletInstance.InitializePayload(
                    stats.Damage,
                    stats.MagnetismRadius,
                    stats.HasMicroStun,
                    stats.PiercedArmorTypes
                );

                bulletInstance.gameObject.SetActive(true);
            }
        }

        private void AccumulateHeat(WeaponStats stats)
        {
            if (_isOverheated) return;

            _currentHeat += stats.HeatPerShot;
            _currentHeat = Mathf.Min(_currentHeat, 100f);

            if (_currentHeat >= 100f)
            {
                _isOverheated = true;
                _overheatRecoveryTimer = stats.OverheatCooldownDuration;
                Debug.LogWarning("[WeaponSystem] Critical overheat reached! Firing interface locked.");
            }
        }

        private void HandleCooling(WeaponStats stats)
        {
            if (_currentHeat > 0f && !_isOverheated)
            {
                _currentHeat -= stats.CoolingRate * Time.deltaTime;
                _currentHeat = Mathf.Max(_currentHeat, 0f);
            }
        }

        private void HandleOverheatRecovery(WeaponStats stats)
        {
            if (_isOverheated)
            {
                _overheatRecoveryTimer -= Time.deltaTime;
                _currentHeat = Mathf.Lerp(0f, 100f, _overheatRecoveryTimer / stats.OverheatCooldownDuration);

                if (_overheatRecoveryTimer <= 0f)
                {
                    _isOverheated = false;
                    _currentHeat = 0f;
                    Debug.Log("[WeaponSystem] Thermal recovery finalized. Shooting interface operational.");
                }
            }
        }

        private void OnEnable()
        {
            Debug.Log($"WEAPON SLOT {Name} is active");
        }

        private void SetRequireComponents()
        {
            _gunRenderer = GetComponent<SpriteRenderer>();

            if (Body is not null)
            {
                Body.isKinematic = true;
                Body.simulated = true;
                _gunRenderer.sprite = weaponData != null ? weaponData.Sprite : null;
                return;
            }

            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().simulated = true;
            GetComponent<SpriteRenderer>().sprite = weaponData != null ? weaponData.Sprite : null;
            GetComponent<SpriteRenderer>().sortingOrder = RendererOrder;
        }

        private void OnDisable()
        {
            if (!SceneManager.GetActiveScene().isLoaded) return;
            Debug.Log($"{this.name} disabled");
        }

        private void OnDestroy()
        {
            Debug.Log($"{this.name} destroyed");
        }

        private void Awake()
        {
            SetRequireComponents();

            GameObject spObject = new GameObject("ShootPosition") { tag = "SP" };
            spObject.transform.parent = transform;

            float boundCenterY = _gunRenderer != null && _gunRenderer.sprite != null ? _gunRenderer.bounds.center.y : 0f;
            spObject.transform.position = transform.position + new Vector3(0, boundCenterY + 0.25f, 0);

            ShootPosition = spObject.transform.position;

            if (!Application.isPlaying && SceneManager.GetActiveScene().isLoaded)
            {
                Debug.Log($"{this.name} awaked");
            }
        }

        private void Update()
        {
            if (transform.childCount > 0)
            {
                ShootPosition = transform.GetChild(0).position;
            }

            if (weaponHandler == null || _statsBuilder == null) return;

            WeaponStats stats = _statsBuilder.BuildFinalStats(weaponHandler.UpgradeStatus);
            if (stats == null) return;

            HandleCooling(stats);
            HandleOverheatRecovery(stats);
        }
    }
}