using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Enums;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Entity / Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private AttackType attackType = AttackType.Normal;

        [SerializeField]
        private float _rotationSpeed = 0f;

        [SerializeField]
        private Sprite sprite;

        //[SerializeField]
        private GameObject _gun = null;

        public Rigidbody2D Body => _gun.GetComponent<Rigidbody2D>();

        protected AttackType AttackType => attackType;

        public float RotationSpeed => _rotationSpeed;

        public Transform ShootPosition { get; private set; } = null;

        //public Weapon()
        //{
            
        //}

        public void Rotate(float angle)
        {

        }

        public void Rotate(Vector3 direction)
        {

        }

        public void Rotate(int direction)
        {
            float zAxisValue = direction * _rotationSpeed * Time.fixedDeltaTime;

            _gun.transform.rotation = Quaternion.Euler(0, 0, Body.rotation += zAxisValue);
        }

        public void Shoot()
        {

        }

        public void Shoot(GameObject target)
        {

        }

        private void OnEnable()
        {
            _gun = new GameObject();
            _gun.name = string.Concat(attackType.ToString(), "Gun");
            _gun.tag = "Gun";
            _gun.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            _gun.AddComponent<Transform>();


            SpriteRenderer spriteRenderer = _gun.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = 3;


            ShootPosition = _gun.gameObject.GetComponent<Transform>();
            ShootPosition.SetParent(_gun.transform);
            ShootPosition.position = new Vector3(ShootPosition.position.x, ShootPosition.position.y - 0.05f, ShootPosition.position.z);

            GameObject child = new GameObject("ShootPosition");
            child.tag = "ShootPosition";
            child.transform.SetParent(_gun.transform, false);

            ShootPosition.position = child.transform.position += new Vector3(0, 0.5f, 0);
        }
    }
}
