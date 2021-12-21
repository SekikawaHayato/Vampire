using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Vampire.Scenario
{
    public class CharacterIndicator : MonoBehaviour
    {
        [SerializeField]
        Image _rinaImage;
        [SerializeField]
        Image _AdolfImage;
        [SerializeField]
        AssetBundleLoader rinaAssetBundleLoader;
        [SerializeField]
        AssetBundleLoader adolfAssetBundleLoader;

        ScenarioManager scenarioManager;


        // Start is called before the first frame update
        void Awake()
        {
            rinaAssetBundleLoader.LoadAssets();
            adolfAssetBundleLoader.LoadAssets();

            if (TryGetComponent<ScenarioManager>(out scenarioManager))
            {
                scenarioManager.RinaFace.Subscribe(t =>
                {
                    RinaChanger(t);
                });

                scenarioManager.AdolfFace.Subscribe(t =>
                {
                    AdolfChanger(t);
                });

                scenarioManager.RinaActive.Subscribe(t =>
                {
                    RinaActive(t);
                });

                scenarioManager.AdolfActive.Subscribe(t =>
                {
                    AdolfActive(t);
                });
            }
        }

        void RinaChanger(string face)
        {
            _rinaImage.sprite = rinaAssetBundleLoader.sprites["Rina_"+face];
        }

        void AdolfChanger(string face)
        {
            _AdolfImage.sprite = adolfAssetBundleLoader.sprites["Adolf_"+face];
        }

        void RinaActive(bool flag)
        {
            _rinaImage.gameObject.SetActive(flag);
        }

        void AdolfActive(bool flag)
        {
            _AdolfImage.gameObject.SetActive(flag);
        }
    }
}
