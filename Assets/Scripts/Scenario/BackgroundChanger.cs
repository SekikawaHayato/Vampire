using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Vampire.Scenario
{
    public class BackgroundChanger : MonoBehaviour
    {
        [SerializeField] float _fadeTime;
        [SerializeField] float _interval;
        [SerializeField] Image _fadePanel;
        [SerializeField] Image _background;
        [SerializeField] AssetBundleLoader _assetBundleLoader;
        Color _fadeColor;
        Sprite _useSprite;

        bool isComplete = true;

        // Start is called before the first frame update
        void Awake()
        {
            // 画像データを読み込む
            _assetBundleLoader.LoadAssets();
            _fadeColor = _fadePanel.color;
        }

        // 呼び出し用メソッド
        public void ChangeBackground(string imageName, Action action = null)
        {
            if (!isComplete) return;
            if (imageName != "black") _useSprite = _assetBundleLoader.sprites[imageName];
            StartCoroutine(Change(action));
        }

        // 背景を変更する処理
        IEnumerator Change(Action onFinished = null)
        {
            isComplete = false;
            float fadeSpeed = 1f / _fadeTime;
            while (_fadeColor.a <= 1)
            {
                _fadeColor.a += fadeSpeed * Time.deltaTime;
                _fadePanel.color = _fadeColor;
                yield return null;
            }
            _background.sprite = _useSprite;
            yield return new WaitForSeconds(_interval);
            while (_fadeColor.a >= 0)
            {
                _fadeColor.a -= fadeSpeed * Time.deltaTime;
                _fadePanel.color = _fadeColor;
                yield return null;
            }
            yield return new WaitForSeconds(_interval);
            isComplete = true;
            if(onFinished != null)onFinished();
        }
    }
}