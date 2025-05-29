using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour, IMovable
{
    private Vector3 _startPosition = new Vector3();

    private Rigidbody2D _rigidBody = null;

    protected AttackType _attackType;

    protected float _baseDamage = 0f;

    [SerializeField]
    private float _moveSpeed = 0f;

    protected float _lifeTime = 1f;

    protected float Speed => _moveSpeed;

    public virtual void Move()
    {
        _rigidBody.transform.Translate(Vector3.up * _moveSpeed * Time.fixedDeltaTime);
    }

    // Start is called before the first frame update
    protected void Start()
    {

        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
        _rigidBody.simulated = true;
        _rigidBody.transform.position = _startPosition;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_rigidBody != null) Move();

        if (_lifeTime <= 0f) Destroy(this);
    }

    protected void FixedUpdate()
    {

    }

    protected void Awake()
    {
        _startPosition = GameObject.Find("ShootPosition").transform.position;
        //Debug.Log(gameObject.name.ToUpper() + $" Instatiated {transform.localPosition}");
    }

    private void OnDestroy()
    {
        // someAnimation
    }
}
