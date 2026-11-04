using UnityEngine;
using System.Collections;

namespace FinalRide.Gameplay.Abilities
{
    [CreateAssetMenu(fileName = "NewShieldAbility", menuName = "Final Ride/Abilities/Shield")]
    public class ShieldAbility : VehicleAbility
    {
        [Header("Shield Custom Settings")]
        [SerializeField] private float duration = 2f;

        public override void Activate(Vehicles.Player player)
        {
            // Запускаем корутину щита на объекте игрока, чтобы не зависеть от уничтожения SO
            player.StartCoroutine(ShieldRoutine(player));
        }

        private IEnumerator ShieldRoutine(Vehicles.Player player)
        {
            player.IsShieldActive = true;
            Debug.Log($"[Ability] {AbilityName} АКТИВИРОВАН!");

            var renderer = player.GetComponentInChildren<SpriteRenderer>();

            if (renderer != null) renderer.color = Color.cyan;

            yield return new WaitForSeconds(duration);

            if (renderer != null) renderer.color = Color.white;
            player.IsShieldActive = false;
            Debug.Log($"[Ability] {AbilityName} деактивирован.");
        }
    }
}
