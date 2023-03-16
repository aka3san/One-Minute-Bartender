using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeNaichiRanking : MonoBehaviour
{
    int score =200;
    
    public void rankload()
    {
        //ないちさんのにscoreを与えて呼び出す
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (score,0);
    }
}
