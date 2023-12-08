using System;
using UnityEngine;
using DG.Tweening;
using Services.SaveSystem;
using Utility.DependencyInjection;

namespace Gameplay
{
    [SelectionBase]
    public class BuyPlaceController : MonoBehaviour, IInvestable, IDependant
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;
        [SerializeField] private uint _cost;

        [Space(10)]
        [SerializeField] private Transform _view;
        [SerializeField] private Transform _informers;
        [SerializeField] private InvestProgressbar _progressbar;

        [Space(10)]
        [SerializeField] private GameObject _equipment;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private ISaveService _saveService;
        [Find] private Collider _collider;

        private int _invested;
        private bool _available;
        private bool _constructed;
        #endregion

        #region EVENTS
        public event Action<int, int> OnInvest;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            Init();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            LoadProgress();

            if (_constructed)
            {
                PlaceEquipment();
                SelfRemove();
                return;
            }

            if (_available)
            {
                TurnOn();
                HideEquipment();
                DOVirtual.DelayedCall(0.1f, () => { Invest(0); });
                return;
            }

            TurnOff();
            HideEquipment();
        }

        private void LoadProgress()
        {
            var loadData = _saveService.Load<SimulatorSaveData>();
            var simulator = loadData.Simulators.Find(e => e.ID == _id);

            _invested = simulator.Invested;
            _available = simulator.Available;
            _constructed = simulator.Constructed;
        }

        private void SaveProgress()
        {
            var saveData = _saveService.Load<SimulatorSaveData>();
            var simulator = saveData.Simulators.Find(e => e.ID == _id);
            saveData.Simulators.Remove(simulator);

            simulator.Invested = _invested;
            simulator.Available = _available;
            simulator.Constructed = _constructed;

            saveData.Simulators.Add(simulator);

            _saveService.Save(saveData);
        }

        private void TurnOn()
        {
            _collider.enabled = true;
            _progressbar.ShowAvailableState();
        }

        private void TurnOff()
        {
            _collider.enabled = false;
            _progressbar.ShowUnavailableState();
        }

        private void ShowEquipment()
        {
            _equipment.SetActive(true);
        }

        private void HideEquipment()
        {
            _equipment.SetActive(false);
        }

        private void PlaceEquipment()
        {
            _equipment.transform.SetParent(transform.parent);
            _equipment.SetActive(true);
        }

        private void SelfRemove()
        {
            Destroy(gameObject);
        }
        #endregion

        #region METHODS PUBLIC
        public void Invest(uint value)
        {
            _invested += (int)value;
            _constructed = _invested >= _cost;

            SaveProgress();
            OnInvest?.Invoke(_invested, (int)_cost);

            if (_constructed)
            {
                PlaceEquipment();
                SelfRemove();
            }
        }
        #endregion
    }
}
