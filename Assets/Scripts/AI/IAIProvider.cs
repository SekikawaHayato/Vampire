using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Vampire.AI
{
    public interface IAIProvider
    {
        AIState State { get; }
        Transform Target { get;}
        Vector3 Direction { get; }
        //void ChangeTarget();
    }
}