using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Vampire.Scenario
{
    public class LogGenerator : MonoBehaviour
    {
        [SerializeField] Button _logButton;
        [SerializeField] Button _backButton;
        [SerializeField] GameObject _logPrefab;
        [SerializeField] Transform _parent;
        [SerializeField] ScenarioManager _scenarioManager;
        [SerializeField] GameObject _logPanel;

        private void Awake()
        {
            _logButton.onClick.AsObservable()
                .Subscribe(t => LogButton())
                .AddTo(gameObject);

            _backButton.onClick.AsObservable()
                .Subscribe(t => BackButton())
                .AddTo(gameObject);
        }

        void LogButton()
        {
            foreach (Transform child in _parent)
            {
                Destroy(child.gameObject);
            }

            List<string> name = _scenarioManager.logName;
            List<string> message = _scenarioManager.logMessage;

            for(int i = 0; i < message.Count; i++)
            {
                GameObject obj = Instantiate(_logPrefab, _parent);
                obj.GetComponent<LogText>().SetText(name[i], message[i]);
            }

            _logPanel.SetActive(true);
        }

        void BackButton()
        {
            _logPanel.SetActive(false);
        }
    }
}
