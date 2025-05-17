using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercilessProgramming : UpgradeAbstract
{
    private int mercilessStacks = 0;

    // Called when enemies die
    public void GiveMerciless()
    {
        if (mercilessStacks >= 3) { return; }
        mercilessStacks++;
        GetComponent<Player>().cooldownMultiplier -= 0.1f;
        StartCoroutine(RemoveMerciless());
    }

    private IEnumerator RemoveMerciless()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Player>().cooldownMultiplier += 0.1f;
        mercilessStacks--;
    }

    // Remove stats
    public override void Remove()
    {
        GetComponent<Player>().cooldownMultiplier += 0.1f * mercilessStacks;
        Destroy(this);
    }
}
