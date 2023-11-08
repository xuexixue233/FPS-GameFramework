using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FPS
{
    public class Enemy_Find : Action
    {
        public SharedInt soundId;
        public override void OnStart()
        {
            if (!GameEntry.Sound.IsLoadingSound(soundId.Value))
            {
                soundId.Value = (int)GameEntry.Sound.PlaySound(10009);
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}