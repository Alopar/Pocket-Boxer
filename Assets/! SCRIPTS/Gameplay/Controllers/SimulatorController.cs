using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class SimulatorController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;
        [SerializeField] private CurrencyType _currencyType;
        #endregion

        #region FIELDS PRIVATE
        [Find] private RewardComponent _rewardComponent;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            
        }

        private void Update()
        {
            
        }
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        #endregion
    }
}
