using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "TestManager", menuName = "Test/ SettingsTest")]
    public class SomeManager : ScriptableObject
    {
        //[SerializeField]
        //EnemyCar _enemyCar;

        public SomeManager()
        {
            
        }

        //public static void AddComponents(EnemyCar enemyCar)
        //{
        //    enemyCar.gameObject.AddComponent(typeof(Rigidbody2D));

        //    enemyCar.gameObject.AddComponent(typeof(BoxCollider2D));

        //    enemyCar.gameObject.AddComponent(typeof(SpriteRenderer));

        //    enemyCar.GetComponent<SpriteRenderer>();

        //    enemyCar.GetComponent<SpriteRenderer>().enabled = true;
        //}

        public static void SetAttributes<T>(T gameObject) where T : Vehicle
        {
            if (gameObject is not null & string.IsNullOrEmpty(gameObject.name))
            {
                try
                {
                    Sprite sprite = (Sprite)Resources.Load($"Assets / FINAL_RIDE / Sprites / Vehicle /{gameObject.name}.png");
                }

                catch { }

                finally { }
            }
        }
    }
}
