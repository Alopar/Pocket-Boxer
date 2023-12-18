using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class BoxerFactory
    {
        #region FIELDS PRIVATE
        [Inject] private ComponentResolver _componentResolver;
        #endregion

        #region METHODS PUBLIC
        public BoxerController Create(BoxerController prefab)
        {
            var tempAction = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);

            var player = GameObject.Instantiate(prefab);
            player.name = prefab.name;

            var children = player.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                _componentResolver.Resolve(child);
                DependencyContainer.Inject(child);
            }

            player.gameObject.SetActive(tempAction);
            prefab.gameObject.SetActive(tempAction);

            return player;
        }
        #endregion
    }
}
