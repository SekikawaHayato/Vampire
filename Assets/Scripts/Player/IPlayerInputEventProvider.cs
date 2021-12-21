using System;
using UniRx;
using UnityEngine;

namespace Vampire.Players
{
    public interface IPlayerInputEventProvider
    {
        IReadOnlyReactiveProperty<Vector3> MoveDirection{ get;}
        IObservable<Unit> OnLightAttach {get;}
        IReadOnlyReactiveProperty<bool> IsJump{get;}
    }
}
