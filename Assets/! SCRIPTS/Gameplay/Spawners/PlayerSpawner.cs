using UnityEngine;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;

namespace Gameplay
{
    public class PlayerSpawner : MonoBehaviour, IDependant
    {
        #region FIELDS INSPECTOR
        [SerializeField] private PlayerController _playerPrefab;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISignalService _signalService;
        [Inject] private PlayerFactory _playerFactory;
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
            var player = _playerFactory.Create(playerPrefab);
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

            _signalService.Send<PlayerSpawn>(new(player));
        }
        #endregion
    }
}
