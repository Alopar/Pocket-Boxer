using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay;
using EventHolder;
using Utility;
using Utility.Storage;
using DG.Tweening;
using UnityEngine.AI;

namespace Manager
{
    [DefaultExecutionOrder(-5)]
    public class GameManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Header("BASE SETTINGS:")]
        [SerializeField] private PlayerController _playerPrefab;

        [Header("DATA BASE:")]
        #endregion

        #region FIELDS PRIVATE
        private static GameManager _instance;

        private MoneyDeposite _moneyWallet;
        private DiamondDeposite _diamondWallet;

        private DateTime _startTime;
        #endregion

        #region PROPERTIES
        public static GameManager Instance => _instance;

        public MoneyDeposite MoneyWallet => _moneyWallet;
        public DiamondDeposite DiamondWallet => _diamondWallet;
        #endregion

        #region HANDLERS
        private void SceneLoadedHandler(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name == "MoonSDKScene") return;

            _moneyWallet.SetCurrency(0);
            _diamondWallet.SetCurrency(0);

            Bootstrap();
        }
        #endregion

        #region UNITY CALLBACKS
        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedHandler;
        }

        private void OnDisable()
        {   
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                Init();
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnApplicationQuit()
        {
            // app metrica
            //AnalyticsFinishLevel("lose", 0);
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            DOTween.Init();
            Application.targetFrameRate = GameSettings.Data.ApplicationFrameRate;

            _moneyWallet = new MoneyDeposite(new IntPlayerPrefStorage("MONEY"));
            _diamondWallet = new DiamondDeposite(new IntPlayerPrefStorage("DIAMOND"));
        }

        private void Bootstrap()
        {
            NavMesh.avoidancePredictionTime = 0.5f;

            var playerSpawnPoint = GameObject.FindWithTag("PlayerSpawner");
            var player = Instantiate(_playerPrefab, playerSpawnPoint.transform.position, Quaternion.identity);
            player.Init();

            EventHolder<PlayerSpawnInfo>.NotifyListeners(new PlayerSpawnInfo(player));
            EventHolder<GameplayEventInfo>.NotifyListeners(new(GameplayEvent.StartGame));
        }
        #endregion

        #region METHODS PUBLIC
#if UNITY_EDITOR
        [ContextMenu("* ADD MONEY")]
        public void AddMoney()
        {
            _moneyWallet.SetCurrency(100000);
        }

        [ContextMenu("* ADD DIAMOND")]
        public void AddDiamond()
        {
            _diamondWallet.SetCurrency(1000);
        }
#endif
        #endregion
    }
}