using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class EquipmentsComponent : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<AnimationEquipment> _equipments;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            HideEquipments();
        }
        #endregion

        #region METHODS PUBLIC
        public void ShowEquipment(CharacterAnimation animation)
        {
            HideEquipments();
            var equipment = _equipments.Find(e => e.Animation == animation);
            equipment?.Equipments.ForEach(e => e.SetActive(true));
        }

        public void HideEquipments()
        {
            foreach (var equipment in _equipments)
            {
                equipment.Equipments.ForEach(e => e.SetActive(false));
            }
        }
        #endregion
    }
}
