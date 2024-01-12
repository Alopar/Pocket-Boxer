using System;
using System.Collections.Generic;
using UnityEngine;
using Services.CurrencySystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class WalletComponent : MonoBehaviour
    {
        #region FIELDS PRIVATE
        [Inject] private ICurrencyService _currencyService;
        [Find] private MagnetComponent _magnetComponent;
        #endregion

        #region EVENTS
        public event Action<uint, CurrencyType> OnTokenCollected;
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

                OnTokenCollected?.Invoke(token.Cost, token.Currency);
            };

            callbacks.Enqueue(callback);
            _magnetComponent.MagnetObjectsByCallbacks(callbacks);
        }
        #endregion
    }
}
