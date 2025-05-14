using Assets.Scripts.Enums;
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

    [SerializeField]
    private AttackType _attackType = AttackType.Normal;

    public string Name { get => string.IsNullOrEmpty(_name) ? name : _name; set => _name = value; }

    public string Discription => _description;

    public IEnumerable<Dictionary<string, string>> Abilities { get; set; }

    public AttackType AttackType => _attackType;

    public Dictionary<AttackType, KeyValuePair<float, float>> Weapon => throw new System.NotImplementedException();

    bool IWeaponed.HasAdditionalWeapon => throw new System.NotImplementedException();

    public EnemyCar()
    {
        _id += 1;
    }

    public override void Move()
    {
        Vector2 playerPosition = _player.transform.position;

        _rigidbody.position += (playerPosition - _rigidbody.position) * MovementSpeed * Time.fixedDeltaTime;
    }

    public void Describe(string text)
    {
        _description = text;
    }

    public void Shoot(float baseDamage)
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable()
    {
        Debug.Log($"Enemy - {Name} is now Enable. Has capability to fly: {IsFlying}");

    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _player = FindObjectOfType<Player>().gameObject;
    }

    private void FixedUpdate()
    {
        Move();
    }

    //public delegate void MoveHandler();

    //public event MoveHandler OnMove;


}
