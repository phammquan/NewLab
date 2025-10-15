// File: IronNail.cs

using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class IronNail : MetalBase
{
    protected override void ActionDone(TestTube testTube)
    {
        testTube.AddItem(Metal.Fe, 2f);
        Destroy(gameObject);
    }
}