[System.Serializable]
public struct AttackButtonInfo
{
    public string attackName;
    public string attackUses;
    public string attackAccuracy;
    public string attackDamage;


    public void FillText(string name, int uses, float accuracy, int damage)
    {
        attackName = name;
        attackUses = $"Uses Left: {uses}";
        attackAccuracy = $"Accuracy: {accuracy}";
        attackDamage = $"Damage: {damage}";
    }
}
