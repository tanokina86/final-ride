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
}
