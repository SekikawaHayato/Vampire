using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


namespace Vampire.Scenario
{
    public class BackgroundChanger : MonoBehaviour
    {
        [SerializeField]
        float _fadeTime;
        [SerializeField]
        float _interval;
        [SerializeField]
        Image _fadePanel;
        Color _fadeColor;
        [SerializeField]
        Image _background;
        Sprite useSprite;
        [SerializeField]
        AssetBundleLoader assetBundleLoader;

        bool isComplete = true;

        // Start is called before the first frame update
        void Awake()
        {
            assetBundleLoader.LoadAssets();
            _fadeColor = _fadePanel.color;
        }

        public void ChangeBackground(string imageName, Action action = null)
        {
            if (!isComplete) return;
            if (imageName != "black") useSprite = assetBundleLoader.sprites[imageName];
            StartCoroutine(Change(action));
        }

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
            _background.sprite = useSprite;
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