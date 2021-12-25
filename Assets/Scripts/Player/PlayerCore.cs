using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace Vampire.Players{
    public class PlayerCore : MonoBehaviour
    {
        public IObservable<Vector3> MoveObservable => _moveSubject;
        public IReadOnlyReactiveProperty<bool> IsDead => _isDead;

        readonly ReactiveProperty<bool> _isDead = new ReactiveProperty<bool>();
        readonly Subject<Vector3> _moveSubject= new Subject<Vector3>();

        private bool _isInvincible;
        IPlayerInputEventProvider _inputEventProvider;
        [SerializeField] float _moveLimit = 0.8f;

        // Start is called before the first frame update
        void Start()
        {
            _isDead.AddTo(this);
            _inputEventProvider = GetComponent<IPlayerInputEventProvider>();

            this.OnCollisionEnter2DAsObservable()
                .Where(_ => !_isInvincible)
                .Where(x => x.gameObject.TryGetComponent<PlayerMove>(out _))
                .Subscribe(onNext: _ => _isDead.Value = true);
            this.UpdateAsObservable().Subscribe(_ =>
            {
                _moveSubject.OnNext(GetMoveVector());
            });
        }

        public void SetInvinvible(bool isInvincible){
            _isInvincible = isInvincible;
        }

        Vector3 GetMoveVector(){
            return _inputEventProvider.MoveDirection.Value.magnitude<_moveLimit? Vector3.zero :_inputEventProvider.MoveDirection.Value;
            //float y = _inputEventProvider.MoveDirection.Value.z;
            //return 0? 
            //if (x > _moveLimit)
            //{
            //    return Vector3.right;
            //}
            //else if (x < -_moveLimit)
            //{
            //    return Vector3.left;
            //}
            //else if (y > _moveLimit)
            //{
            //    return Vector3.up;
            //}
            //else if (y < -_moveLimit)
            //{
            //    return Vector3.down;
            //}
            //else
            //{
            //    return Vector3.zero;
            //}
        }
    }
}
