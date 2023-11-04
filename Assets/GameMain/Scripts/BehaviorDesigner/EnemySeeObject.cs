using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FPS
{
    [TaskCategory("FPS_Enemy")]
    public class EnemySeeObject : Conditional
    {
        public SharedTransform sawTarget;
        public Transform[] targets;
        public float fieldOfViewAngle;
        public float viewDistance;


        public override TaskStatus OnUpdate()
        {
            if (targets==null)
            {
                return TaskStatus.Failure;
            }

            foreach (var target in targets)
            {
                var targetPosition = target.position;
                var position = transform.position;
                var distance = (targetPosition - position).magnitude;
                var angle = Vector3.Angle(transform.forward, targetPosition - position);
                if (distance < viewDistance && angle < fieldOfViewAngle*0.5f)
                {
                    sawTarget.Value = target;
                    return TaskStatus.Success;
                }
            }

            return TaskStatus.Failure;
        }
    }
}
