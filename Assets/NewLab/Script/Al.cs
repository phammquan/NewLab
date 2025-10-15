using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Al : MetalBase
{
    protected override void ActionDone(TestTube testTube)
    {
        testTube.AddItem(Metal.Al, 0);
        Destroy(gameObject);
    }
}
