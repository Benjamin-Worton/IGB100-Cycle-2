using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeAbstract : MonoBehaviour
{
    /// <summary>
    /// Removes upgrade and all objects that rely on the upgrade
    /// </summary>
    public abstract void Remove();
}
