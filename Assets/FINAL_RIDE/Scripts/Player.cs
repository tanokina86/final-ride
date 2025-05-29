using Assets.FINAL_RIDE.Scripts.Abstract_Entities;
using Assets.Scripts;
using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Vehicle, IWeaponed
{
    private Rigidbody2D _playerRigidbody = null;

    //private Weapon _weapon;

    public Gun Gun => this.GetComponentInChildren<Gun>();

    private WeaponHandler _weaponHandler;
    public bool HasAdditionalWeapon { get; } = true;

    [SerializeField]

    public Weapon Weapon { get; set ; }

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();

        SetWeapon();

        StartCoroutine(ShootRoutine(.25f));
    }
    
    private void Update()
    {
        //Shoot();

        //timer = Time.deltaTime * timer++;

        //Debug.Log(timer);

        if (Input.GetKeyDown(KeyCode.U))
        {
            _weaponHandler.UpgradeWeapon((AttackType)(Random.Range(1, 5)));
        }
    }
    
    private void FixedUpdate()
    {
        Move();

        RotateGun();
    }
    
    private void LateUpdate()
    {
        
    }

    private void SetWeapon()
    {
        _weaponHandler.CreateWeaponData(); //launch it in another class Bootstrap 

        _weaponHandler.CreateGun(AttackType.Normal, "MachineGun");

        _weaponHandler.SetGunTo(transform);

        //if (_weapon is null)
        //{
        //    weapon = _weapon;

        //    weapon.body.transform.setparent(this.gameobject.transform, false);

        //    weapon.body.transform.position = _playerrigidbody.position;

        //    weapon.body.gameobject.tag = "playergun";

        //    weapon.body.gameobject.setactive(true);

        //}
    }

    //private void StartShoot()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        StopCoroutine(CreateProjectileRoutine());
    //        Debug.Log($"COROUTINE CreateProjectile IS ENDED");
    //    }

    //    StartCoroutine(CreateProjectileRoutine());
    //}

    #region Base Actions
    public override void Move()
    {
        Vector2 axisIncrement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * MovementSpeed * Time.fixedDeltaTime;

        _playerRigidbody.MovePosition(_playerRigidbody.position + axisIncrement);
    }

    public void RotateGun()
    {
        int direction = Input.GetKey(KeyCode.Mouse0) ? 1 : Input.GetKey(KeyCode.Mouse1) ? -1 : 0;

        Gun.Rotate(direction);
    }

    public void Shoot()
    {
        if(Input.GetKey(KeyCode.Space)) Gun.Shoot();
    }

    private IEnumerator ShootRoutine(float speed = .5f)
    {
        while(Health > 0)
        {
            yield return new WaitForSeconds(speed);

            Shoot();
        }
    }

    #endregion

    #region Special Abilities
    /// <summary>
    /// Contains all of the Player abilities 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    #endregion
    //private IEnumerator CreateProjectileRoutine(float speed = .5f)
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        yield return new WaitForSeconds(speed);

    //        CreateProjectile(true);
    //    }
    //}

    private void OnEnable()
    {
        //_weapon = ScriptableObject.CreateInstance(typeof(Weapon)) as Weapon;

        _weaponHandler = new WeaponHandler(name);
    }
}
