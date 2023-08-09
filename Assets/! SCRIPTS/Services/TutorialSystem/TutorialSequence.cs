using System.Collections.Generic;
using UnityEngine;

namespace Services.TutorialSystem
{
    [CreateAssetMenu(fileName = "NewTutorialSequence", menuName = "Tutorial/Sequence", order = 0)]
    public class TutorialSequence : ScriptableObject
    {
        #region FIELDS INSPECTOR
        [SerializeField] private List<TutorialPart> _sequence;
        #endregion

        #region PROPERTIES
        public List<TutorialPart> Sequence => _sequence;
        #endregion
    }
}
