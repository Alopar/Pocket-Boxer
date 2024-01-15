using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gameplay
{
    public class FloatupStats : FloatupNumeric
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Color _strengthColor;
        [SerializeField] private Color _dexterityColor;
        [SerializeField] private Color _enduranceColor;

        [Space(10)]
        [SerializeField] private Sprite _strengthSprite;
        [SerializeField] private Sprite _dexteritySprite;
        [SerializeField] private Sprite _enduranceSprite;

        [Space(10)]
        [SerializeField] private Image _statIcon;
        [SerializeField] private TextMeshProUGUI _statText;
        #endregion

        #region METHODS PUBLIC
        public void Init(int number, StatType type)
        {
            var text = "";
            var color = Color.white;
            var sprite = _strengthSprite;
            switch (type)
            {
                case StatType.Strength:
                    text = "damage";
                    color = _strengthColor;
                    sprite = _strengthSprite;
                    break;
                case StatType.Dexterity:
                    text = "speed";
                    color = _dexterityColor;
                    sprite = _dexteritySprite;
                    break;
                case StatType.Endurance:
                    text = "health";
                    color = _enduranceColor;
                    sprite = _enduranceSprite;
                    break;
            }

            _statText.text = text; 
            _statText.color = color;

            _statIcon.sprite = sprite;
            _statIcon.color = color;

            SetTextColor(color);
            Init(1);
        }
        #endregion
    }
}
