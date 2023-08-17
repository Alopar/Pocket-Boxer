using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.DependencyInjection;
using Lofelt.NiceVibrations;

namespace Gameplay
{
    public class WalletComponent : MonoBehaviour
    {
        #region FIELDS PRIVATE
        [Inject] private IWalletService _walletService;
        [MonoInject] private MagnetComponent _magnetComponent;
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
                switch (token.Currency)
                {
                    case CurrencyType.Money:
                        _walletService.SetCurrency<MoneyDeposite>(token.Cost);
                        break;
                    case CurrencyType.Diamond:
                        _walletService.SetCurrency<DiamondDeposite>(token.Cost);
                        break;
                    case CurrencyType.ExperiencePoints:
                        _walletService.SetCurrency<ExperiencePointsDeposite>(token.Cost);
                        break;
                    case CurrencyType.StrengthPoints:
                        _walletService.SetCurrency<StrengthPointsDeposite>(token.Cost);
                        break;
                    case CurrencyType.DexterityPoints:
                        _walletService.SetCurrency<DexterityPointsDeposite>(token.Cost);
                        break;
                    case CurrencyType.EndurancePoints:
                        _walletService.SetCurrency<EndurancePointsDeposite>(token.Cost);
                        break;
                }

                token.Delete();
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            };

            callbacks.Enqueue(callback);
            _magnetComponent.MagnetObjectsByCallbacks(callbacks);
        }
        #endregion
    }
}
