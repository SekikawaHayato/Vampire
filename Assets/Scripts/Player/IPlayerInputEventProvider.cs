using System;
using UnityEngine;
using UniRx;

namespace Vampire.Players
{
    public interface IPlayerInputEventProvider
    {
        IReadOnlyReactiveProperty<Vector3> MoveDirection{ get;}
        IObservable<Unit> OnLightAttach {get;}
        IReadOnlyReactiveProperty<bool> IsJump{get;}
    }
}
