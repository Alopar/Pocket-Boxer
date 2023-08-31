using System;
using UnityEngine;
using Services.SignalSystem;

namespace Gameplay
{
    public class BatteryComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField, Range(0, 1000)] private int _capacity = 200;
        #endregion

        #region FIELDS PRIVATE
        private int _occupied = 0;
        #endregion

        #region PROPERTIES
        public float Occupied => _occupied;
        public bool IsFull => (float)_occupied / _capacity == 1f;
        #endregion

        #region METHODS PRIVATE
        private void ChangeCapacityByValue(int value)
        {
            _capacity += value;
            SignalSystem<BatteryOccupiedInfo>.Send(new(_capacity, _occupied));
        }

        private void ChangeOccupiedByValue(int value)
        {
            _occupied += value;
            _occupied = Math.Clamp(_occupied, 0, _capacity);
            SignalSystem<BatteryOccupiedInfo>.Send(new(_capacity, _occupied));
        }
        #endregion

        #region METHODS PUBLIC
        public void Init()
        {
            ChangeOccupiedByValue(_capacity);
        }

        public bool TryGetEnergy(uint value)
        {
            if(_occupied < value) return false;
            ChangeOccupiedByValue(-(int)value);
            return true;
        }

        public bool TrySetEnergy(uint value)
        {
            ChangeOccupiedByValue((int)value);
            if (_occupied == _capacity) return false;
            return true;
        }

        public void SetCapacity(int value)
        {
            _capacity = value;
            SignalSystem<BatteryOccupiedInfo>.Send(new(_capacity, _occupied));
        }
        #endregion
    }
}
