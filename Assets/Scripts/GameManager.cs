using System.Collections;
using System.Collections.Generic;
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
        
        [Header("Broadcasting on:")]
        [SerializeField] private PlayerTurnChannel _playerTurnChannel;
        [SerializeField] private EnemyTurnChannel _enemyTurnChannel;
        [Header("Listening to")]
        [SerializeField] private TurnCompleteChannel _playerTurnCompleteChannel;
        [SerializeField] private TurnCompleteChannel _enemyTurnCompleteChannel;

        private void OnEnable()
        {
            _playerTurnCompleteChannel.OnTurnComplete.AddListener(PlayerCompleteTurn);
            _enemyTurnCompleteChannel.OnTurnComplete.AddListener(EnemyCompleteTurn);
        }

        private void OnDisable()
        {
            _playerTurnCompleteChannel.OnTurnComplete.RemoveListener(PlayerCompleteTurn);
            _enemyTurnCompleteChannel.OnTurnComplete.RemoveListener(EnemyCompleteTurn);
        }

        private void Start()
        {
            var dieRoll = Random.Range(0, 20);
            _battleState = BattleState.Start;
            _currentPlayerUnit = GameObject.FindGameObjectWithTag("Player");
            _currentEnemyUnit = GameObject.FindGameObjectWithTag("Enemy");

            if (dieRoll > 10)
             ChangeState(BattleState.PlayerTurn);
            
            else
             ChangeState(BattleState.EnemyTurn);
        }
       
        private void PlayerCompleteTurn(bool turnComplete)
        {
            _hasPlayerCompletedTurn = turnComplete;
            if (_hasPlayerCompletedTurn)
             ChangeState(BattleState.EnemyTurn);
        }

        private void EnemyCompleteTurn(bool turnComplete)
        {
            _hasEnemyCompletedTurn = turnComplete;
            if (_hasEnemyCompletedTurn)
             ChangeState(BattleState.PlayerTurn);
        }

        private void ChangeState(BattleState newState)
        {
            switch(newState)
            {
                case BattleState.PlayerTurn:
                    _battleState = BattleState.PlayerTurn;
                    _playerTurnChannel.RaisePlayerTurnEvent();
                    break;
                case BattleState.EnemyTurn:
                    _battleState = BattleState.EnemyTurn;
                    _enemyTurnChannel.RaiseEnemyTurnEvent();
                    break;
                case BattleState.Win:
                    //Display win sequence
                    break;
                case BattleState.Lose:
                    //Display losing sequence;
                    break;
            }
        }
    }
}
