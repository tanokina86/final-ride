using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public sealed class Bullet : Projectile
{
    private Sprite _sprite = null;

    private new void Awake()
    {
        name = String.IsNullOrEmpty(name) ? "Bullet" : name;

        _attackType = Assets.Scripts.Enums.AttackType.Normal;

        _baseDamage = 1f;

        _lifeTime = 10f;

        if (!gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            gameObject.AddComponent<SpriteRenderer>();

            _sprite = (Sprite)Resources.Load("Assets/FINAL_RIDE/Sprites/Projectiles/bullet.png", typeof(Sprite));
        }

        //base.Awake(); //Shows message info in console
    }


    private new void Start()
    {
        base.Start();

        float offsetMaxValue = Speed / 100f;

        Vector3 someVector = new Vector3(UnityEngine.Random.Range(-offsetMaxValue, offsetMaxValue), UnityEngine.Random.Range(0, offsetMaxValue), 0);
        
        Transform someTransform = GameObject.Find("PlayerWeapon").GetComponentInChildren(typeof(Transform), name == "ShootPosition") as Transform;

        gameObject.transform.localPosition = someTransform.position + someVector;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
