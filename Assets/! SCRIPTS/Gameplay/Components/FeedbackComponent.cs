using UnityEngine;
using Gameplay.Managers;
using Services.CurrencySystem;
using Utility.MonoPool;
using Utility.DependencyInjection;
using Lofelt.NiceVibrations;

namespace Gameplay
{
    public class FeedbackComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Transform _floatupNumericPoint;

        [Space(10)]
        [SerializeField] private FloatupToken _floatupTokenPrefab;
        [SerializeField] private FloatupStats _floatupStatsPrefab;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private StatsManager _statsManager;
        [Find] private WalletComponent _walletComponent;
        #endregion

        #region HANDLERS
        private void TokenCollected(uint number, CurrencyType currency)
        {
            LaunchTokenNumeric((int)number, currency);
        }

        private void StatChange(StatType statType, int value)
        {
            LaunchStatNumeric(value, statType);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _statsManager.OnStatChange += StatChange;
            _walletComponent.OnTokenCollected += TokenCollected;
        }

        private void OnDisable()
        {
            _statsManager.OnStatChange -= StatChange;
            _walletComponent.OnTokenCollected -= TokenCollected;
        }
        #endregion

        #region METHODS PRIVATE
        private void LaunchTokenNumeric(int number, CurrencyType currency)
        {
            var floatupNumber = MonoPool.Instantiate(_floatupTokenPrefab);
            floatupNumber.Init(number, currency);
            floatupNumber.transform.position = _floatupNumericPoint.position;

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        private void LaunchStatNumeric(int number, StatType type)
        {
            var floatupNumber = MonoPool.Instantiate(_floatupStatsPrefab);
            floatupNumber.Init(number, type);
            floatupNumber.transform.position = _floatupNumericPoint.position;

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        #endregion
    }
}
