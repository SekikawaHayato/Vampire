using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Vampire.AI
{
    public enum AIState
    {
        Stay,
        Endure,
        Damage,
        Move,

    }

    public class AICore : MonoBehaviour,IAIProvider
    {
        float _damageTimer;
        //#region UniRx
        //public IReadOnlyReactiveProperty<AIState> State => _state;

        //// イベントの発行に利用するSubject
        //readonly ReactiveProperty<AIState> _state = new ReactiveProperty<AIState>();
        //#endregion

        AIState _state = AIState.Stay;
        GameObject _target;
        [SerializeField] List<GameObject> _allTargets;
        public AIState State
        {
            get { return _state; }
        }

        public Transform Target
        {
            get { return _target.transform; }
        }

        public Vector3 Direction
        {
            get { return _target.transform.position - this.gameObject.transform.position; }
        }

        // Start is called before the first frame update
        void Start()
        {
            ChangeTarget();
            _state = AIState.Move;
            this.OnTriggerEnter2DAsObservable()
                //.Where(_ => _.gameObject==_target)
                .Where(_ => _state == AIState.Move)
                .Subscribe(_ =>
                {
                    if (_.gameObject == _target)
                    {
                        Arrival();
                    }
                    else if(_.gameObject.tag == "Wind")
                    {
                        Wind();
                    }
                    else if(_.gameObject.tag == "Garlic")
                    {

                    }
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    switch (_state)
                    {
                        case AIState.Stay:

                            break;
                        case AIState.Endure:
                            _damageTimer -= Time.deltaTime;
                            if(_damageTimer <= 0)
                            {
                                _state = AIState.Damage;
                                _damageTimer = 3;
                            }
                            break;
                        case AIState.Damage:
                            _damageTimer -= Time.deltaTime;
                            if (_damageTimer <= 0)
                            {
                                // ゲームオーバー処理
                                SceneLoader.Instance.NextScene("Title");
                            }
                            break;
                        case AIState.Move:

                            break;
                        

                    }
                });

        }

        void Arrival()
        {
            _state = AIState.Stay;
            Invoke("ChangeTarget", 1.5f);
        }

        void Wind()
        {
            _damageTimer = 3;
            _state = AIState.Endure;
        }

        void Garlic()
        {

        }

        void ChangeTarget()
        {
            if (_allTargets.Count == 0)
            {
                
            }
            else
            {
                int index = Random.Range(0, _allTargets.Count);
                _target = _allTargets[index];
                _allTargets.RemoveAt(index);
                _state = AIState.Move;
            }
        }
    }
}
