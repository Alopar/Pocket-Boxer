using EventHolder;
using UnityEngine;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class PlayerSpawner : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private PlayerController _playerPrefab;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            SpawnPlayer(_playerPrefab);
        }
        #endregion

        #region METHODS PUBLIC
        public void SpawnPlayer(PlayerController playerPrefab)
        {
            var player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            DependenciesContext.Inject(player);

            player.transform.SetParent(transform.parent);

            EventHolder<PlayerSpawnInfo>.NotifyListeners(new PlayerSpawnInfo(player));
        }
        #endregion
    }
}
