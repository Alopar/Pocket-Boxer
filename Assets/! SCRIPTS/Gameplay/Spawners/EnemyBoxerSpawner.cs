using System;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Managers;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class EnemyBoxerSpawner : MonoBehaviour, IDependant
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<BoxerCountry> _boxers;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Inject] private FightManager _fightManager;
        [Inject] private BoxerFactory _boxerFactory;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            SpawnBoxer();
        }
        #endregion

        #region METHODS PUBLIC
        public void SpawnBoxer()
        {
            var enemy = _fightManager.EnemyData;
            var country = (Country)Enum.Parse(typeof(Country), enemy.Country);
            var prefab = _boxers.Find(e => e.Country == country).BoxerPrefab;

            var boxer = _boxerFactory.Create(prefab);
            boxer.transform.position = transform.position;
            boxer.transform.rotation = transform.rotation;

            boxer.SetStats((int)enemy.Strength, (int)enemy.Dexterity, (int)enemy.Endurance);

            _signalService.Send<EnemyBoxerSpawn>(new(boxer));
        }
        #endregion
    }

    [Serializable]
    public class BoxerCountry
    {
        public Country Country;
        public BoxerController BoxerPrefab;
    }
}
