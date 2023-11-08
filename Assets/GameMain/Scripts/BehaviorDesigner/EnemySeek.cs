using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FPS
{
    [TaskCategory("FPS_Enemy")]
    public class EnemySeek : Action
    {
        public float speed;
        public SharedTransform seekTarget;
        public float arriveDistance;
        private float sqrArriveDistance;
        public override void OnStart()
        {
            sqrArriveDistance = arriveDistance;
        }

        public override TaskStatus OnUpdate()
        {
            if (seekTarget==null)
            {
                return TaskStatus.Failure;
            }

            var position = seekTarget.Value.position;
            transform.LookAt(position);
            transform.position =
                Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            if ((seekTarget.Value.position - transform.position).sqrMagnitude<sqrArriveDistance)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}