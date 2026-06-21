using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private PlayerPool _playerPool;
    [SerializeField] private EnemyPool _enemyPool;

    public void SpawnEnemy(GameObject enemy, Transform pos)
    {
        GameObject newEnemy = Instantiate(enemy, pos.position, Quaternion.identity);
        EnemyMoverToBaseBehavior mewEnemyMover = newEnemy.GetComponent<EnemyMoverToBaseBehavior>();
    }
}
