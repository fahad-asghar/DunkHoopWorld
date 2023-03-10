using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FloatingMovement : MonoBehaviour
{    
    public void Move(Transform target, int rewardAmount, string coinOrDimaond)
    {
        transform.GetChild(0).GetComponent<Text>().text = "+" + rewardAmount;

        transform.GetComponent<Image>().DOFade(0, 0);
        transform.GetChild(0).GetComponent<Text>().DOFade(0, 0).OnComplete(delegate() {
        transform.GetChild(0).GetComponent<Text>().DOFade(1, 0.3f);
            transform.DOLocalMoveY(-350, 1, false).OnComplete(delegate ()
            {
                transform.GetChild(0).GetComponent<Text>().DOFade(0, 0.2f).OnComplete(delegate ()
                {
                    transform.GetComponent<Image>().DOFade(1, 0.3f).OnComplete(delegate ()
                    {
                        transform.DOMove(target.position, 1, false).OnComplete(delegate ()
                        {
                            if(coinOrDimaond.Equals("Coin"))
                                RewardManager.instance.AddCoins(rewardAmount);
                            else
                                RewardManager.instance.AddDiamonds(rewardAmount);

                            Destroy(gameObject);
                        });                     
                    });
                });
            });
        });
    }
}
