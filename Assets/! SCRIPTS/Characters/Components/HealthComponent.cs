using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class HealthComponent : MonoBehaviour
    {
        #region FIELDS PRIVATE
        private int _maxHealth;
        private int _currentHealth;
        #endregion

        #region EVENTS
        public event UnityAction<int, int> OnHealthChange;
        public event UnityAction OnHit;
        public event UnityAction OnDie;
        #endregion

        #region PROPERTIES
        public bool IsDied => _currentHealth <= 0;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        #endregion

        #region METHODS PUBLIC
        public void Init(uint health)
        {
            _maxHealth = (int)health;
            _currentHealth = _maxHealth;
            OnHealthChange?.Invoke(_currentHealth, _maxHealth);
        }

        public void SetMaxHealth(uint value)
        {
            _maxHealth = (int)value;
            OnHealthChange?.Invoke(_currentHealth, _maxHealth);
        }

        public bool DealDamage(uint damage)
        {
            if (IsDied) return true;

            var result = false;
            _currentHealth -= (int)damage;
            OnHit?.Invoke();
            OnHealthChange?.Invoke(_currentHealth, _maxHealth);

            if (IsDied)
            {
                result = true;
                OnDie?.Invoke();
            }

            return result;
        }

        public void Heal(uint value)
        {
            _currentHealth += (int)value;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            OnHealthChange?.Invoke(_currentHealth, _maxHealth);
        }
        #endregion
    }
}
