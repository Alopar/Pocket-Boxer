using UnityEngine;
using Utility.DependencyInjection;

namespace Services.ScreenSystem
{
    public class ScreenFactory
    {
        #region FIELDS PRIVATE
        [Inject] private ComponentDependencyResolver _componentResolver;
        #endregion

        #region METHODS PUBLIC
        public AbstractScreen Create(AbstractScreen prefab)
        {
            var tempAction = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);

            var instance = GameObject.Instantiate(prefab);
            var children = instance.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                _componentResolver.Resolve(child);
                DependencyContainer.Inject(child);
            }

            instance.gameObject.SetActive(tempAction);
            prefab.gameObject.SetActive(tempAction);

            return instance;
        }
        #endregion
    }
}
