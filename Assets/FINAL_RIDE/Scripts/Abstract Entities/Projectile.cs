using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IMovable
{
    Vector3 _startPosition = new Vector3();

    Rigidbody2D _rigidBody = null;

    [SerializeField]
    private float _moveSpeed = 0f;

    public virtual void Move()
    {
        _rigidBody.transform.Translate(Vector3.up * _moveSpeed * Time.fixedDeltaTime);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        _startPosition = GameObject.FindWithTag("ShootPosition").transform.position;
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.transform.position = _startPosition;
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        if (_rigidBody != null) Move();
    }

    protected void Awake()
    {
        Debug.Log(gameObject.name.ToUpper() + " Instatiated");
    }
}
