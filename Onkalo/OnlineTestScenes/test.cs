using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ないちさんのに120を与えて呼び出す
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (120);
    }
}
