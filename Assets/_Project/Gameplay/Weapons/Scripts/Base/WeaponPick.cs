using UnityEngine;
using Assets.Scripts.Enums;

namespace FinalRide.Gameplay.Weapons.Base
{
    [RequireComponent(typeof(Collider2D))]
    public class WeaponPick : MonoBehaviour
    {
        [Header("Pickup Configurations")]
        [SerializeField] private AttackType attackType;

        /// <summary>
        /// Evaluates trigger entries to identify the player vehicle and route upgrade data flow to the weapon handler.
        /// </summary>
        /// <param name="collision">The active external Collider2D component boundary node.</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) return;

            if (collision.CompareTag("Player"))
            {
                Vehicles.Player playerComponent = collision.GetComponent<Vehicles.Player>();

                if (playerComponent != null)
                {
                    WeaponHandler handler = playerComponent.GetWeaponHandler();
                    if (handler != null)
                    {
                        handler.UpgradeWeapon(attackType);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
