using Services.SaveSystem;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class DependencyTest : MonoBehaviour
    {
        #region FIELDS PRIVATE
        [Inject]
        private ISaveService _saveService;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            GameDependenciesContext.Inject(this);
        }

        private void Update()
        {
            var loadData = _saveService.Load<CurrencySaveData>();
            print(loadData.Money);
        }
        #endregion
    }
}
