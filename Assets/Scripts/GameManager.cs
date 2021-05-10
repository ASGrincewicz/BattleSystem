using System.Collections;
using UnityEngine;

namespace Veganimus.BattleSystem
{
    public enum BattleState
    {
        Start, PlayerTurn, EnemyTurn, Win, Lose
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BattleState _battleState;
        [SerializeField] private GameObject _currentPlayerUnit;
        [SerializeField] private GameObject _currentEnemyUnit;
        [SerializeField] private bool _hasPlayerCompletedTurn;
        [SerializeField] private bool _hasEnemyCompletedTurn;
        private bool _isBattleOver;
        private WaitForSeconds _changeStateDelay;
        
        [Header("Broadcasting on:")]
        [SerializeField] private PlayerTurnChannel _playerTurnChannel;
        [SerializeField] private EnemyTurnChannel _enemyTurnChannel;
        [Header("Listening to")]
        [SerializeField] private TurnCompleteChannel _playerTurnCompleteChannel;
        [SerializeField] private TurnCompleteChannel _enemyTurnCompleteChannel;
        [SerializeField] private BattleStateChannel _endBattleChannel;

        private void OnEnable()
        {
            _playerTurnCompleteChannel.OnTurnComplete.AddListener(PlayerCompleteTurn);
            _enemyTurnCompleteChannel.OnTurnComplete.AddListener(EnemyCompleteTurn);
            _endBattleChannel.OnBattleStateChanged.AddListener(EndBattle);
        }

        private void OnDisable()
        {
            _playerTurnCompleteChannel.OnTurnComplete.RemoveListener(PlayerCompleteTurn);
            _enemyTurnCompleteChannel.OnTurnComplete.RemoveListener(EnemyCompleteTurn);
            _endBattleChannel.OnBattleStateChanged.RemoveListener(EndBattle);
        }

        private void Start()
        {
            _changeStateDelay = new WaitForSeconds(5f);
            var dieRoll = Random.Range(0, 20);
            _battleState = BattleState.Start;
            _currentPlayerUnit = GameObject.FindGameObjectWithTag("Player");
            _currentEnemyUnit = GameObject.FindGameObjectWithTag("Enemy");

            if (dieRoll > 10)
                StartCoroutine(ChangeState(BattleState.PlayerTurn));
            else
                StartCoroutine(ChangeState(BattleState.EnemyTurn));
        }
       
        private void PlayerCompleteTurn(bool turnComplete)
        {
            _hasPlayerCompletedTurn = turnComplete;
            if (_hasPlayerCompletedTurn && _isBattleOver == false)
                StartCoroutine(ChangeState(BattleState.EnemyTurn));
        }

        private void EnemyCompleteTurn(bool turnComplete)
        {
            _hasEnemyCompletedTurn = turnComplete;
            if (_hasEnemyCompletedTurn && _isBattleOver == false)
                StartCoroutine(ChangeState(BattleState.PlayerTurn));
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
                    _playerTurnChannel.RaisePlayerTurnEvent();
                    break;
                case BattleState.EnemyTurn:
                    _battleState = BattleState.EnemyTurn;
                    BattleUIManager.Instance.ActivateButtons(false);
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.EnemyTurn);
                    _enemyTurnChannel.RaiseEnemyTurnEvent();
                    break;
                case BattleState.Win:
                    _battleState = BattleState.Win;
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.Win);
                    //Time.timeScale = 0;
                    break;
                case BattleState.Lose:
                    _battleState = BattleState.Lose;
                    BattleUIManager.Instance.ToggleTurnIndicators(BattleState.Lose);
                    //Time.timeScale = 0;
                    break;
            }
        }
    }
}
