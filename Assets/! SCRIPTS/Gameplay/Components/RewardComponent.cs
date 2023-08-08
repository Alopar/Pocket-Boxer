using System.Threading.Tasks;
using UnityEngine;
using Utility.MonoPool;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class RewardComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private MonoBehaviour _treasurePrefab;

        [Space(10)]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField, Range(0, 1)] private float _spawnDelay;
        [SerializeField, MinMaxSlider(0, 10), Tooltip("Random spread from - to")] private Vector2 _randomNumber;

        [Space(10)]
        [SerializeField, Range(0, 10)] private float _impulsePower;
        [SerializeField, Range(0, 10)] private float _torquePower;
        #endregion

        #region METHODS PUBLIC
        public void GiveOutReward()
        {
            TreasureStream(_treasurePrefab, _spawnPoint.position, _spawnPoint.up, _spawnDelay, Random.Range((int)_randomNumber.x, (int)_randomNumber.y));
        }
        #endregion

        #region COROUTINES
        private async void TreasureStream(MonoBehaviour prefab, Vector3 spawnPosition, Vector3 spawnDirection, float delay, int number)
        {
            var impulsePower = _impulsePower;
            var torquePower = _torquePower;
            for (int i = 0; i < number; i++)
            {
                var treasure = MonoPool.Instantiate(prefab) as Cargo;
                treasure.transform.position = spawnPosition + (Random.insideUnitSphere * 0.5f);

                var force = spawnDirection * impulsePower;
                var torque = Random.onUnitSphere * torquePower;
                treasure.GiveKick(force, torque);

                await Task.Delay((int)(delay * 1000f));
            }
        }
        #endregion
    }
}
