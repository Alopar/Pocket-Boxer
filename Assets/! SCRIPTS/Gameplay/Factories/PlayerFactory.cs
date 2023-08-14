using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class PlayerFactory
    {
        public PlayerController Create(PlayerController prefab)
        {
            var tempAction = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);

            var player = GameObject.Instantiate(prefab);
            var children = player.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                MonoInjector.Inject(child);
                DependencyContainer.Inject(child);
            }

            player.gameObject.SetActive(tempAction);
            prefab.gameObject.SetActive(tempAction);

            return player;
        }
    }
}