using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Vampire.Players{
    public sealed class PlayerMove : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<bool> IsGround => _isGrounded;
        
        [SerializeField] float _dashSpeed = 2;
        [SerializeField] float _jumpSpeed = 5.5f;
        [SerializeField] LayerMask _groundMask;

        readonly ReactiveProperty<bool> _isGrounded = new BoolReactiveProperty();
        PlayerCore _playerCore;
        Rigidbody2D _rigidbody2D;
        
        bool _isMoveBlock;

        // Start is called before the first frame update
        void Start()
        {
            _isGrounded.AddTo(this);
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerCore = GetComponent<PlayerCore>();

            _playerCore.MoveObservable.Subscribe(t => {
                Move(t);
            });
        }

        void Move(Vector3 moveVector){
            _rigidbody2D.velocity = moveVector * _dashSpeed;
        }
    }
}