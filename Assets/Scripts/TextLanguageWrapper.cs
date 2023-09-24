using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NoClearGames
{
    
    [RequireComponent(typeof(TMP_Text))]
    public class TextLanguageWrapper : MonoBehaviour
    {
        [SerializeField] private string key;
        
        // Start is called before the first frame update
        void Start()
        {
#if UNITY_WEBGL
            TMP_Text text = GetComponent<TMP_Text>();
            text.text = SharedState.LanguageDefs[key];
#endif
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
