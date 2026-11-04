namespace FinalRide.Gameplay.Weapons
{
    public class WeaponStats
    {
        public float Damage { get; set; } = 10f;
        public float FireRate { get; set; } = 0.5f;
        public int ProjectileCount { get; set; } = 1;
        public float SpreadAngle { get; set; } = 0f;
        public float ProjectileScale { get; set; } = 1f;

        public bool IsBypassingArmor { get; private set; } = false;
        public bool HasMicroStun { get; private set; } = false;
        public bool IsHeavyDamage { get; private set; } = false;

        /// <summary>
        /// Safely applies shocker attributes using logical OR to prevent data overwriting in the stack.
        /// </summary>
        public void ApplyShockerAttributes()
        {
            IsBypassingArmor |= true;
            HasMicroStun |= true;
        }

        /// <summary>
        /// Safely applies heavy armor-piercing attributes to the active weapon calculation block.
        /// </summary>
        public void ApplyCannonAttributes()
        {
            IsHeavyDamage |= true;
        }
    }
}