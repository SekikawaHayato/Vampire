using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using ATest;

namespace Vampire.Scenario
{
    public class ScenarioManager: MonoBehaviour
    {
        A test;

        #region UniRx
        public IReadOnlyReactiveProperty<string> MessageText => _messageText;
        public IObservable<string> NameText => _nameText;
        public IObservable<string> RinaFace => _rinaFace;
        public IObservable<string> AdolfFace => _adolfFace;
        public IObservable<bool> RinaActive => _rinaActive;
        public IObservable<bool> AdolfActive => _adolfActive;
        
        // イベント発行に利用するReactivePropertyなど
        readonly ReactiveProperty<string> _messageText = new ReactiveProperty<string>();
        readonly Subject<string> _nameText = new Subject<string>();
        readonly Subject<string> _rinaFace = new Subject<string>();
        readonly Subject<string> _adolfFace = new Subject<string>();
        readonly Subject<bool> _rinaActive = new Subject<bool>();
        readonly Subject<bool> _adolfActive = new Subject<bool>();
        #endregion

        // シナリオデータ
        List<string[]> _scenarios;
        // 表示数キャラクターの名前
        Dictionary<string, string> _characterName;

        public List<string> logName;
        public List<string> logMessage;
        

        // シナリオの進行を制御する変数
        [SerializeField] [Range(0.001f, 0.3f)] float interval = 0.05f;
        int _currentStep = 0;
        int _currentLine = 0;
        int _lineCount = 0;
        string _previousLine = string.Empty;
        string _currentText = string.Empty;
        int _currentLineTextCount = 0;
        float _timeUntilDisplay = 0;
        float _timeElapsed = 1;
        float _lastUpdateCharacter = -1;
        string[] _lineText;
        bool _isDisplayComplete = true;
        bool _isTransitionCpmplete = true;

        // 定数
        const int typeIndex = 0;
        const int optionIndex = 1;
        const int messageIndex = 2;
        const int rinaFaceIndex = 3;
        const int adolfFaceIndex = 4;
        const int rinaActiveIndex = 5;
        const int adolfActiveIndex = 6;

        // InputEvent
        IScenarioInputEventProvider _inputEventProvider;

        BackgroundChanger _backgroundChanger;

        // プロパティ
        public List<string[]> Scenarios
        {
            get { return _scenarios; }
            set { _scenarios = value; }
        }

        public Dictionary<string,string> CharacterName
        {
            get { return _characterName; }
            set { _characterName = value; }
        }

        void InitTest()
        {
            test = new A(5,1);

        }

        private void Start()
        {
            InitTest();
            print(test.GetA());
            // コンポーネントの取得
            TryGetComponent<BackgroundChanger>(out _backgroundChanger);
            if (TryGetComponent<IScenarioInputEventProvider>(out _inputEventProvider))
            {
                // クリックイベントの追加
                _inputEventProvider.IsClick.Subscribe(_ => ClickEvent(_));
            }

            // テキストの更新処理を行う
            this.UpdateAsObservable()
                .Where(_ => _isTransitionCpmplete)
                .Where(_ => !_isDisplayComplete)
                .Subscribe(_ => {
                    UpdateText();
                });

            logName = new List<string>();
            logMessage = new List<string>();

            SetNextStep();
        }

        // テキストの更新
        void UpdateText()
        {
            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - _timeElapsed) / _timeUntilDisplay) * _currentLineTextCount);
            if (displayCharacterCount != _lastUpdateCharacter)
            {
                _messageText.Value = _previousLine + _lineText[_currentLine].Substring(0, displayCharacterCount);
                _lastUpdateCharacter = displayCharacterCount;
                if(_lastUpdateCharacter == _currentLineTextCount) CheckDisplayComplete();
            }
        }

        // テキストの表示が完了しているか
        void CheckDisplayComplete()
        {
            if (_currentLine != _lineCount)
            {
                _previousLine += _lineText[_currentLine] + "\n";
                SetNextLine();
            }
            else
            {
                logMessage.Add(_messageText.Value);
                _isDisplayComplete = true;
            }
        }

        // クリックイベント
        void ClickEvent(bool check)
        {
            if (!_isTransitionCpmplete) return;
            if (!_isDisplayComplete)
            {
                // 文字を最後まで表示する
                while (_currentLine < _lineCount)
                {
                    _previousLine += _lineText[_currentLine] + "\n";
                    _currentLine++;
                }
                _messageText.Value = _previousLine + _lineText[_currentLine];
                logMessage.Add(_messageText.Value);
                _isDisplayComplete = true;
            }
            else if (_currentStep < _scenarios.Count)
            {
                SetNextStep();
            }
        }

        // 次の行を表示する
        void SetNextStep()
        {
            switch (_scenarios[_currentStep][typeIndex])
            {
                // 背景の変更
                case "Background":
                    _isTransitionCpmplete = false;
                    _backgroundChanger.ChangeBackground(_scenarios[_currentStep][optionIndex], BackgroundCallBack);
                    _messageText.Value = null;
                    _nameText.OnNext(null);
                    break;
                // テキストを表示
                case "Words":
                    // 表示するテキストを読み込む
                    _currentText = _scenarios[_currentStep][messageIndex];
                    // 名前を表示する
                    _nameText.OnNext(_characterName[_scenarios[_currentStep][optionIndex]]);
                    logName.Add(_characterName[_scenarios[_currentStep][optionIndex]]);
                    if (_scenarios[_currentStep][rinaFaceIndex] != "") _rinaFace.OnNext(_scenarios[_currentStep][rinaFaceIndex]);
                    if (_scenarios[_currentStep][adolfFaceIndex] != "") _adolfFace.OnNext(_scenarios[_currentStep][adolfFaceIndex]);
                    switch (_scenarios[_currentStep][rinaActiveIndex])
                    {
                        case "Active":
                            _rinaActive.OnNext(true);
                            break;
                        case "Inactive":
                            _rinaActive.OnNext(false);
                            break;
                    }
                    switch (_scenarios[_currentStep][adolfActiveIndex])
                    {
                        case "Active":
                            _adolfActive.OnNext(true);
                            break;
                        case "Inactive":
                            _adolfActive.OnNext(false);
                            break;
                    }
                    _previousLine = string.Empty;
                    _lineText = _currentText.Split('\\');
                    _lineCount = _lineText.Length - 1;
                    _currentLine = -1;
                    SetNextLine();
                    _isDisplayComplete = false;
                    break;
                // 画面遷移
                case "Scene":
                    break;
            }
            _currentStep++;
        }

        // 背景変更のコールバック
        void BackgroundCallBack()
        {
            _isTransitionCpmplete = true;
            SetNextStep();
        }

        // 次の行に移る
        void SetNextLine()
        {
            _currentLine++;
            _currentLineTextCount = _lineText[_currentLine].Length;
            _timeUntilDisplay = _currentLineTextCount * interval;
            _timeElapsed = Time.time;

            _lastUpdateCharacter = -1;
        }
    }
}