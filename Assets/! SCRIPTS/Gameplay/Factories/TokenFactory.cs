using UnityEngine;
using Utility.MonoPool;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class TokenFactory
    {
        #region FIELDS PRIVATE
        [Inject] private ComponentResolver _componentResolver;
        #endregion

        #region METHODS PUBLIC
        public Token Create(Token prefab)
        {
            var token = MonoPool.Instantiate(prefab);
            var children = token.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                _componentResolver.Resolve(child);
                DependencyContainer.Inject(child);
            }

            return token;
        }
        #endregion
    }
}
