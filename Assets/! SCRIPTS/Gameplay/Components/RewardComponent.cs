using System.Threading.Tasks;
using UnityEngine;
using Utility.MonoPool;
using DG.Tweening;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class RewardComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Token _strengthTokenPrefab;
        [SerializeField] private Token _dexterityTokenPrefab;
        [SerializeField] private Token _endunranceTokenPrefab;

        [Space(10)]
        [SerializeField] private Transform _spawnPoint;
        [SerializeField, Range(0, 1)] private float _spawnDelay;
        [SerializeField, MinMaxSlider(0, 5)] private Vector2 _randomDistance;

        [Space(10)]
        [SerializeField, Range(0, 3)] private int _numJumps;
        [SerializeField, Range(0, 5)] private float _jumpPower;
        [SerializeField, Range(0, 3)] private float _jumpDuration;
        #endregion

        #region METHODS PUBLIC
        public void GiveOutReward(CurrencyType currency, uint cost, int number)
        {
            Token prefab = null;
            switch (currency)
            {
                case CurrencyType.StrengthPoints:
                    prefab = _strengthTokenPrefab;
                    break;
                case CurrencyType.DexterityPoints:
                    prefab = _dexterityTokenPrefab;
                    break;
                case CurrencyType.EndurancePoints:
                    prefab = _endunranceTokenPrefab;
                    break;
            }

            TokenStream(prefab, _spawnPoint.position, _spawnDelay, number, cost);
        }

        //TODO: delete test code
        [Button("DROP STRENGTH TOKEN")]
        public void DropStrengthToken()
        {
            GiveOutReward(CurrencyType.StrengthPoints, 10, 3);
        }

        [Button("DROP DEXTERITY TOKEN")]
        public void DropDexterityToken()
        {
            GiveOutReward(CurrencyType.DexterityPoints, 10, 3);
        }

        [Button("DROP ENDURANCE TOKEN")]
        public void DropEnduranceToken()
        {
            GiveOutReward(CurrencyType.EndurancePoints, 10, 3);
        }
        #endregion

        #region COROUTINES
        private async void TokenStream(Token prefab, Vector3 spawnPosition, float delay, int number, uint cost)
        {
            for (int i = 0; i < number; i++)
            {
                //TODO: implement factory
                var token = MonoPool.Instantiate(prefab);
                MonoInjector.Inject(token);
                token.Init(cost);

                token.transform.position = spawnPosition;
                var direction = Random.insideUnitCircle.normalized * Random.Range(_randomDistance.x, _randomDistance.y);
                var jumpPosition = spawnPosition + new Vector3(direction.x, spawnPosition.y, direction.y);
                token.transform.DOJump(jumpPosition, _jumpPower, _numJumps, _jumpDuration);

                await Task.Delay((int)(delay * 1000f));
            }
        }
        #endregion
    }
}
