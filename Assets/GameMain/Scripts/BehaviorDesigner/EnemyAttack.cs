using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FPS
{
    [TaskCategory("FPS_Enemy")]
    public class EnemyAttack : Action
    {
        public SharedFloat attackInterval;
        public SharedGameObject target;
        private float currentTime;
        private Enemy enemy;
        private float successOn;
        public override void OnStart()
        {
            currentTime = 0;
            successOn = 0.5f;
            enemy = GetComponent<Enemy>();
        }

        public override TaskStatus OnUpdate()
        {
            if (target==null)
            {
                successOn = 0.5f;
                return TaskStatus.Failure;
            }
            var position = target.Value.transform.position;
            transform.LookAt(position);
            currentTime += Time.deltaTime;
            if (currentTime>attackInterval.Value)
            {
                Debug.Log("敌人射击一次");
                if (ShootOnSuccess())
                {
                    Debug.Log("敌人射中");
                    enemy.ShootSuccess(target.Value);
                }
                successOn += successOn/2;
                currentTime = 0;
            }
            return TaskStatus.Running;
        }

        private bool ShootOnSuccess()
        {
            float probability = Random.Range(0, 1);
            return probability<successOn;
        }
    }
}
