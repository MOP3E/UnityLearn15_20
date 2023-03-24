using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Уничтожаемый объект, у которого есть некоторое количество очков жизни.
    /// </summary>
    public class Destructible : Entity
    {
        /// <summary>
        /// Это неразрушимый объект.
        /// </summary>
        [SerializeField] private bool _indestrictible = false;

        /// <summary>
        /// Это неразрушимый объект.
        /// </summary>
        public bool IsIndestrictible => _indestrictible;

        /// <summary>
        /// Максимальное (и начальное) число очков жизни разрушаемого объекта.
        /// </summary>
        [SerializeField] private int _maxHitpoints = 0;

        /// <summary>
        /// Очки жизни разрушаемого объекта.
        /// </summary>
        private int _hitpoints;

        /// <summary>
        /// Очки жизни разрушаемого объекта.
        /// </summary>
        public int Hitpoints
        {
            get => _hitpoints;
            private set
            {
                _hitpoints = value;
                HitpointsChange?.Invoke();
            }
        }

        /// <summary>
        /// Событие разрушения объекта.
        /// </summary>
        [HideInInspector] public UnityEvent Destruction;

        /// <summary>
        /// Событие изменения очков жизни объекта.
        /// </summary>
        [HideInInspector] public UnityEvent HitpointsChange;

        /// <summary>
        /// Уже убит.
        /// </summary>
        private bool _isKilled = false;

        /// <summary>
        /// Вызывается перед первым кадром.
        /// </summary>
        protected virtual void Start()
        {
            Hitpoints = _maxHitpoints;
        }

        /// <summary>
        /// Нанесение урона объекту.
        /// </summary>
        /// <param name="damage">Величина наносимого урона.</param>
        public void Hit(int damage)
        {
            if(_indestrictible) return;
            Hitpoints -= damage;
            if (Hitpoints <= 0) Kill();
        }

        /// <summary>
        /// Лечение объекта.
        /// </summary>
        /// <param name="cure">Величина лечения.</param>
        public bool Cure(int cure)
        {
            if (Hitpoints >= _maxHitpoints) return false;
            Hitpoints += cure;
            if (Hitpoints > _maxHitpoints) Hitpoints = _maxHitpoints;
            return true;
        }

        /// <summary>
        /// Гарантированное убийство объекта.
        /// </summary>
        public void Kill()
        {
            if (_isKilled) return;

            Hitpoints = 0;
            Destruction?.Invoke();
            _isKilled = true;
        }

        /// <summary>
        /// Получить нормализованное число очков жизни.
        /// </summary>
        public float GetNormalizedHitpoints()
        {
            return Hitpoints < _maxHitpoints ? (float)Hitpoints / (float)_maxHitpoints : 1f;
        }
    }
}
