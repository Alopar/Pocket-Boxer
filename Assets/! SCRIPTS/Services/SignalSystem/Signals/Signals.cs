using UnityEngine;
using UnityEngine.Events;
using Gameplay;
using Screens.Layers.Arena;

namespace Services.SignalSystem.Signals
{
    #region CAMERA
    public readonly struct CameraOffset : ISignal
    {
        public readonly Vector3 Offset;

        public CameraOffset(Vector3 offset)
        {
            Offset = offset;
        }
    }

    public readonly struct CameraLookAt : ISignal
    {
        public readonly Transform LookAtPoint;

        public CameraLookAt(Transform lookAtPoint)
        {
            LookAtPoint = lookAtPoint;
        }
    }

    public readonly struct CameraLookPlayer : ISignal
    {
        // N/A
    }

    public readonly struct CameraChangeFOV : ISignal
    {
        public readonly float FOV;

        public CameraChangeFOV(float fov)
        {
            FOV = fov;
        }
    }
    #endregion

    #region PLAYER
    public readonly struct PlayerSpawn : ISignal
    {
        public readonly PlayerController PlayerController;

        public PlayerSpawn(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }

    public readonly struct BoxerSpawn : ISignal
    {
        public readonly BoxerController BoxerController;

        public BoxerSpawn(BoxerController boxerController)
        {
            BoxerController = boxerController;
        }
    }

    public readonly struct Strike : ISignal
    {
        public readonly AbilityType Ability;
        public readonly ControleType ControleType;

        public Strike(AbilityType ability, ControleType controleType)
        {
            Ability = ability;
            ControleType = controleType;
        }
    }

    public readonly struct BatteryOccupied : ISignal
    {
        public readonly int Capacity;
        public readonly int Occupied;

        public BatteryOccupied(int capacity, int occupied)
        {
            Capacity = capacity;
            Occupied = occupied;
        }
    }

    public readonly struct HidePlayer : ISignal
    {
        // N/A
    }

    public readonly struct ShowPlayer : ISignal
    {
        // N/A
    }
    #endregion

    #region STATS
    public readonly struct StrengthChange : ISignal
    {
        public readonly uint Level;
        public readonly float Delta;

        public StrengthChange(uint level, float delta)
        {
            Level = level;
            Delta = delta;
        }
    }

    public readonly struct DexterityChange : ISignal
    {
        public readonly uint Level;
        public readonly float Delta;

        public DexterityChange(uint level, float delta)
        {
            Level = level;
            Delta = delta;
        }
    }

    public readonly struct EnduranceChange : ISignal
    {
        public readonly uint Level;
        public readonly float Delta;

        public EnduranceChange(uint level, float delta)
        {
            Level = level;
            Delta = delta;
        }
    }
    #endregion

    #region LEVEL
    public readonly struct LevelStart : ISignal
    {
        public readonly int Number;

        public LevelStart(int number)
        {
            Number = number;
        }
    }

    public readonly struct LevelEnd : ISignal
    {
        public readonly int Number;

        public LevelEnd(int number)
        {
            Number = number;
        }
    }
    #endregion

    #region CINEMA
    public readonly struct CinemaStart : ISignal
    {
        public readonly string ID;
        public readonly UnityAction Callback;

        public CinemaStart(string iD, UnityAction callback)
        {
            ID = iD;
            Callback = callback;
        }
    }

    public readonly struct CinemaFinish : ISignal
    {
        // N/A
    }

    public readonly struct CinemaActorMove : ISignal
    {
        public readonly ActorComponent Actor;
        public readonly Transform TargetPoint;

        public CinemaActorMove(ActorComponent actor, Transform targetPoint)
        {
            Actor = actor;
            TargetPoint = targetPoint;
        }
    }

    public readonly struct CinemaActorEmotion : ISignal
    {
        public readonly ActorComponent Actor;
        public readonly string EmotionName;

        public CinemaActorEmotion(ActorComponent actor, string emotionName)
        {
            Actor = actor;
            EmotionName = emotionName;
        }
    }
    #endregion
}
