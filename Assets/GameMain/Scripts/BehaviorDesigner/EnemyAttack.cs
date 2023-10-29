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

        public override void OnStart()
        {
            currentTime = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (target==null)
            {
                return TaskStatus.Failure;
            }
            var position = target.Value.transform.position;
            transform.LookAt(position);
            currentTime += Time.deltaTime;
            if (currentTime>attackInterval.Value)
            {
                Debug.Log("敌人射击一次");
                currentTime = 0;
            }
            return TaskStatus.Running;
        }
    }
}
