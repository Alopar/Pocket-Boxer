using System;
using UnityEngine;

namespace Gameplay
{
    public class Manikin : IDisposable
    {
        #region FIELDS PRIVATE
        private readonly GameObject _doll;
        private readonly Animator _animator;
        private readonly EquipmentsComponent _equipments;
        #endregion

        #region CONSTRUCTORS
        public Manikin(GameObject doll, Transform point)
        {
            _doll = doll;
            _doll.transform.position = point.position;
            _doll.transform.rotation = point.rotation;
            _doll.transform.SetParent(point);
            _doll.SetActive(false);

            _animator = doll.GetComponent<Animator>();
            _equipments = doll.GetComponent<EquipmentsComponent>();
        }
        #endregion

        #region METHODS PUBLIC
        public void Activate(CharacterAnimation animation)
        {
            _doll.SetActive(true);
            _equipments.ShowEquipment(animation);
            _animator.Play(animation.ToString());
        }

        public void SetAnimationSpeed(float speed)
        {
            _animator.speed = speed;
        }

        public void Dispose()
        {
            GameObject.Destroy(_doll);
        }
        #endregion
    }
}
