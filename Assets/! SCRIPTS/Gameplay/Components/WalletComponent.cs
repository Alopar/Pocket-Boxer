using System;
using System.Collections.Generic;
using UnityEngine;
using Services.CurrencySystem;
using Utility.DependencyInjection;
using Lofelt.NiceVibrations;

namespace Gameplay
{
    public class WalletComponent : MonoBehaviour
    {
        #region FIELDS PRIVATE
        [Inject] private ICurrencyService _currencyService;
        [Find] private MagnetComponent _magnetComponent;
        #endregion

        #region UNITY CALLBACKS
        private void Update()
        {
            CollectTokens();
        }
        #endregion

        #region METHODS PRIVATE
        private void CollectTokens()
        {
            var callbacks = new Queue<Action<Transform>>();
            Action<Transform> callback = (Transform target) => {
                var token = target.GetComponent<Token>();
                _currencyService.PutCurrency(token.Currency, token.Cost);
                token.Delete();
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            };

            callbacks.Enqueue(callback);
            _magnetComponent.MagnetObjectsByCallbacks(callbacks);
        }
        #endregion
    }
}
