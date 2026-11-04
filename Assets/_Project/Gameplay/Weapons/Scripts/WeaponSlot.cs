using Assets.Scripts.Enums;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalRide.Gameplay.Weapons
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class WeaponSlot : MonoBehaviour
    {
        [Header("Runtime Weapon Data")]
        [SerializeField]
        private Weapon weaponData; // Наш паспорт-шаблон с цифрами (заменяет старые инспекторные поля)

        private SpriteRenderer _gunRenderer;
        //private float _nextShotTime = 0f;

        public Bounds Bounds => _gunRenderer != null && _gunRenderer.sprite != null ? _gunRenderer.sprite.bounds : new Bounds();
        public int RendererOrder { get; set; } = 0;
        public string Name { get; set; }

        // Чистые рантайм-ссылки (берутся из ScriptableObject)
        public AttackType AttackType => weaponData != null ? weaponData.AttackType : AttackType.Normal;
        public float RotationSpeed => weaponData != null ? weaponData.RotationSpeed : 0f;
        public Projectile Projectile => weaponData != null ? weaponData.ProjectilePrefab : null;
        public Sprite Sprite { get { return _gunRenderer.sprite; } set { _gunRenderer.sprite = value; } }
        public Weapon WeaponData => weaponData;

        public Rigidbody2D Body => GetComponent<Rigidbody2D>();

        // Точка дула (генерируется кодом в Awake)
        public Vector3 ShootPosition { get; private set; } = Vector3.zero;

        /// <summary>
        /// Динамически накатывает новые данные (спрайт и префаб пуль) из WeaponHandler
        /// </summary>
        public void ApplyWeaponData(Weapon newData)
        {
            weaponData = newData;
            if (weaponData != null && _gunRenderer != null)
            {
                _gunRenderer.sprite = weaponData.Sprite;
            }
        }

        #region ТВОЯ СТАРАЯ МАТЕМАТИКА ВРАЩЕНИЯ (ROTATE)

        // Перегрузка 1: Поворот к конкретному углу (например, для ИИ врагов)
        public virtual void Rotate(float angle)
        {
            Body.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Перегрузка 2: Поворот по вектору направления
        public virtual void Rotate(Vector3 direction)
        {
            Body.transform.rotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, direction);
        }

        // Перегрузка 3: Аркадный инкремент кнопками мыши (для Игрока)
        public virtual void Rotate(int direction)
        {
            float zAxisValue = direction * RotationSpeed * Time.fixedDeltaTime;
            Body.transform.rotation = Quaternion.Euler(0, 0, Body.rotation += zAxisValue);
        }

        #endregion

        #region ТВОЯ СТАРАЯ ЛОГИКА СТРЕЛЬБЫ (SHOOT)

        // Стрельба 1: Базовый выстрел с твоим фирменным разбросом
        public virtual void Shoot(bool randomPosition = true)
        {
            if (Projectile == null) return;

            // Твоя оригинальная логика направления и позиции снаряда
            Quaternion projectileRotation = Body.transform.rotation;
            Vector3 projectilePosition = ShootPosition;

            float directionAngle = UnityEngine.Random.Range(-.015f, .015f);
            projectileRotation.z = randomPosition ? projectileRotation.z + directionAngle : projectileRotation.z;

            // Честный спавн префаба из ScriptableObject без кулдаунов
            Instantiate(Projectile, projectilePosition, projectileRotation);

            if (Projectile != null)
            {
                Projectile.gameObject.SetActive(true);
            }
        }

        // Стрельба 2: Заготовка под автонаведение
        public virtual void Shoot(GameObject target)
        {
            // Сюда перенесем логику трекинга цели при необходимости
        }

        #endregion

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
                Body.simulated = true; // Сделаем true, чтобы триггеры пуль работали стабильно
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

            // Твоя автоматическая генерация точки дула на основе высоты спрайта
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
            // Каждый кадр обновляем позицию дула, чтобы она следовала за поворотом и движением пушки
            if (transform.childCount > 0)
            {
                ShootPosition = transform.GetChild(0).position;
            }
        }
    }
}
