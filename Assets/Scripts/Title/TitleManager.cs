using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace Vampire.Title
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] Button _startButton;
        [SerializeField] float _duration;
        [SerializeField] Ease _easeType;
        Tweener _tweener;

        // Start is called before the first frame update
        void Start()
        {
            _startButton.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    SceneLoader.Instance.NextScene("Scenario");
                    _tweener.Kill();
                });
            _tweener = _startButton.image
                .DOFade(0.0f, _duration)
                .SetEase(_easeType)
                .SetLoops(-1, LoopType.Yoyo);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
