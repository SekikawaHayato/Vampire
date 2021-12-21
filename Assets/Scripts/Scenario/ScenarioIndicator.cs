using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace Vampire.Scenario
{
    public class ScenarioIndicator : MonoBehaviour
    {

        [SerializeField]
        Text messageWindowText;
        [SerializeField]
        Text nameText;

        ScenarioManager scenarioManager;

        private void Awake()
        {
            if(TryGetComponent<ScenarioManager>(out scenarioManager))
            {
                scenarioManager.MessageText.Subscribe(t =>
                {
                    SetText(t);
                });
                scenarioManager.NameText.Subscribe(t =>
                {
                    SetName(t);
                });
            }
        }

        // テキストをUIに表示
        void SetText(string text)
        {
            messageWindowText.text = text;
        }

        void SetName(string name)
        {
            nameText.text = name;
        }

    }
}
