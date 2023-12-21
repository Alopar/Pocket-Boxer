using System.Collections.Generic;
using UnityEngine;
using Screens.Layers.Arena;
using Utility.DependencyInjection;
using Services.SignalSystem;
using Services.SignalSystem.Signals;

namespace Gameplay
{
    public class EnemyController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private float _brainDelay;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Find] private BoxerController _boxerController;

        private float _brainTimer = 0f;
        private bool _brainAvailable = false;

        private BoxerController _opponentController;
        #endregion

        #region HANDLERS
        [Subscribe]
        private void PlayerBoxerSpawn(PlayerBoxerSpawn signal)
        {
            _opponentController = signal.BoxerController;
        }
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            LaunchBrainTimer();
        }

        private void OnEnable()
        {
            _signalService.Subscribe(this);
        }

        private void OnDisable()
        {
            _signalService.Unsubscribe(this);
        }

        private void Update()
        {
            if(_boxerController.State == BoxerState.Victory || _boxerController.State == BoxerState.Death)
            {
                this.enabled = false;
                return;
            }

            UpdateTimers();
            ChooseAbility();
        }
        #endregion

        #region METHODS PRIVATE
        private void LaunchBrainTimer()
        {
            _brainAvailable = false;
            _brainTimer = _brainDelay;
        }

        private void UpdateTimers()
        {
            _brainTimer -= Time.deltaTime;
            if (_brainTimer <= 0)
            {
                _brainAvailable = true;
            }
        }

        private void ChooseAbility()
        {
            if (!_brainAvailable) return;
            if (_boxerController.State != BoxerState.Stance) return;
            if (_opponentController == null) return;

            if (TryDefence() || TryAttack())
            {
                LaunchBrainTimer();
                return;
            }
        }

        private bool TryAttack()
        {
            var attackAbility = GetAttackAbility();
            if (attackAbility.Count == 0) return false;

            var targetZone = (TargetZone)Random.Range(1, 4);
            attackAbility[Random.Range(0, attackAbility.Count)].TryActivate(targetZone);

            return true;
        }

        private bool TryDefence()
        {
            if (!CheckOpponentAttack()) return false;

            var defenceAbility = GetDefenceAbility();
            if (defenceAbility.Count == 0) return false;

            defenceAbility[Random.Range(0, defenceAbility.Count)].TryActivate(TargetZone.None);

            return true;
        }

        private bool CheckOpponentAttack()
        {
            var ability = _opponentController.Ability;
            var result = ability == AbilityType.Headbutt || ability == AbilityType.HandKick || ability == AbilityType.FootKick ? true : false;
            return result;
        }

        private List<Ability> GetAttackAbility()
        {
            var abilitys = new List<Ability>();
            foreach (var ability in _boxerController.AbilityComponent.Abilities)
            {
                if (ability.State != AbilityState.Available) continue;
                if (ability.Type == AbilityType.Block || ability.Type == AbilityType.Dodge) continue;

                abilitys.Add(ability);
            }

            return abilitys;
        }

        private List<Ability> GetDefenceAbility()
        {
            var abilitys = new List<Ability>();
            foreach (var ability in _boxerController.AbilityComponent.Abilities)
            {
                if (ability.State != AbilityState.Available) continue;
                if (ability.Type == AbilityType.Headbutt || ability.Type == AbilityType.FootKick || ability.Type == AbilityType.FootKick) continue;

                abilitys.Add(ability);
            }

            return abilitys;
        }
        #endregion
    }
}
