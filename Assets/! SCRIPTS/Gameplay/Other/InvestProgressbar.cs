using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gameplay
{
    public class InvestProgressbar : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private Slider _filler;
        [SerializeField] private TextMeshProUGUI _investedText;

        [Space(10)]
        [SerializeField] private GameObject _availableState;
        [SerializeField] private GameObject _unavailableState;
        #endregion

        #region FIELDS PRIVATE
        private IInvestable _investable;
        #endregion

        #region HANDLERS
        private void OnInvest(int current, int max)
        {
            _filler.value = (float)current / max;
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

        #region METHODS PUBLIC
        public void ShowAvailableState()
        {
            _availableState.SetActive(true);
            _unavailableState.SetActive(false);
        }

        public void ShowUnavailableState()
        {
            _availableState.SetActive(false);
            _unavailableState.SetActive(true);
        }
        #endregion
    }
}
