using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ExternalBehaviorTree enemyBehaviorTree;
    public BehaviorTree behaviorTree;
    // Start is called before the first frame update
    void Start()
    {
        behaviorTree = gameObject.AddComponent<BehaviorTree>();
        behaviorTree.ExternalBehavior = enemyBehaviorTree;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
