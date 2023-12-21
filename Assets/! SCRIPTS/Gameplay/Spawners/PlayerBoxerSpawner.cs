using UnityEngine;
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
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            SpawnBoxer(_boxerPrefab);
        }
        #endregion

        #region METHODS PUBLIC
        public void SpawnBoxer(BoxerController boxerPrefab)
        {
            var boxer = _boxerFactory.Create(boxerPrefab);
            boxer.transform.position = transform.position;
            boxer.transform.rotation = transform.rotation;

            //TODO:
            boxer.SetStats(2, 3, 2);

            _signalService.Send<PlayerBoxerSpawn>(new(boxer));
        }
        #endregion
    }
}
