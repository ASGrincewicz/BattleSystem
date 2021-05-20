using UnityEngine;
using Veganimus.BattleSystem;
using System.Collections.Generic;
using System;

public class UnitStats : MonoBehaviour, IComparable<UnitStats>
{
    ///<summary>
    ///Assigned in Unit Class on Unit Prefab to determine Stats.
    ///</summary>
    public int Id => UnitHitPoints;
    public int SecondID => UnitDefense;
    public int ThirdID => UnitSpeed;
    [SerializeField] private GameObject _unitModelPrefab;
    public GameObject UnitModelPrefab { get { return _unitModelPrefab; } }
    [SerializeField] private UnitInfo _unitInfo = new UnitInfo();
   
    public string UnitName { get { return _unitInfo.unitName; } }
   
    public int UnitHitPoints { get { return _unitInfo.hitPoints; } }

    public int UnitSpeed { get { return _unitInfo.speed; } }
    
    public int UnitDefense { get { return _unitInfo.defense; } }
   
    public int UnitAccuracyModifier { get { return _unitInfo.accuracyMod; } }

    public ElementType UnitType { get { return _unitInfo.elementType; } }

    [SerializeField] private List<UnitAttackMove> _unitAttackMoves = new List<UnitAttackMove>();
    public List<UnitAttackMove> UnitAttackMoves { get { return _unitAttackMoves; } }

    [SerializeField] private List<UnitDefenseMove> _unitDefenseMoves = new List<UnitDefenseMove>();
    public List<UnitDefenseMove> UnitDefenseMoves { get { return _unitDefenseMoves; } }
    #region Compare
    public int CompareTo(UnitStats other)
    {
        if (this.Id < other.Id)
            return -1;

        else if (this.Id > other.Id)
            return 1;

        else if (Id == other.Id)
        {
            if (SecondID < other.SecondID)
                return -1;

            else if (this.SecondID > other.SecondID)
                return 1;

            else if (this.SecondID == other.SecondID)
            {
                if (this.ThirdID < other.ThirdID)
                    return -1;

                else if (this.ThirdID > other.ThirdID)
                    return 1;

                else
                    return 0;

            }
            else
                return 0;

        }
        else
            return 0;
    }
    #endregion
}

    //public UnityEngine.Playables.PlayableDirector director;
    //public List<GameObject> unitMoveCutscenes = new List<GameObject>();
    //private Animator _animator;

