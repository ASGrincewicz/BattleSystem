using UnityEngine;
using TMPro;

public class AttackButton : MonoBehaviour
{
    public AttackButtonInfo attackButtonInfo = new AttackButtonInfo();

    public TMP_Text attackName;
    public TMP_Text attackUses;
    public TMP_Text attackAccuracy;
    public TMP_Text attackDamage;

    public void Start()
    {
        UpdateButtonText();
    }

    public void UpdateButtonText()
    {
        attackName.text = attackButtonInfo.attackName;
        attackUses.text = attackButtonInfo.attackUses;
        attackAccuracy.text = attackButtonInfo.attackAccuracy;
        attackDamage.text = attackButtonInfo.attackDamage;
    }
}
