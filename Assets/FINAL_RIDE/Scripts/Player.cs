using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Vehicle, IWeaponed
{
    Rigidbody2D _playerRigidbody = null;

    [SerializeField]
    GameObject _projectile = null;

    public bool HasAdditionalWeapon { get; } = true;

    [SerializeField]
    private Weapon _weapon;

    public Weapon Weapon { get; set ; }

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();

        Weapon = _weapon is null ? (Weapon)ScriptableObject.CreateInstance(typeof(Weapon)) : _weapon;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();

        RotateGun();



        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(CreateProjectile());
        }
        else if (Input.GetKeyUp(KeyCode.Space)) StopAllCoroutines();
        //Shoot(1f);

    }

    public override void Move()
    {
        Vector2 axisIncrement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * MovementSpeed * Time.fixedDeltaTime;

        _playerRigidbody.MovePosition(_playerRigidbody.position + axisIncrement);

        _weapon.Body.position = _playerRigidbody.position;
    }

    public void RotateGun()
    {
        int direction = Input.GetKey(KeyCode.Mouse0) ? 1 : Input.GetKey(KeyCode.Mouse1) ? -1 : 0;

        Weapon.Rotate(direction);
    }

    public void Shoot(float speed)
    {
        



        //CreateProjectile(speed);
    }

    private IEnumerator CreateProjectile(float speed = 1f)
    {
        yield return new WaitForSeconds(speed);

        float directionAngle = Random.Range(-.015f, .015f);

        Quaternion projectileRotation = Weapon.Body.transform.rotation;

        //Vector3 projectilePosition = _projectile.transform.localPosition;

        Vector3 projectilePosition = Weapon.ShootPosition.position;

        //Debug.Log("BEFORE: " + projectilePosition);

        projectileRotation.z = projectileRotation.z + directionAngle;

        //Debug.Log("AFTER: " + projectilePosition);

        Instantiate(_projectile, projectilePosition, projectileRotation);

        _projectile.gameObject.SetActive(true);
        
    }
}
