namespace FinalRide.Gameplay.Interfaces
{
    public interface IDamageable
    {
        public bool IsInvincible { get; }

        public float Health { get; }
        void TakeDamage(float damage);
    }
}