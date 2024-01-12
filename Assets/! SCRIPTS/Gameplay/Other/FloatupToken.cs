using Services.CurrencySystem;
using UnityEngine;

namespace Gameplay
{
    public class FloatupToken : FloatupNumeric
    {
        #region FIELDS INSPECTOR
        [Space(10)]
        [SerializeField] private Color _strengthColor;
        [SerializeField] private Color _dexterityColor;
        [SerializeField] private Color _enduranceColor;
        #endregion

        #region METHODS PUBLIC
        public void Init(int number, CurrencyType type)
        {
            var color = Color.white;
            switch (type)
            {
                case CurrencyType.StrengthPoints:
                    color = _strengthColor;
                    break;
                case CurrencyType.DexterityPoints:
                    color = _dexterityColor;
                    break;
                case CurrencyType.EndurancePoints:
                    color = _enduranceColor;
                    break;
            }
            SetTextColor(color);
            Init(number);
        }
        #endregion
    }
}
