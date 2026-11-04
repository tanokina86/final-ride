using UnityEngine;

namespace FinalRide.Gameplay.Abilities
{
    public abstract class VehicleAbility : ScriptableObject
    {
        [Header("Ability Info")]
        [SerializeField] private string abilityName;
        [SerializeField] private float energyCost = 30f;

        public string AbilityName => abilityName;
        public float EnergyCost => energyCost;

        /// <summary>
        /// Метод активации способности. Сюда передается ссылка на игрока, 
        /// чтобы способность могла управлять его физикой, цветом или параметрами.
        /// </summary>
        public abstract void Activate(Vehicles.Player player);
    }
}