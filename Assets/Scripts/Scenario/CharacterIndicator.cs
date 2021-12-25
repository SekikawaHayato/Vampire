using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Vampire.Scenario
{
    public class CharacterIndicator : MonoBehaviour
    {
        [SerializeField] Image _rinaImage;
        [SerializeField] Image _AdolfImage;
        [SerializeField] AssetBundleLoader _rinaAssetBundleLoader;
        [SerializeField] AssetBundleLoader _adolfAssetBundleLoader;

        ScenarioManager _scenarioManager;


        // Start is called before the first frame update
        void Awake()
        {
            // 画像データを読み込む
            _rinaAssetBundleLoader.LoadAssets();
            _adolfAssetBundleLoader.LoadAssets();

            // コンポーネントを取得
            if (TryGetComponent<ScenarioManager>(out _scenarioManager))
            {
                // イベントの追加
                _scenarioManager.RinaFace.Subscribe(t =>
                {
                    RinaChanger(t);
                });

                _scenarioManager.AdolfFace.Subscribe(t =>
                {
                    AdolfChanger(t);
                });

                _scenarioManager.RinaActive.Subscribe(t =>
                {
                    RinaActive(t);
                });

                _scenarioManager.AdolfActive.Subscribe(t =>
                {
                    AdolfActive(t);
                });
            }
        }

        // 表情差分の表示
        void RinaChanger(string face)
        {
            _rinaImage.sprite = _rinaAssetBundleLoader.sprites["Rina_"+face];
        }

        void AdolfChanger(string face)
        {
            _AdolfImage.sprite = _adolfAssetBundleLoader.sprites["Adolf_"+face];
        }

        // 表示非表示の切り替え
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
