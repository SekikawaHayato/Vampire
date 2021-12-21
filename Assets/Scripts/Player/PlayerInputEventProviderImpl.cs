using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace Vampire.Players.InputImpls
{
    public sealed class PlayerInputEventProviderImpl : MonoBehaviour,IPlayerInputEventProvider
    {
        // 長押しだと判定するまでの時間
        private static readonly float LongPressSeconds = 0.25f;
        [SerializeField] Joystick joystick = null;

        #region IInputEventProvider
        public IObservable<Unit> OnLightAttach => _lightAttackSubject;
        public IReadOnlyReactiveProperty<bool> IsJump => _jump;
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _move;
        #endregion
        
        // イベント発行に利用するSubjectやReactiveProperty
        private readonly Subject<Unit> _lightAttackSubject = new Subject<Unit>();
        private readonly ReactiveProperty<bool> _jump = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<Vector3> _move = new ReactiveProperty<Vector3>();

        // Start is called before the first frame update
        void Start()
        {
            _lightAttackSubject.AddTo(this);
            _jump.AddTo(this);
            _move.AddTo(this);
            // this.UpdateAsObservable()
            // .Select(_ => Input.GetButton("Attack"))
            // .DistinctUntilChanged()
            // .TimeInterval()
            // .Skip(1)
            // .Subscribe(t =>
            // {

            // }).AddTo(this);
        }

        // Update is called once per frame
        void Update()
        {
            _move.SetValueAndForceNotify(new Vector3(joystick.Horizontal,0,joystick.Vertical));
            //Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")));
        }
    }
}