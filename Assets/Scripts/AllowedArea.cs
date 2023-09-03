using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoClearGames
{
    public class AllowedArea : MonoBehaviour
    {
        public event Action TrainTouched;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Neutral"))
            {
                TrainTouched?.Invoke();
            }
        }
    }
}
