using BehaviorDesigner.Runtime.Tasks;
using FPS;

public class EnemyDie : Action
{

    private Enemy enemy;
    
    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemy.enemyData.HP<=0)
        {
            GameEntry.Sound.PlaySound(10007,"Enemy");
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
