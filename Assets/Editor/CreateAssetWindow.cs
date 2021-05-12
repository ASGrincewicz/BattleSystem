using UnityEditor;
using UnityEngine;


namespace Veganimus.BattleSystem
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    public class CreateAssetWindow : EditorWindow
    {
        private static string _windowTitle = "Create Asset";
        private string _assetName;
        public enum AssetTypeToCreate { [InspectorName("Health Item")]HealthItem, EquipmentItem, AttackMove, DefenseMove }
        private AssetTypeToCreate _assetTypeToCreate;
        private bool groupEnabled;
        private bool isConsumable = true;
        private int uses = 1;
        private int attackDamage;
        private int attackAccuracy;
        private int defenseBuff;
        private MoveType moveType;
        private ElementType elementType;
        private int healAmount;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Custom/Create Asset")]
        private static void CreateItemWindowInit()
        {
            // Get existing open window or if none, make a new one:
            CreateAssetWindow window = (CreateAssetWindow)GetWindow(t: typeof(CreateAssetWindow), false, _windowTitle);
            window.Show();
        }

        private void OnGUI()
        {

            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            _assetName = EditorGUILayout.TextField("Asset Name", _assetName);
            _assetTypeToCreate = (AssetTypeToCreate)EditorGUILayout.EnumPopup("Type to Create", _assetTypeToCreate);
            switch (_assetTypeToCreate)
            {
                case AssetTypeToCreate.HealthItem:
                    isConsumable = EditorGUILayout.Toggle("Is Consumable", isConsumable);
                    uses = EditorGUILayout.IntField("Health Item Uses",uses, GUILayout.Width(200f));
                    healAmount = EditorGUILayout.IntField("Health Item Uses", healAmount, GUILayout.Width(200f));
                    break;
                case AssetTypeToCreate.EquipmentItem:
                    isConsumable = EditorGUILayout.Toggle("Is Consumable", isConsumable);
                    uses = EditorGUILayout.IntField("Equipment Uses",uses,GUILayout.Width(200f));
                    break;
                case AssetTypeToCreate.AttackMove:
                    uses = EditorGUILayout.IntField("Attack Move Uses",uses, GUILayout.Width(200f));
                    attackDamage = EditorGUILayout.IntSlider("Attack Damage",attackDamage,5,100, GUILayout.Width(200f));
                    attackAccuracy = EditorGUILayout.IntSlider("Attack Accuracy",attackAccuracy,10,100, GUILayout.Width(200f));
                    moveType = (MoveType)EditorGUILayout.EnumPopup("Move Type", moveType);
                    elementType = (ElementType)EditorGUILayout.EnumPopup("Element Type", elementType);
                    break;
                case AssetTypeToCreate.DefenseMove:
                    uses = EditorGUILayout.IntField("Defense Move Uses", uses, GUILayout.Width(200f));
                    defenseBuff = EditorGUILayout.IntSlider("Defense Buff",defenseBuff,0,10, GUILayout.Width(200f));
                    moveType = (MoveType)EditorGUILayout.EnumPopup("Move Type", moveType);
                    break;

            }
            var button = GUILayout.RepeatButton("Create New Asset", GUILayout.Height(50f), GUILayout.Width(200f));
            if (button && _assetName != "")
                CreateNewItemAsset(_assetTypeToCreate);

        }
        private void CreateNewItemAsset(AssetTypeToCreate asset)
        {
            switch (asset)
            {
                case AssetTypeToCreate.HealthItem:
                    var newHealthItem = CreateInstance<HealthItem>();
                    AssetDatabase.CreateAsset(
                        newHealthItem,
                        $"Assets/Scripts/Scriptable Objects/Items/Consumables/{_assetName}.asset");
                    newHealthItem.itemName = _assetName;
                    newHealthItem.isConsumable = isConsumable;
                    newHealthItem.itemUses = uses;
                    newHealthItem.healAmount = healAmount;
                    break;
                case AssetTypeToCreate.EquipmentItem:
                    var newEqupmentItem = CreateInstance<EquipmentItem>();
                    AssetDatabase.CreateAsset(
                        newEqupmentItem,
                        $"Assets/Scripts/Scriptable Objects/Items/Equippables/{_assetName}.asset");
                    newEqupmentItem.itemName = _assetName;
                    newEqupmentItem.isConsumable = isConsumable;
                    newEqupmentItem.itemUses = uses;
                    break;
                case AssetTypeToCreate.AttackMove:
                    var newAttackMove = CreateInstance<UnitAttackMove>();
                    AssetDatabase.CreateAsset(
                        newAttackMove,
                        $"Assets/Scripts/Scriptable Objects/Moves/Attack Moves/{_assetName}.asset");
                    newAttackMove.moveName = _assetName;
                    newAttackMove.moveUses = uses;
                    newAttackMove.damageAmount = attackDamage;
                    newAttackMove.moveAccuracy = attackAccuracy;
                    newAttackMove.moveType = moveType;
                    newAttackMove.elementType = elementType;
                    break;
                case AssetTypeToCreate.DefenseMove:
                    var newDefenseMove = CreateInstance<UnitDefenseMove>();
                    AssetDatabase.CreateAsset(
                        newDefenseMove,
                        $"Assets/Scripts/Scriptable Objects/Moves/Defense Moves/{_assetName}.asset");
                    newDefenseMove.moveName = _assetName;
                    newDefenseMove.moveUses = uses;
                    newDefenseMove.defenseBuff = defenseBuff;
                    newDefenseMove.moveType = moveType;
                    break;
            }
        }
    }
}
