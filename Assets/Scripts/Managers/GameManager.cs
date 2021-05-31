using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Veganimus.BattleSystem
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private BattleState _battleState;
        [SerializeField] private List<Character> characters;
        [SerializeField] private GameObject _currentPlayerUnit;
        [SerializeField] private GameObject _currentEnemyUnit;
        [SerializeField] private bool _hasPlayerCompletedTurn;
        [SerializeField] private bool _hasEnemyCompletedTurn;
        [SerializeField] private int _playerSpeed;
        [SerializeField] private int _enemySpeed;
        [SerializeField] private int _consecutiveTurns;
        private bool _isBattleOver;
        private DieRoll _dieRoll = new DieRoll();
        private WaitForSeconds _changeStateDelay;
        [Header("Broadcasting on:")]
        [SerializeField] private CharacterTurnChannel _characterTurnChannel;
        [Header("Listening to")]
        [SerializeField] private TurnCompleteChannel _characterTurnCompleteChannel;
        [SerializeField] private BattleStateChannel _endBattleChannel;

        private void OnEnable()
        {
            _characterTurnCompleteChannel.OnTurnComplete.AddListener(CharacterCompleteTurn);
            _endBattleChannel.OnBattleStateChanged.AddListener(EndBattle);
        }

        private void OnDisable()
        {
            _characterTurnCompleteChannel.OnTurnComplete.RemoveListener(CharacterCompleteTurn);
            _endBattleChannel.OnBattleStateChanged.RemoveListener(EndBattle);
        }

        private void Start()
        {
            _currentPlayerUnit = GameObject.FindGameObjectWithTag("Player");
            _currentEnemyUnit = GameObject.FindGameObjectWithTag("Enemy");
            _changeStateDelay = new WaitForSeconds(3f);
            _battleState = BattleState.Start;
            RollForTurn();
            
        }
       private void RollForTurn()
       {
            if (_playerSpeed > _enemySpeed)
            {
                var result = _dieRoll.Roll(_playerSpeed - _enemySpeed);
               // Debug.Log($"Player Roll:{_dieRoll.GetResult()}");
                if (result == true && _consecutiveTurns < 1 )
                {
                     _consecutiveTurns++;
                    StartCoroutine(ChangeState(BattleState.PlayerTurn));
                }
                else
                {
                    _consecutiveTurns = 0;
                    StartCoroutine(ChangeState(BattleState.EnemyTurn));
                }
            }
            else if (_enemySpeed > _playerSpeed)
            {
                var result = _dieRoll.Roll(_enemySpeed - _playerSpeed);
                //Debug.Log($"Enemy Roll:{_dieRoll.GetResult()}");
                if (result == true && _consecutiveTurns < 1 )
                {
                    _consecutiveTurns++;
                    StartCoroutine(ChangeState(BattleState.EnemyTurn));
                }
                else
                {
                    _consecutiveTurns = 0;
                    StartCoroutine(ChangeState(BattleState.PlayerTurn));
                }
                   
            }
            else if (_playerSpeed == _enemySpeed)
            {
                var result = _dieRoll.Roll();
               // Debug.Log($"Roll:{_dieRoll.GetResult()}");
                if (result == true)
                    StartCoroutine(ChangeState(BattleState.PlayerTurn));
                else
                    StartCoroutine(ChangeState(BattleState.EnemyTurn));
            }
        }
       private void CharacterCompleteTurn(CharacterType characterType, bool turnComplete)
        {
            switch(characterType)
            {
                case CharacterType.Player:
                    _hasPlayerCompletedTurn = turnComplete;
                    if (_hasPlayerCompletedTurn && _isBattleOver == false)
                    {
                        if (_playerSpeed > _enemySpeed && _consecutiveTurns < 1)
                         RollForTurn();
                        
                        else
                        {
                            _consecutiveTurns = 0;
                            StartCoroutine(ChangeState(BattleState.EnemyTurn));
                        }
                    }
                    break;
                case CharacterType.Enemy:
                    _hasEnemyCompletedTurn = turnComplete;
                    if (_hasEnemyCompletedTurn && _isBattleOver == false)
                    {
                        if (_playerSpeed < _enemySpeed && _consecutiveTurns < 1)
                         RollForTurn();

                        else
                        {
                            _consecutiveTurns = 0;
                            StartCoroutine(ChangeState(BattleState.PlayerTurn));
                        }
                    }
                    break;
            }
        }
      
        private void EndBattle(BattleState battleState)
        {
            _isBattleOver = true;
            StartCoroutine(ChangeState(battleState));
        }

        private IEnumerator ChangeState(BattleState newState)
        {
            yield return _changeStateDelay;
            switch(newState)
            {
                case BattleState.Start:
                    _battleState = BattleState.Start;
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.Start);
                    break;
                case BattleState.PlayerTurn:
                    _battleState = BattleState.PlayerTurn;
                    BattleUIManager.Instance.ActivateButtons(true);
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.PlayerTurn);
                    _characterTurnChannel.RaiseCharacterTurnEvent(CharacterType.Player);
                    break;
                case BattleState.EnemyTurn:
                    _battleState = BattleState.EnemyTurn;
                    BattleUIManager.Instance.ActivateButtons(false);
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.EnemyTurn);
                    _characterTurnChannel.RaiseCharacterTurnEvent(CharacterType.Enemy);
                    break;
                case BattleState.Win:
                    _battleState = BattleState.Win;
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.Win);
                    break;
                case BattleState.Lose:
                    _battleState = BattleState.Lose;
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.Lose);
                    break;
            }
            _playerSpeed = _currentPlayerUnit.GetComponentInChildren<Unit>().RunTimeUnitInfo.speed;
            _enemySpeed = _currentEnemyUnit.GetComponentInChildren<Unit>().RunTimeUnitInfo.speed;
        }
        public void LoadMainMenu()
        {
            Time.timeScale = 1;
            var load = SceneManager.LoadSceneAsync(0);
            if (load.isDone)
                SceneManager.UnloadSceneAsync(1);
        }
        public void PauseGame() => Time.timeScale = 0;

        public void UnPauseGame() => Time.timeScale = 1;

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
#if PLATFORM_WEBGL
            Application.OpenURL("https://veganimus.itch.io/turn-based-battle-system-demo");
#endif
        }
    }
}
