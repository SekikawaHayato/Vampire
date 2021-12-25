using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Vampire.Scenario
{
    public class ScenarioIndicator : MonoBehaviour
    {
        // テキストを表示するUI
        [SerializeField] Text _messageWindowText;
        [SerializeField] Text _nameText;

        ScenarioManager _scenarioManager;
        /// <summary>
        ///
        /// 
        /// </summary>
        private void Awake()
        {
            // コンポーネントの取得
            if(TryGetComponent<ScenarioManager>(out _scenarioManager))
            {
                // イベントの追加
                _scenarioManager.MessageText.Subscribe(t =>
                {
                    SetText(t);
                });
                _scenarioManager.NameText.Subscribe(t =>
                {
                    SetName(t);
                });
            }
        }

        // テキストをUIに表示
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void SetText(string text)
        {
            _messageWindowText.text = text;
        }

        // 名前をUIに表示
        void SetName(string name)
        {
            _nameText.text = name;
        }

    }
}
