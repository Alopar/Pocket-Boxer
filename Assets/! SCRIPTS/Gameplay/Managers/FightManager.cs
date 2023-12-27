using System;
using Services.Database;
using Services.SaveSystem;
using Services.ScreenSystem;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using Utility.DependencyInjection;
using DG.Tweening;

namespace Gameplay.Managers
{
    public class FightManager : IDisposable
    {
        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;
        [Inject] private ISignalService _signalService;
        [Inject] private IScreenService _screenService;
        [Inject] private IDatabaseService _databaseService;

        private ushort _enemyLevel;
        private Enemies _enemyTable;
        #endregion

        #region PROPERTIES
        public ushort EnemyLevel => _enemyLevel;
        public EnemyData EnemyData => _enemyTable.GetDataByIndex((uint)_enemyLevel - 1);
        #endregion

        #region CONSTRUCTORS
        public FightManager()
        {
            LoadData();
            TakeDatabaseTables();
            _signalService.Subscribe(this);
        }
        #endregion

        #region HANDLERS
        [Subscribe]
        private void Defeat(Defeat signal)
        {
            var enemy = _enemyTable.GetDataByIndex((uint)_enemyLevel - 1);

            if(signal.ControleType == ControleType.AI)
            {
                DOVirtual.DelayedCall(2.5f, () => {
                    _screenService.CloseScreen(ScreenType.ArenaHUD);
                    _screenService.CloseScreen(ScreenType.Ability);
                    _screenService.ShowScreen(ScreenType.Win, enemy.Cost);

                    LevelUp();
                });
            }
            else
            {
                DOVirtual.DelayedCall(2.5f, () => {
                    _screenService.CloseScreen(ScreenType.ArenaHUD);
                    _screenService.CloseScreen(ScreenType.Ability);
                    _screenService.ShowScreen(ScreenType.Lose, enemy.Cost / 10);
                });
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void LoadData()
        {
            var loadData = _saveService.Load<FightData>();
            _enemyLevel = loadData.EnemyLevel;
        }

        private void SaveData()
        {
            var saveData = new FightData();
            saveData.EnemyLevel = _enemyLevel;

            _saveService.Save(saveData);
        }

        private void TakeDatabaseTables()
        {
            var db = _databaseService;
            _enemyTable = db.GetTable<EnemyData>("Enemies") as Enemies;
        }

        private void LevelUp()
        {
            _enemyLevel++;
            SaveData();
        }
        #endregion

        #region METHODS PUBLIC
        public void Dispose()
        {
            _signalService.Unsubscribe(this);
        }
        #endregion
    }
}
