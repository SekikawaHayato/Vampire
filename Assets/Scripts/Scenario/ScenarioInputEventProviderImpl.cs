using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using System;

namespace Vampire.Scenario
{
    public class ScenarioInputEventProviderImpl : MonoBehaviour,IScenarioInputEventProvider
    {
        #region
        public IObservable<bool> IsClick => _isClickSubject;
        #endregion

        readonly Subject<bool> _isClickSubject=new Subject<bool>();
        // Start is called before the first frame update
        void Start()
        {
            _isClickSubject.AddTo(this);
            this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Where(_ => !EventSystem.current.IsPointerOverGameObject())
                .Subscribe(_ =>
                {
                    _isClickSubject.OnNext(true);
                });
        }
    }
}