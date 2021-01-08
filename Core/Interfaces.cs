using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface ILockable
    {
        bool IsAlive();
        Transform GetLockOnTarget(Transform from);
    }
}
