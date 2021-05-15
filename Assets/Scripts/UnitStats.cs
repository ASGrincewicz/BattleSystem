using UnityEngine;
using Veganimus.BattleSystem;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Unit Stats")]
public class UnitStats : ScriptableObject
{
    ///<summary>
    ///Assigned in Unit Class on Unit Prefab to determine Stats.
    ///</summary>
    public string unitName;
    public ElementType unitType;
    public int unitHitPoints;
    public int unitSpeed;
    public int unitDefense;
    public int accuracyModifier;
    public List<UnitAttackMove> unitAttackMoves = new List<UnitAttackMove>();
    public List<UnitDefenseMove> unitDefenseMoves = new List<UnitDefenseMove>();
    public UnityEngine.Playables.PlayableDirector director;
    public List<GameObject> unitMoveCutscenes = new List<GameObject>();
    private Animator animator;


}