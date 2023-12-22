using System;
using Services.Database;
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
        [Inject] private IDatabaseService _databaseService;
        [Inject] private ISignalService _signalService;
        [Inject] private IScreenService _screenService;
        #endregion

        #region CONSTRUCTORS
        public FightManager()
        {
            TakeDatabaseTables();
            _signalService.Subscribe(this);
        }
        #endregion

        #region HANDLERS
        [Subscribe]
        private void Defeat(Defeat signal)
        {
            if(signal.ControleType == ControleType.AI)
            {
                DOVirtual.DelayedCall(2.5f, () => {
                    _screenService.CloseScreen(ScreenType.ArenaHUD);
                    _screenService.CloseScreen(ScreenType.Ability);
                    _screenService.ShowScreen(ScreenType.Win, (uint)500);
                });
            }
            else
            {
                DOVirtual.DelayedCall(2.5f, () => {
                    _screenService.CloseScreen(ScreenType.ArenaHUD);
                    _screenService.CloseScreen(ScreenType.Ability);
                    _screenService.ShowScreen(ScreenType.Lose, (uint)50);
                });
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void TakeDatabaseTables()
        {
            var db = _databaseService;
            //_statUpgradeTables[StatType.Strength] = db.GetTable<StatsUpgradeData>("StrengthStatsUpgrade") as StatsUpgrade;
            //_statUpgradeTables[StatType.Dexterity] = db.GetTable<StatsUpgradeData>("DexterityStatsUpgrade") as StatsUpgrade;
            //_statUpgradeTables[StatType.Endurance] = db.GetTable<StatsUpgradeData>("EnduranceStatsUpgrade") as StatsUpgrade;
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
