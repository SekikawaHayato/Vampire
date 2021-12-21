using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using UniRx.Triggers;
using UnityEngine;

namespace Vampire.Scenario
{
    public class ScenarioManager: MonoBehaviour
    {

        // TODO: キャラ名の表示
        // TODO: キャラの位置を変更
        // TODO: キャラの画像を表示
        List<string[]> scenarios;
        Dictionary<string,string> characterName;

        string[] lineText;

        #region
        public IReadOnlyReactiveProperty<string> MessageText => _messageText;
        public IObservable<string> NameText => _nameText;
        public IObservable<string> RinaFace => _rinaFace;
        public IObservable<string> AdolfFace => _adolfFace;
        public IObservable<bool> RinaActive => _rinaActive;
        public IObservable<bool> AdolfActive => _adolfActive;
        #endregion

        // イベント発行に利用するReactiveProperty
        readonly ReactiveProperty<string> _messageText = new ReactiveProperty<string>();
        readonly Subject<string> _nameText = new Subject<string>();
        readonly Subject<string> _rinaFace = new Subject<string>();
        readonly Subject<string> _adolfFace = new Subject<string>();
        readonly Subject<bool> _rinaActive = new Subject<bool>();
        readonly Subject<bool> _adolfActive = new Subject<bool>();

        
        [SerializeField]
        [Range(0.001f, 0.3f)]
        float interval = 0.05f;

        int currentStep = 0;
        int currentLine = 0;
        int lineCount = 0;
        string previousLine = string.Empty;
        string currentText = string.Empty;
        int currentLineTextCount = 0;
        float timeUntilDisplay = 0;
        float timeElapsed = 1;
        float lastUpdateCharacter = -1;

        // 定数
        const int typeIndex = 0;
        const int optionIndex = 1;
        const int messageIndex = 2;
        const int rinaFaceIndex = 3;
        const int adolfFaceIndex = 4;
        const int rinaActiveIndex = 5;
        const int adolfActiveIndex = 6;


        bool isDisplayComplete = true;
        bool isTransitionCpmplete = true;

        // InputEvent
        IScenarioInputEventProvider _inputEventProvider;

        BackgroundChanger _backgroundChanger;

        public List<string[]> Scenarios
        {
            get { return scenarios; }
            set { scenarios = value; }
        }

        public Dictionary<string,string> CharacterName
        {
            get { return characterName; }
            set { characterName = value; }
        }

        private void Start()
        {
            TryGetComponent<BackgroundChanger>(out _backgroundChanger);
            if (TryGetComponent<IScenarioInputEventProvider>(out _inputEventProvider))
            {
                _inputEventProvider.IsClick.Subscribe(_ => ClickEvent(_));
            }

            // テキストの更新処理を行う
            this.UpdateAsObservable()
                .Where(_ => isTransitionCpmplete)
                .Where(_ => !isDisplayComplete)
                .Subscribe(_ => {
                    UpdateText();
                });

            SetNextStep();
        }

        // テキストの更新
        void UpdateText()
        {
            // セル内改行に対応する
            /* １行表示完了したら一時変数に追加
             * 一時変数をSetTextの最初に追加
             * 行を表示し終わったら次の行があるかチェック
             * テキスト更新処理で一時変数を初期化
             */
            
            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentLineTextCount);
            if (displayCharacterCount != lastUpdateCharacter)
            {
                _messageText.Value = previousLine + lineText[currentLine].Substring(0, displayCharacterCount);
                lastUpdateCharacter = displayCharacterCount;
                if(lastUpdateCharacter == currentLineTextCount) CheckDisplayComplete();
            }
        }

        void CheckDisplayComplete()
        {
            if (currentLine != lineCount)
            {
                previousLine += lineText[currentLine] + "\n";
                SetNextLine();
            }
            else
            {
                isDisplayComplete = true;
            }
        }

        // クリックイベント
        void ClickEvent(bool check)
        {
            if (!isTransitionCpmplete) return;
            if (!isDisplayComplete)
            {
                // 文字を最後まで表示する
                while (currentLine < lineCount)
                {
                    previousLine += lineText[currentLine] + "\n";
                    currentLine++;
                }
                _messageText.Value = previousLine + lineText[currentLine];
                isDisplayComplete = true;
            }
            else if (currentStep < scenarios.Count)
            {
                SetNextStep();
            }
        }



        // 次の行を表示する
        void SetNextStep()
        {
            switch (scenarios[currentStep][typeIndex])
            {
                case "Background":
                    isTransitionCpmplete = false;
                    _backgroundChanger.ChangeBackground(scenarios[currentStep][optionIndex], BackgroundCallBack);
                    break;
                case "Words":
                    // 表示するテキストを読み込む
                    currentText = scenarios[currentStep][messageIndex];
                    // 名前を表示する
                    _nameText.OnNext(characterName[scenarios[currentStep][optionIndex]]);
                    if (scenarios[currentStep][rinaFaceIndex] != "") _rinaFace.OnNext(scenarios[currentStep][rinaFaceIndex]);
                    if (scenarios[currentStep][adolfFaceIndex] != "") _adolfFace.OnNext(scenarios[currentStep][adolfFaceIndex]);
                    switch (scenarios[currentStep][rinaActiveIndex])
                    {
                        case "Active":
                            _rinaActive.OnNext(true);
                            break;
                        case "Inactive":
                            _rinaActive.OnNext(true);
                            break;
                    }
                    switch (scenarios[currentStep][adolfActiveIndex])
                    {
                        case "Active":
                            _adolfActive.OnNext(true);
                            break;
                        case "Inactive":
                            _adolfActive.OnNext(true);
                            break;
                    }
                    previousLine = string.Empty;
                    lineText = currentText.Split('\\');
                    lineCount = lineText.Length - 1;
                    currentLine = -1;
                    SetNextLine();
                    isDisplayComplete = false;
                    break;
                case "Scene":
                    break;
            }
            currentStep++;
        }

        void BackgroundCallBack()
        {
            isTransitionCpmplete = true;
            SetNextStep();
        }

        void SetNextLine()
        {
            currentLine++;
            currentLineTextCount = lineText[currentLine].Length;
            timeUntilDisplay = currentLineTextCount * interval;
            timeElapsed = Time.time;

            lastUpdateCharacter = -1;
        }
    }
}