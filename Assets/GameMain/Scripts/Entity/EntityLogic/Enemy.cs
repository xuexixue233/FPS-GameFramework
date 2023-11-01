using BehaviorDesigner.Runtime;

namespace FPS
{
    public class Enemy : Soldier
    {
        public BehaviorTree _behaviorTree;
        public EnemyData enemyData;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _behaviorTree=gameObject.GetComponent<BehaviorTree>();
        }

        private void InitBehaviorTree()
        {
            _behaviorTree.GetVariable("ViewDistance").SetValue(enemyData.ViewDistance);
            _behaviorTree.GetVariable("FieldOfViewAngle").SetValue(enemyData.FieldOfViewAngle);
            _behaviorTree.GetVariable("WaitMinTime").SetValue(enemyData.WaitMinTime);
            _behaviorTree.GetVariable("WaitMaxTime").SetValue(enemyData.WaitMaxTime);
            _behaviorTree.GetVariable("AttackInterval").SetValue(enemyData.AttackInterval);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            enemyData = userData as EnemyData;
            if (enemyData==null)
            {
                return;
            }
            //启动行为树
            InitBehaviorTree();
            _behaviorTree.Start();
        }

        protected override void OnDead(Entity attacker)
        {
            _behaviorTree.OnDestroy();
            base.OnDead(attacker);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            
        }
    }
}