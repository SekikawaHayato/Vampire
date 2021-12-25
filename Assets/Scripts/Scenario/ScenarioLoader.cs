using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire.Scenario
{
    public class ScenarioLoader : MonoBehaviour
    {
        // シナリオデータ
        [SerializeField] TextAsset[] _scenarioTextAssets;
        [SerializeField] TextAsset _characterNameTextAssets;

        // シナリオを選択する変数
        int _scenarioSelector;
        ScenarioManager _indicator;

        // Start is called before the first frame update
        void Awake()
        {
            // データの読み込み
            List<string[]> scenarios = CSVReader.LoadScenario(_scenarioTextAssets[_scenarioSelector]);
            List<string[]> characterNameSource = CSVReader.LoadScenario(_characterNameTextAssets);
            Dictionary<string, string> characterName = new Dictionary<string,string>();

            // データを変数に格納
            foreach(string[] character in characterNameSource)
            {
                characterName.Add(character[0], character[1]);
            }

            // ScenarioManagerに変数を渡す
            if (TryGetComponent<ScenarioManager>(out _indicator))
            {
                _indicator.Scenarios = scenarios;
                _indicator.CharacterName = characterName;
            }
        }
    }
}