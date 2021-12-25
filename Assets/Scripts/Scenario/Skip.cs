using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Vampire.Scenario
{
    public class Skip : MonoBehaviour
    {
        [SerializeField] Button _skipPanelButton;
        [SerializeField] Button _skipButton;
        [SerializeField] Button _backButton;
        [SerializeField] GameObject _skipPanel;

        // Start is called before the first frame update
        void Start()
        {
            _skipPanelButton.onClick.AsObservable()
                .Subscribe(t => SkipPanelButton())
                .AddTo(gameObject);
            _skipButton.onClick.AsObservable()
                .Subscribe(t => SkipButton())
                .AddTo(gameObject);
            _backButton.onClick.AsObservable()
                .Subscribe(t => BackButton())
                .AddTo(gameObject);
        }

        void SkipPanelButton()
        {
            _skipPanel.SetActive(true);
        }

        void SkipButton()
        {
            SceneLoader.Instance.NextScene("Main");
        }
        void BackButton()
        {
            _skipPanel.SetActive(false);
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}

