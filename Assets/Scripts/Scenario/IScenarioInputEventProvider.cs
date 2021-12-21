using System;
using UniRx;
using UnityEngine;

namespace Vampire.Scenario
{
    public interface IScenarioInputEventProvider
    {
        IObservable<bool> IsClick { get; }
    }
}
