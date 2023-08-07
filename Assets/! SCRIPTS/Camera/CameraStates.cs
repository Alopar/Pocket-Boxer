using UnityEngine;
using EventHolder;
using EntityState;
using DG.Tweening;

namespace Gameplay
{
    public partial class CameraController
    {
        public class CinemaCameraState : EntityState<CameraController>
        {
            #region HANDLERS
            private void h_CinemaFinish(CinemaFinishInfo info)
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

                EventHolder<CinemaFinishInfo>.AddListener(h_CinemaFinish, true);
            }

            public override void Exit()
            {
                base.Exit();
                EventHolder<CinemaFinishInfo>.RemoveListener(h_CinemaFinish);
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
            private void h_Input(InputInfo info)
            {
                _currentOffset = new Vector3(_entity._inputCameraOffset * info.Direction.x, _entity._inputCameraOffset * info.Direction.y, _entity._endOffset);
            }

            private void h_Update(EntityState.UpdateType type)
            {
                switch (type)
                {
                    case EntityState.UpdateType.Update:
                        SetCameraOffset();
                        break;
                }
            }

            private void h_CameraLookAt(CameraLookAtInfo info)
            {
                _entity._currentCamera = _entity._observingCamera;

                _entity._playerCamera.Priority = 0;
                _entity._observingCamera.Priority = 10;
                _entity._observingCamera.Follow = info.LookAtPoint.transform;
            }

            private void h_CameraLookPlayer(CameraLookPlayerInfo info)
            {
                _entity._currentCamera = _entity._playerCamera;

                _entity._playerCamera.Priority = 10;
                _entity._observingCamera.Priority = 0;
            }

            private void h_CinemaStart(CinemaStartInfo info)
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
                EventHolder<InputInfo>.AddListener(h_Input, false);
                EventHolder<CinemaStartInfo>.AddListener(h_CinemaStart, false);
                EventHolder<CameraLookAtInfo>.AddListener(h_CameraLookAt, false);
                EventHolder<CameraLookPlayerInfo>.AddListener(h_CameraLookPlayer, false);
            }

            public override void Exit()
            {   
                _entity._transmitter.CommonUpdate -= h_Update;
                EventHolder<InputInfo>.RemoveListener(h_Input);
                EventHolder<CinemaStartInfo>.RemoveListener(h_CinemaStart);
                EventHolder<CameraLookAtInfo>.RemoveListener(h_CameraLookAt);
                EventHolder<CameraLookPlayerInfo>.RemoveListener(h_CameraLookPlayer);
            }
            #endregion
        }
    }
}