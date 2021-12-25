using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace Vampire.Scenario
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField]
        Button menuButton;
        [SerializeField]
        Button skipButton;
        [SerializeField]
        Button logButton;

        [SerializeField]
        RectTransform menuObject;

        bool canMove=true;

        readonly float[] posX = { 132,40 };
        const float moveTime=0.5f;

        // Start is called before the first frame update
        void Start()
        {
            menuButton.onClick.AsObservable()
                .Where(_=>canMove)
                .Select(_=>1)
                .Scan((acc,current)=>acc+current)
                .Subscribe(_ => {
                    canMove = false;
                    menuObject.DOAnchorPosX(posX[_ % 2], moveTime).OnComplete(()=>
                    {
                        canMove = true;
                    });       
                });
        }
        
    }
}
