using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    public interface IMagnet
    {
        public PoleSign GetSign();
        public float GetPower();
    }
}
