using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScore : MonoBehaviour
{
    private ISaveData savedata;

    [SerializeField] ResultManager ResultManager;
    [SerializeField] AudioSource SE;

    //保存されてる最後のid
    int id;
    int score;

    void Awake(){
        savedata = new PlayerPrefsSaveData();
        // savedata = new DebugSaveData();//デバッグ用
    }

    void Start(){
        score = GameManager.Getscore();
        // score=9999;//デバッグ用
        Savetoplayerprefs(score);
    }

    public void Savetoplayerprefs(int score){//クリア時にスコアをセーブする
        //前回のスコアidを検索する
        id = savedata.CheckLatestScore();
        //+1する
        // id++;

        Debug.Log("idは"+id);

        //保存する
        savedata.SaveScore(id, score);
    }


    //オンラインランキング用のメソッド
    public void rankload()
    {
        SE.Play();
        //ないちさんのにscoreを与えて呼び出す
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (score,0);
        Rankingfalse.NameInputONOFF=1;
    }
}
