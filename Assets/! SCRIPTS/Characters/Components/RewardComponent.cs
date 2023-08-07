using System.Threading.Tasks;
using UnityEngine;
using Utility.MonoPool;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class RewardComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private uint _reward;
        [SerializeField] private MonoBehaviour _treasurePrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField, Range(0, 1)] private float _spawnDelay;
        [SerializeField, Range(1, 30)] private int _number;
        #endregion

        #region FIELDS PRIVATE
        private PlayerController _player;
        #endregion

        #region METHODS PUBLIC
        public void Init(PlayerController player)
        {
            _player = player;
        }

        public void GiveOutReward()
        {
            TreasureStream(_treasurePrefab, _spawnPoint.position, _player, _spawnDelay, _number, _reward);
        }

        public void GiveOutReward(uint currencyAmount)
        {
            TreasureStream(_treasurePrefab, _spawnPoint.position, _player, _spawnDelay, _number, currencyAmount);
        }
        #endregion

        #region COROUTINES
        private async void TreasureStream(MonoBehaviour prefab, Vector3 spawnPosition, PlayerController player, float delay, int number, uint money)
        {
            int moneyCounter = (int)money;
            int moneyDelta = moneyCounter / number;

            var counter = number;
            while (counter > 0)
            {
                moneyCounter = counter > 1 ? moneyCounter - moneyDelta : moneyCounter;
                var moneyAmount = counter > 1 ? moneyDelta : moneyCounter;

                var treasure = MonoPool.Instantiate(prefab);
                treasure.transform.position = spawnPosition + Random.insideUnitSphere;
                counter--;

                await Task.Delay((int)(delay * 1000f));
            }
        }
        #endregion
    }
}
