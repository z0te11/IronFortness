using UnityEngine;

[CreateAssetMenu(fileName = "Characters", menuName = "IronFortress/Characters", order = 2)]
public class Characters : ScriptableObject
{
    public int levelUnit;
    public int lives;
    public float damage;
    public float damageDisance;
    public float cooldown;       
}
