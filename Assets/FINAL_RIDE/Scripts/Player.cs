using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Vehicle, IWeaponed
{
    Rigidbody2D _playerRigidbody = null;

    [SerializeField]
    private float _gunRotationSpeed = 0f;

    [SerializeField]
    GameObject _projectile = null;

    public bool HasAdditionalWeapon { get; } = true;

    private Rigidbody2D _gun = null;

    //TODO: Make additional class Weapon for IWeaponed entities
    [SerializeField]
    private AttackType _attackType = AttackType.Normal;
    public AttackType AttackType => _attackType;

    public Dictionary<AttackType, KeyValuePair<float, float>> Weapon => throw new System.NotImplementedException();

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();

        _gun = GameObject.FindWithTag("Gun").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();

        RotateGun();

        if (Input.GetKey(KeyCode.Space)) Shoot(2f);
    }

    public override void Move()
    {
        Vector2 axisIncrement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * MovementSpeed * Time.fixedDeltaTime;

        _playerRigidbody.MovePosition(_playerRigidbody.position + axisIncrement);
    }

    public void RotateGun()
    {
        int direction = Input.GetKey(KeyCode.Mouse0) ? 1 : Input.GetKey(KeyCode.Mouse1) ? -1 : 0;

        float zAxisValue = direction * _gunRotationSpeed * Time.fixedDeltaTime;

        _gun.transform.rotation = Quaternion.Euler(0, 0, _gun.rotation += zAxisValue);
    }

    public void Shoot(float baseDamage)
    {
        Instantiate(_projectile, _projectile.transform.position, _gun.transform.rotation);

        _projectile.gameObject.SetActive(true);
    }
}
