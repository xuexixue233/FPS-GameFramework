using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

namespace FPS
{
    public class EnemyPatrolView : Action
    {
        public SharedFloat patrolViewAngle;
        private Vector3 rotationView;
        private Vector3 minRotation;
        private Vector3 maxRotation;
        private Vector3 targetRotation;
        private Tween _tween;
        public SharedGameObject target;
        
        public override void OnStart()
        {
            rotationView = transform.localRotation.eulerAngles;
            minRotation=rotationView-new Vector3(0f,patrolViewAngle.Value,0f);
            maxRotation=rotationView+new Vector3(0f,patrolViewAngle.Value,0f);
            targetRotation = minRotation;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value!=null)
            {
                return TaskStatus.Failure;
            }

            _tween ??= transform.DOLocalRotate(targetRotation, 3).SetEase(Ease.Linear).OnComplete((() =>
            {
                targetRotation = targetRotation == minRotation ? maxRotation : minRotation;
                _tween = null;
            }));
            return TaskStatus.Running;
        }
    }
}
