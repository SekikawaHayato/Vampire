using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire.Scenario
{
    public class ScenarioLoader : MonoBehaviour
    {
        [SerializeField]
        TextAsset[] scenarioTextAssets;
        [SerializeField]
        TextAsset characterNameTextAssets;

        int scenarioSelector;

        ScenarioManager indicator;

        // Start is called before the first frame update
        void Awake()
        {
            List<string[]> scenarios = CSVReader.LoadScenario(scenarioTextAssets[scenarioSelector]);
            List<string[]> characterNameSource = CSVReader.LoadScenario(characterNameTextAssets);
            Dictionary<string, string> characterName = new Dictionary<string,string>();

            foreach(string[] character in characterNameSource)
            {
                characterName.Add(character[0], character[1]);
            }

            if (TryGetComponent<ScenarioManager>(out indicator))
            {
                indicator.Scenarios = scenarios;
                indicator.CharacterName = characterName;
            }
        }
    }
}