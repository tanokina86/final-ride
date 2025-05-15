using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet : Projectile
{
    private Sprite _sprite = null;

    private new void Awake()
    {
        name = String.IsNullOrEmpty(name) ? "Bullet" : name;

        if (!gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            gameObject.AddComponent<SpriteRenderer>();

            _sprite = (Sprite)Resources.Load("Assets/FINAL_RIDE/Sprites/Projectiles/bullet.png", typeof(Sprite));
        }

        base.Awake();
    }


    private new void Start()
    {
        base.Start();

        float maxValue = Speed / 100f;

        Vector3 someVector = new Vector3(UnityEngine.Random.Range(-maxValue, maxValue), UnityEngine.Random.Range(0, maxValue), 0);
        gameObject.transform.localPosition = GameObject.FindWithTag("ShootPosition").transform.position + someVector;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
