using System;
using UnityEngine;
using Manager;
using DG.Tweening;
using Services.SaveSystem;
using Services.ServiceLocator;
using EventHolder;

namespace Gameplay
{
    [SelectionBase]
    public class ConstructionYardController : MonoBehaviour, IInvestable
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _id;
        [SerializeField] private uint _cost;

        [Space(10)]
        [SerializeField] private Transform _view;
        [SerializeField] private Transform _informers;

        [Space(10)]
        [SerializeField] private GameObject _building;
        #endregion

        #region FIELDS PRIVATE
        private ISaveService _saveService;

        private Collider _collider;

        private int _invested;
        private bool _available;
        private bool _constructed;
        #endregion

        #region EVENTS
        public event Action<int, int> OnInvest;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }
        #endregion

        #region METHODS PRIVATE
        private void Init()
        {
            ResolveDependency();
            //LoadProgress();

            if (_constructed)
            {
                TurnOff();
                ShowBuilding();
                return;
            }

            if (_available)
            {
                TurnOn();
                HideBuilding();
                DOVirtual.DelayedCall(0.1f, () => { Invest(0); });
                return;
            }

            TurnOff();
            HideBuilding();
        }

        private void ResolveDependency()
        {
            _saveService = ServiceLocator.GetService<ISaveService>();
            _collider = GetComponent<Collider>();
        }

        //private void LoadProgress()
        //{
        //    var loadData = _saveService.Load<BuildingSaveData>();
        //    var building = loadData.Buildings.Find(e => e.ID == _id);

        //    _invested = building.Invested;
        //    _available = building.Available;
        //    _constructed = building.Constructed;
        //}

        //private void SaveProgress()
        //{
        //    var saveData = _saveService.Load<BuildingSaveData>();
        //    var building = saveData.Buildings.Find(e => e.ID == _id);
        //    saveData.Buildings.Remove(building);

        //    building.Invested = _invested;
        //    building.Available = _available;
        //    building.Constructed = _constructed;

        //    saveData.Buildings.Add(building);

        //    _saveService.Save(saveData);
        //}

        private void TurnOn()
        {
            _collider.enabled = true;
            _informers.gameObject.SetActive(true);
        }

        private void TurnOff()
        {
            _collider.enabled = false;
            _informers.gameObject.SetActive(false);
        }

        private void ShowBuilding()
        {
            _building.SetActive(true);
        }

        private void HideBuilding()
        {
            _building.SetActive(false);
        }
        #endregion

        #region METHODS PUBLIC
        public void Invest(uint value)
        {
            _invested += (int)value;
            _constructed = _invested >= _cost;
            if (_constructed)
            {
                TurnOff();
                ShowBuilding();

                EventHolder<GameplayEventInfo>.NotifyListeners(new(GameplayEvent.ChargerBuyed));
                EventHolder<TutorialObservingInfo>.NotifyListeners(null);
            }

            //SaveProgress();
            OnInvest?.Invoke(_invested, (int)_cost);
        }
        #endregion
    }
}
