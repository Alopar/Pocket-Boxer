using UnityEngine;
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
        [SerializeField] private FloatupToken _FloatupTokenPrefab;
        #endregion

        #region FIELDS PRIVATE
        [Find] private WalletComponent _walletComponent;
        #endregion

        #region HANDLERS
        private void TokenCollected(uint number, CurrencyType currency)
        {
            LaunchTokenNumeric((int)number, currency);
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            _walletComponent.OnTokenCollected += TokenCollected;
        }

        private void OnDisable()
        {
            _walletComponent.OnTokenCollected -= TokenCollected;
        }
        #endregion

        #region METHODS PRIVATE
        private void LaunchTokenNumeric(int number, CurrencyType currency)
        {
            var floatupNumber = MonoPool.Instantiate(_FloatupTokenPrefab);
            floatupNumber.Init(number, currency);
            floatupNumber.transform.position = _floatupNumericPoint.position;

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
        #endregion
    }
}
