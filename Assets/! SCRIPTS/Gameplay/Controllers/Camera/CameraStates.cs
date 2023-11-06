using UnityEngine;
using EntityState;
using Services.SignalSystem;
using Services.SignalSystem.Signals;
using DG.Tweening;


namespace Gameplay
{
    public partial class CameraController
    {
        public class CinemaCameraState : EntityState<CameraController>
        {
            #region HANDLERS
            [Subscribe]
            private void h_CinemaFinish(CinemaFinish info)
            {
                _entity.ChangeState(new FollowCameraState());
            }
            #endregion

            #region METHODS PUBLIC
            public override void Enter(EntityMonoBehavior entity)
            {
                base.Enter(entity);

                _entity._playerCamera.Priority = 0;
                _entity._observingCamera.Priority = 0;

                _entity._signalsService.Subscribe(this);
            }

            public override void Exit()
            {
                base.Exit();
                _entity._signalsService.Unsubscribe(this);
            }
            #endregion
        }

        public class ObserverCameraState : EntityState<CameraController>
        {
            #region METHODS PUBLIC
            public override void Enter(EntityMonoBehavior entity)
            {
                base.Enter(entity);

                var beginOffset = new Vector3(0, 0, _entity._startOffset);
                var finalOffset = new Vector3(0, 0, _entity._endOffset);
                DOVirtual.Vector3(beginOffset, finalOffset, _entity._observingTime, (v) => _entity._cameraOffset.m_Offset = v)
                    .SetEase(Ease.OutCubic)
                    .OnComplete(() => _entity.ChangeState(new FollowCameraState()));
            }
            #endregion
        }

        public class FollowCameraState : EntityState<CameraController>
        {
            #region FIELDS PRIVATE
            private Vector3 _currentOffset = Vector3.zero;
            #endregion

            #region HANDLERS
            [Subscribe(false)]
            private void h_Input(Services.SignalSystem.Signals.InputDirection info)
            {
                _currentOffset = new Vector3(_entity._inputCameraOffset * info.Direction.x, _entity._inputCameraOffset * info.Direction.y, _entity._endOffset);
            }

            [Subscribe(false)]
            private void h_Update(EntityState.UpdateType type)
            {
                switch (type)
                {
                    case EntityState.UpdateType.Update:
                        SetCameraOffset();
                        break;
                }
            }

            [Subscribe(false)]
            private void h_CameraLookAt(CameraLookAt info)
            {
                _entity._currentCamera = _entity._observingCamera;

                _entity._playerCamera.Priority = 0;
                _entity._observingCamera.Priority = 10;
                _entity._observingCamera.Follow = info.LookAtPoint.transform;
            }

            [Subscribe(false)]
            private void h_CameraLookPlayer(CameraLookPlayer info)
            {
                _entity._currentCamera = _entity._playerCamera;

                _entity._playerCamera.Priority = 10;
                _entity._observingCamera.Priority = 0;
            }

            [Subscribe(false)]
            private void h_CinemaStart(CinemaStart info)
            {
                _entity.ChangeState(new CinemaCameraState());
            }
            #endregion

            #region METHODS PRIVATE
            private void SetCameraOffset()
            {
                _entity._cameraOffset.m_Offset = _entity._cameraOffset.m_Offset.sqrMagnitude <= 0.001f ? Vector3.zero : _entity._cameraOffset.m_Offset;
                if (_currentOffset == Vector3.zero && _entity._cameraOffset.m_Offset == Vector3.zero) return;

                _entity._cameraOffset.m_Offset = Vector3.Lerp(_entity._cameraOffset.m_Offset, _currentOffset, Time.deltaTime * 2f);
                _currentOffset = Vector3.zero;
            }
            #endregion

            #region METHODS PUBLIC
            public override void Enter(EntityMonoBehavior entity)
            {
                base.Enter(entity);

                _entity._playerCamera.Priority = 10;
                _entity._observingCamera.Priority = 0;

                _entity._transmitter.CommonUpdate += h_Update;
                _entity._signalsService.Subscribe(this);
            }

            public override void Exit()
            {   
                _entity._transmitter.CommonUpdate -= h_Update;
                _entity._signalsService.Unsubscribe(this);
            }
            #endregion
        }
    }
}