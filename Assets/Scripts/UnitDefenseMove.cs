﻿using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using Veganimus.BattleSystem;

[CreateAssetMenu(menuName = "Unit Moves/ Defense Move")]
public class UnitDefenseMove : UnitMove
{
    [SerializeField] private int _defenseBuff;
    [SerializeField] private int _defenseStrength;

    public UnityEvent OnDefenseMoveUsed;

    public void RaiseDefenseMoveUsedEvent(string unitName)
    {
        PerformDefenseMove(unitName);
        RaiseMoveQueuedEvent(this);
    }
    private void PerformDefenseMove(string unitName)
    {
        if (moveName != "")
            Debug.Log($"{unitName} used {moveName}!");

        else
            return;
    }
    public void CreateNewDefenseMove(string newMoveName)
    {
        UnitDefenseMove newDefenseMove = CreateInstance<UnitDefenseMove>();
        newDefenseMove.moveName = newMoveName;
        AssetDatabase.CreateAsset(CreateInstance<UnitAttackMove>(), $"Assets/Scripts/Scriptable Objects/Moves/Attack Moves/{newDefenseMove.moveName}.asset");
    }
}