using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NoClearGames.UI;
using UnityEngine;

namespace NoClearGames
{
    public partial class DialoguePopUp : BasePage
    {
        public float hidePositionY;
        public float moveDuration = 0.1f;
        
        public void MoveUp()
        {
            var transform1 = transform;
            var defaultPosition = transform1.position;

            Vector3 hidePos = defaultPosition;
            hidePos.y = hidePositionY;

            transform1.position = hidePos;

            transform1.DOMove(defaultPosition, moveDuration);
        }

        public void MoveDown()
        {
            var transform1 = transform;
            var defaultPosition = transform1.position;

            Vector3 hidePos = defaultPosition;
            hidePos.y = hidePositionY;
            transform1.DOMove(hidePos, moveDuration);
        }
    }
}
