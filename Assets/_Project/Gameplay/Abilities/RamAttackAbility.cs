using UnityEngine;
using System.Collections;

namespace FinalRide.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "NewRamAbility", menuName = "Final Ride/Abilities/Ram Attack")]
    public class RamAttackAbility : VehicleAbility
    {
        [Header("Ram Custom Settings")]
        [SerializeField] private float speedMultiplier = 2.5f;
        [SerializeField] private float duration = 0.3f;

        public override void Activate(Vehicles.Player player)
        {
            player.StartCoroutine(RamRoutine(player));
        }

        private IEnumerator RamRoutine(Vehicles.Player player)
        {
            player.IsDashing = true;
            Debug.Log($"[Ability] {AbilityName}! Вперед на таран!");

            Vector2 dashDirection = player.transform.up;
            Rigidbody2D rb = player.PlayerRigidBody;
            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                if (rb != null)
                {
                    rb.velocity = dashDirection * (player.MovementSpeed * speedMultiplier * Time.fixedDeltaTime * 50f);
                }
                yield return new WaitForFixedUpdate();
            }

            if (rb != null) rb.velocity = Vector2.zero;
            player.IsDashing = false;
        }
    }
}
