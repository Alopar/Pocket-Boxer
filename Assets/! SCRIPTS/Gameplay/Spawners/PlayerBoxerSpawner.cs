using UnityEngine;
using Gameplay.Managers;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class PlayerBoxerSpawner : MonoBehaviour, IDependant
    {
        #region FIELDS INSPECTOR
        [SerializeField] private BoxerController _boxerPrefab;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Inject] private BoxerFactory _boxerFactory;
        [Inject] private StatsManager _statsManager;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            SpawnBoxer(_boxerPrefab);
        }
        #endregion

        #region METHODS PRIVATE
        private void SpawnBoxer(BoxerController boxerPrefab)
        {
            var boxer = _boxerFactory.Create(boxerPrefab);
            boxer.transform.position = transform.position;
            boxer.transform.rotation = transform.rotation;

            var strength = _statsManager.GetLevel(StatType.Strength);
            var dextetity = _statsManager.GetLevel(StatType.Dexterity);
            var endurance = _statsManager.GetLevel(StatType.Endurance);
            boxer.SetStats(strength, dextetity, endurance);
            boxer.SetName("Player");

            _signalService.Send<PlayerBoxerSpawn>(new(boxer));
        }
        #endregion

    }
}
