using UnityEngine;
using Veganimus.BattleSystem;
using System.Collections.Generic;


public class UnitStats : MonoBehaviour
{
    ///<summary>
    ///Assigned in Unit Class on Unit Prefab to determine Stats.
    ///</summary>

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

    //public UnityEngine.Playables.PlayableDirector director;
    //public List<GameObject> unitMoveCutscenes = new List<GameObject>();
    //private Animator _animator;
}
