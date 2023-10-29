using System;
using GameFramework.DataTable;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class EnemyData : SoldierData
    {
        [SerializeField]
        private string _Name;
        
        [SerializeField]
        private float _ViewDistance;
        
        [SerializeField]
        private float _FieldOfViewAngle;
        
        [SerializeField]
        private float _WaitMinTime;
        
        [SerializeField]
        private float _WaitMaxTime;
        
        [SerializeField]
        private int _Attack;
        
        [SerializeField]
        private float _AttackInterval;
        
        [SerializeField]
        private float _PatrolViewAngle;
        
        public EnemyData(int entityId, int typeId) : base(entityId, typeId, CampType.Enemy)
        {
            IDataTable<DREnemy> dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            DREnemy drEnemy = dtEnemy.GetDataRow(TypeId);
            if (drEnemy==null)
            {
                return;
            }

            _ViewDistance = drEnemy.ViewDistance;
            _FieldOfViewAngle = drEnemy.FieldOfViewAngle;
            _WaitMinTime = drEnemy.WaitMinTime;
            _WaitMaxTime = drEnemy.WaitMaxTime;
            _Attack = drEnemy.Attack;
            _AttackInterval = drEnemy.AttackInterval;
            _PatrolViewAngle = drEnemy.PatrolViewAngle;
        }

        public string Name
        {
            get => _Name;
            set => _Name = value;
        }

        public float ViewDistance => _ViewDistance;

        public float FieldOfViewAngle => _FieldOfViewAngle;

        public float WaitMinTime => _WaitMinTime;

        public float WaitMaxTime => _WaitMaxTime;

        public int Attack => _Attack;

        public float AttackInterval => _AttackInterval;

        public float PatrolViewAngle => _PatrolViewAngle;
    }
}