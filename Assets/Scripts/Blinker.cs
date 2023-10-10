using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace NoClearGames
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Blinker : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        
        private CanvasGroup _canvasGroup;
        
        // Start is called before the first frame update
        void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            float alpha = (Mathf.Sin(Time.time * speed) / 2) + 0.5f;
            _canvasGroup.alpha = alpha;


            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
