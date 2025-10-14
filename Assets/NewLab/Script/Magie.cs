using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magie : MetalBase
{
    protected override void ActionDone(TestTube testTube)
    {
        testTube.AddItem(Metal.Mg);
        Destroy(gameObject);
    }
}