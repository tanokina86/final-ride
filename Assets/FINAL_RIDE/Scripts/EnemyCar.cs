using Assets.FINAL_RIDE.Scripts.Abstract_Entities;
using Assets.Scripts;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : Vehicle, IEnemy
{
    private int _id = 0;

    public int Id => _id;

    private string _name = null;

    private string _description = null;

    private Rigidbody2D _rigidbody = null;

    private GameObject _player = null;

    public string Name { get => string.IsNullOrEmpty(_name) ? name : _name; set => _name = value; }

    public string Discription => _description;

    public IEnumerable<Dictionary<string, Action>> Abilities { get; set; }

    //[SerializeField]
    //private List<Weapon> _weapon;
    
    private WeaponHandler _weaponHandler;

    public Weapon Weapon { get; set; }

    public Gun Gun => this.GetComponentInChildren<Gun>();

    public bool HasAdditionalWeapon => false;

    public EnemyCar()
    {
        _id += 1;
    }

    public override void Move()
    {
        Vector2 playerPosition = _player.transform.position;

        _rigidbody.position += (playerPosition - _rigidbody.position) * MovementSpeed * Time.fixedDeltaTime;

        //if (_weapon != null) _weapon[0].Body.position = _rigidbody.position;
    }

    public void Describe(string text)
    {
        _description = text;
    }

    public void Shoot(float baseDamage)
    {
        throw new System.NotImplementedException();
    }

    public void Shoot()
    {
        Gun.Shoot();
    }

    private IEnumerator ShootRoutine(float speed = .5f)
    {
        while (Health > 0)
        {
            yield return new WaitForSeconds(speed);

            Shoot();
        }
    }

    private void OnEnable()
    {
        Debug.Log($"Enemy - {Name} is now Enable. Has capability to fly: {IsFlying}");

        _weaponHandler = new WeaponHandler(name);
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _player = FindObjectOfType<Player>().gameObject;

        _weaponHandler.CreateGun(AttackType.Splitted, "Pump");

        _weaponHandler.SetGunTo(transform);

        StartCoroutine(ShootRoutine(1f));


        //if (_weapon is not null)
        //{
        //    Weapon = _weapon;

        //    Weapon.Body.transform.SetParent(this.gameObject.transform, false);

        //    Weapon.Body.transform.position = _rigidbody.position;
        //}

        //Weapon = _weapon is null ? (Weapon)ScriptableObject.CreateInstance(typeof(Weapon)) : _weapon;

        //Weapon.Body.transform.position = _rigidbody.position;
    }

    private void FixedUpdate()
    {
        Move();

        //Vector2 targetDirection = _player.transform.position - transform.position;

        //float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * (Mathf.Rad2Deg - 90);

        //Weapon.Rotate(angle);

    }

    private void Update()
    {
        Vector2 targetDirection = (_player.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;

        Gun.Rotate(angle);

        //Debug.Log(angle);

        //Gun.Body.MoveRotation(angle);
    }

    //public delegate void MoveHandler();

    //public event MoveHandler OnMove;

    private void Awake()
    {
        
    }
}
