using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Enums;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private AttackType attackType = AttackType.Normal;

        [SerializeField]
        private float _rotationSpeed = 0f;

        [SerializeField]
        private Sprite mainSprite;

        [SerializeField]
        private Projectile projectile;

        //[SerializeField]
        private GameObject _gun = null;
        protected AttackType AttackType => attackType; //better to attach to projectile class

        private Rigidbody2D _gunRigidbody;

        private SpriteRenderer _gunRenderer;

        public Rigidbody2D Body => _gunRigidbody;
        public SpriteRenderer SR => _gunRenderer;

        public Sprite Sprite { get { return _gunRenderer?.sprite; } set { _gunRenderer.sprite = value;  } }

        public Projectile Projectile => projectile;


        public float RotationSpeed => _rotationSpeed;

        public Transform ShootPosition { get; private set; } = null;

        public virtual void Rotate(float angle)
        {
            _gun.transform.rotation = Quaternion.AngleAxis(angle * _rotationSpeed * Time.deltaTime, Vector3.forward);
        }

        public virtual void Rotate(Vector3 direction)
        {
            _gun.transform.rotation = Quaternion.FromToRotation(_gun.transform.rotation.eulerAngles, direction);
        }

        public virtual void Rotate(int direction)
        {
            float zAxisValue = direction * _rotationSpeed * Time.fixedDeltaTime;

            _gun.transform.rotation = Quaternion.Euler(0, 0, Body.rotation += zAxisValue);
        }

        public virtual void Shoot(bool randomPosition = true)
        {
            Quaternion projectileRotation = Body.transform.rotation;

            Vector3 projectilePosition = ShootPosition.position;

            float directionAngle = UnityEngine.Random.Range(-.015f, .015f);

            projectileRotation.z = randomPosition ? projectileRotation.z + directionAngle : projectileRotation.z;

            Instantiate(projectile, projectilePosition, projectileRotation);

            projectile?.gameObject.SetActive(true);
        }

        public virtual void Shoot(GameObject target)
        {
            //_gun.transform.rotation.SetFromToRotation(_gun.transform.rotation.eulerAngles, target.transform.rotation.eulerAngles);
        }

        private void OnEnable()
        {

            Debug.Log($"GUN {_gun.name} is created");
        }

        private void SetRequireComponents()
        {
            _gunRigidbody = _gun.AddComponent<Rigidbody2D>();
            _gunRenderer = _gun.AddComponent<SpriteRenderer>();
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
            if (!Application.isPlaying && SceneManager.GetActiveScene().isLoaded)
            {
                Debug.Log($"{this.name} awaked");
            }
        }
    }
}
