using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gameplay
{
    public class InvestProgressbar : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Image _filler;
        [SerializeField] private TextMeshProUGUI _investedText;
        #endregion

        #region FIELDS PRIVATE
        private IInvestable _investable;
        #endregion

        #region HANDLERS
        private void OnInvest(int current, int max)
        {
            _filler.fillAmount = (float)current / max;
            _investedText.text = (max - current).ToString();
        }
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            _investable.OnInvest += OnInvest;
        }

        private void OnDisable()
        {
            _investable.OnInvest -= OnInvest;
        }
        #endregion

        #region METHODS PRIVATE
        private  void Init()
        {
            _investable = GetComponentInParent<IInvestable>();
        }
        #endregion
    }
}
