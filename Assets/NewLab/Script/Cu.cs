using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cu : MetalBase
{
    protected override void ActionDone(TestTube testTube)
    {
        testTube.AddItem(Metal.Cu , 0);
        Destroy(gameObject);
    }
}