using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.FINAL_RIDE.Scripts.Abstract_Entities
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Gun : MonoBehaviour
    {
        [SerializeField]
        private AttackType attackType = AttackType.Normal;

        [SerializeField]
        private float _rotationSpeed = 0f;

        [SerializeField]
        private Projectile projectile;

        private SpriteRenderer _gunRenderer;

        public Bounds Bounds => _gunRenderer.sprite.bounds;

        public int RendererOrder { get; set; } = 0;

        public string Name { get; set; }

        public Gun(string name)
        {
            Name = name;

            //SetRequireComponents();
        }

        public Gun()
        {
            //SetRequireComponents();
        }

        public Rigidbody2D Body => GetComponent<Rigidbody2D>();

        public Sprite Sprite { get { return _gunRenderer.sprite; } set { _gunRenderer.sprite = value; } }

        public Projectile Projectile => projectile;


        public float RotationSpeed => _rotationSpeed;

        public Vector3 ShootPosition { get; private set; } = Vector3.zero;

        public virtual void Rotate(float angle)
        {
            Body.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public virtual void Rotate(Vector3 direction)
        {
            Body.transform.rotation = Quaternion.FromToRotation(transform.rotation.eulerAngles, direction);
        }

        public virtual void Rotate(int direction)
        {
            float zAxisValue = direction * _rotationSpeed * Time.fixedDeltaTime;

            Body.transform.rotation = Quaternion.Euler(0, 0, Body.rotation += zAxisValue);
        }

        public virtual void Shoot(bool randomPosition = true)
        {
            Quaternion projectileRotation = Body.transform.rotation;

            Vector3 projectilePosition = ShootPosition;

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

            Debug.Log($"GUN {Name} is created");
        }

        private void SetRequireComponents()
        {
            if (Body is not null)
            {
                Body.isKinematic = true;
                Body.simulated = false;

                _gunRenderer = GetComponent<SpriteRenderer>();
                _gunRenderer.sprite = null;

                return;
            }

            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().simulated = false;

            GetComponent<SpriteRenderer>().sprite = null;
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

            GameObject shootPosition = new GameObject("ShootPosition") { tag = "SP"};

            shootPosition.transform.parent = transform;

            ShootPosition = shootPosition.transform.position += new Vector3(0, _gunRenderer.bounds.center.y + 0.25f, 0);

            if (!Application.isPlaying && SceneManager.GetActiveScene().isLoaded)
            {
                Debug.Log($"{this.name} awaked");
            }
        }
    }
}
