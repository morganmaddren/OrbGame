using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class WellCenter : Bumper
{
    Well well;

    public override void OnCenterBump(IOrb orb)
    {
        well.OnCenterHit(orb);
    }

    protected override void OnConstruct()
    {
        well = transform.parent.GetComponentStrict<Well>();
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }
}