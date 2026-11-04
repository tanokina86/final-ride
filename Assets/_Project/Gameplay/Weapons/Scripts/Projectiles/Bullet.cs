using Assets.Scripts.Enums;
using FinalRide.Gameplay.Weapons.Projectiles;

namespace FinalRide.Gameplay.Weapons
{
    // Запрещаем наследование через sealed, как у тебя и было в оригинале
    public sealed class Bullet : Projectile
    {
        protected override void Awake()
        {
            // Сначала вызываем базовый Awake, чтобы корректно подтянулся и настроился Rigidbody2D
            base.Awake();

            // Задаем базовые характеристики пули, которые были в твоем старом коде
            gameObject.name = string.IsNullOrEmpty(gameObject.name) ? "Bullet" : gameObject.name;

            _attackType = AttackType.Normal;
            _baseDamage = 1f;

            // Время жизни (LifeTime) и скорость пули теперь безопасно контролируются 
            // через инспектор префаба этой пули, унаследованный от родительского класса Projectile
        }

        // Методы Start() и Update() нам здесь больше вообще не нужны! 
        // Базовый класс Projectile сам плавно и честно двигает пулю через FixedUpdate физикой Rigidbody2D.
    }
}