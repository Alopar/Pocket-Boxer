using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.Managers;
using Utility.DependencyInjection;

namespace Gameplay
{
    [SelectionBase]
    public class RingController : MonoBehaviour, IDependant
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Image _flag;
        [SerializeField] private Image _photo;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _strengthText;
        [SerializeField] private TextMeshProUGUI _dexterityText;
        [SerializeField] private TextMeshProUGUI _enduranceText;

        [Space(10)]
        [SerializeField] private List<EnemyContent> _enemyContent;
        #endregion

        #region FIELDS PRIVATE
        [Inject] private FightManager _fightManager;
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            SetEnemyData();
        }
        #endregion

        #region METHODS PRIVATE
        private void SetEnemyData()
        {
            var enemy = _fightManager.EnemyData;
            _name.text = enemy.Name;
            _strengthText.text = enemy.Strength.ToString();
            _dexterityText.text = enemy.Dexterity.ToString();
            _enduranceText.text = enemy.Endurance.ToString();

            var country = (Country)Enum.Parse(typeof(Country), enemy.Country);
            var content = _enemyContent.Find(e => e.Country == country);
            _flag.sprite = content.Flag;
            _photo.sprite = content.Photo;
        }
        #endregion
    }
}
