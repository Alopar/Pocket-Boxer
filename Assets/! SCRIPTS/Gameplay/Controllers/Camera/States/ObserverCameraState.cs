using UnityEngine;
using Utility.StateMachine;
using DG.Tweening;
using static Gameplay.CameraController;

namespace Gameplay
{
    public class ObserverCameraState : AbstractCameraState
    {
        #region CONSTRUCTORS
        public ObserverCameraState(CameraController entity, StateMachine stateMachine) : base(entity, stateMachine) {}
        #endregion

        #region METHODS PUBLIC
        public override void Enter()
        {
            var beginOffset = new Vector3(0, 0, _startOffset);
            var finalOffset = new Vector3(0, 0, _endOffset);
            DOVirtual.Vector3(beginOffset, finalOffset, _observingTime, (v) => _cameraOffset.m_Offset = v)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _stateMachine.ChangeState<FollowCameraState>());
        }
        #endregion
    }
}
