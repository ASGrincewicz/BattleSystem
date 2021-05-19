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
        public enum AssetTypeToCreate {Item, AttackMove, DefenseMove }
        private AssetTypeToCreate _assetTypeToCreate;
        private bool groupEnabled;
        private bool isConsumable = true;
        private int uses = 1;
        private int attackDamage;
        private int attackAccuracy;
        private int defenseBuff;
        private MoveType moveType;
        private ElementType elementType;
        private ItemType _itemType;
        private StatAffected _affectedStat;
        private int _effectAmount;
        private Object channel;

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
                case AssetTypeToCreate.Item:
                    _itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", _itemType);
                    isConsumable = EditorGUILayout.Toggle("Is Consumable", isConsumable);
                    uses = EditorGUILayout.IntField("Equipment Uses",uses,GUILayout.Width(200f));
                    _affectedStat = (StatAffected)EditorGUILayout.EnumPopup("Affected Stat", _affectedStat);
                    _effectAmount = EditorGUILayout.IntField("Effect Amount", _effectAmount, GUILayout.Width(200f));
                    break;
                case AssetTypeToCreate.AttackMove:
                    uses = EditorGUILayout.IntField("Attack Move Uses",uses, GUILayout.Width(200f));
                    attackDamage = EditorGUILayout.IntSlider("Attack Damage",attackDamage,5,100, GUILayout.Width(200f));
                    attackAccuracy = EditorGUILayout.IntSlider("Attack Accuracy",attackAccuracy,10,100, GUILayout.Width(200f));
                    moveType = (MoveType)EditorGUILayout.EnumPopup("Move Type", moveType);
                    elementType = (ElementType)EditorGUILayout.EnumPopup("Element Type", elementType);
                    channel = EditorGUILayout.ObjectField(channel, typeof(DisplayActionChannel));
                    break;
                case AssetTypeToCreate.DefenseMove:
                    uses = EditorGUILayout.IntField("Defense Move Uses", uses, GUILayout.Width(200f));
                    defenseBuff = EditorGUILayout.IntSlider("Defense Buff",defenseBuff,0,10, GUILayout.Width(200f));
                    moveType = (MoveType)EditorGUILayout.EnumPopup("Move Type", moveType);
                    channel = EditorGUILayout.ObjectField(channel, typeof(DisplayActionChannel));
                    break;

            }
            var button = GUILayout.Button("Create New Asset", GUILayout.Height(50f), GUILayout.Width(200f));
            if (button && _assetName != "")
                CreateNewItemAsset(_assetTypeToCreate);

        }
        private void CreateNewItemAsset(AssetTypeToCreate asset)
        {
            switch (asset)
            {
                case AssetTypeToCreate.Item:
                    var newItem = CreateInstance<Item>();
                    AssetDatabase.CreateAsset(
                        newItem,
                        $"Assets/Scripts/Scriptable Objects/Items/{_assetName}.asset");
                    newItem.itemType = _itemType;
                    newItem.itemName = _assetName;
                    newItem.isConsumable = isConsumable;
                    newItem.itemUses = uses;
                    newItem.statAffected = _affectedStat;
                    newItem.effectAmount = _effectAmount;
                    break;
                //case AssetTypeToCreate.AttackMove:
                //    var newAttackMove = CreateInstance<UnitAttackMove>();
                //    AssetDatabase.CreateAsset(
                //        newAttackMove,
                //        $"Assets/Scripts/Scriptable Objects/Moves/Attack Moves/{_assetName}.asset");
                //    newAttackMove.MoveName = _assetName;
                //    newAttackMove.MoveUses = uses;
                //    newAttackMove.damageAmount = attackDamage;
                //    newAttackMove.MoveAccuracy = attackAccuracy;
                //    newAttackMove.MoveType = moveType;
                //    newAttackMove.MoveElementType = elementType;
                //    newAttackMove.displayActionChannel = (DisplayActionChannel)channel;
                //    break;
                //case AssetTypeToCreate.DefenseMove:
                //    var newDefenseMove = CreateInstance<UnitDefenseMove>();
                //    AssetDatabase.CreateAsset(
                //        newDefenseMove,
                //        $"Assets/Scripts/Scriptable Objects/Moves/Defense Moves/{_assetName}.asset");
                //    newDefenseMove.MoveName = _assetName;
                //    newDefenseMove.MoveUses = uses;
                //    newDefenseMove.defenseBuff = defenseBuff;
                //    newDefenseMove.MoveType = moveType;
                //    newDefenseMove.displayActionChannel = (DisplayActionChannel)channel;
                //    break;
            }
        }
    }
}
