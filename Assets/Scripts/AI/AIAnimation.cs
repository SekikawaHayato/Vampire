using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire.AI {
    public class AIAnimation : MonoBehaviour
    {
        Animator _animator;
        IAIProvider _aiProvider;

        // Start is called before the first frame update
        void Start()
        {
            TryGetComponent<Animator>(out _animator);
            TryGetComponent<IAIProvider>(out _aiProvider);
        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat("x",_aiProvider.Direction.x);
            _animator.speed = (_aiProvider.State ==AIState.Move) ? 1 : 0;
        }
    }
}
