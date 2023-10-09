using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace NoClearGames
{
    public class Rotator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.transform.DOLocalRotate(new Vector3(0, 360), 3, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }

    }
}
