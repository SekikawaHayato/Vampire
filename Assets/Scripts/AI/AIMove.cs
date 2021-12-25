using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Vampire.AI
{
    public class AIMove : MonoBehaviour
    {
        [SerializeField] float _moveSpeed;

        IAIProvider _aIProvider;

        
        // Start is called before the first frame update
        void Start()
        {
            TryGetComponent<IAIProvider>(out _aIProvider);
        }

        // Update is called once per frame
        void Update()
        {
            switch (_aIProvider.State)
            {
                case AIState.Move:
                    transform.Translate(_aIProvider.Direction.normalized * _moveSpeed * Time.deltaTime);
                    break;
            }
        }
    }
}
