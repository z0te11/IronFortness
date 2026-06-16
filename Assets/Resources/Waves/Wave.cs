using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "IronFortress/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [Header("Основные настройки")]
    public int numberWave;
    public float delayBetweenSpawns; 
    
    [Header("Состав волны")]
    public int[] levelUnit; 
    public int enemyCount;            
}
