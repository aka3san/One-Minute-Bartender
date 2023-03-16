using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MybestManager : MonoBehaviour
{
    private ISaveData savedata;

    //保存されてる最後のid
    int latestid;

    //マイスコアを格納するリスト
    List<int> ScoreList = new List<int>();

    //変更するテキスト
    [SerializeField] GameObject Mybesetscore;
    [SerializeField] GameObject MybesetscoreFromT;



    void Awake(){
        savedata = new PlayerPrefsSaveData();
        // savedata = new DebugSaveData();//デバッグ用
    }

    void Start()
    {
        Debug.Log("ロード");
 
        //スコアの数(id)を入手
        latestid = savedata.CheckLatestScore();

        //リストにマイベストを入れる
        for(int i=0;i<=latestid;i++){
            ScoreList.Add(savedata.LoadScore(i));
        }

        //降順にソート
        ScoreList.Sort();
        ScoreList.Reverse();

        //2位、3位が無い場合に0を代入
        //プログラムが合ってるかわからない
        if(latestid==0){
            ScoreList.Add(0);
            ScoreList.Add(0);
        }
        
        if(latestid==1)    ScoreList.Add(0);//最後のidが1の時＝スコアが2つしか無い時

        //確認用
        foreach(int x in ScoreList){
            Debug.Log("リストの中身を確認中・中身:"+x);
        }

        //マイベストスコアを更新
        //1位-3位を更新
        Mybesetscore.transform.GetChild(1).gameObject.GetComponent<Text>().text =ScoreList[0].ToString();
        Mybesetscore.transform.GetChild(2).gameObject.GetComponent<Text>().text =ScoreList[1].ToString();
        Mybesetscore.transform.GetChild(3).gameObject.GetComponent<Text>().text =ScoreList[2].ToString();

        //マイベストスコアを更新
        //1位-3位を更新
        MybesetscoreFromT.transform.GetChild(1).gameObject.GetComponent<Text>().text =ScoreList[0].ToString();
        MybesetscoreFromT.transform.GetChild(2).gameObject.GetComponent<Text>().text =ScoreList[1].ToString();
        MybesetscoreFromT.transform.GetChild(3).gameObject.GetComponent<Text>().text =ScoreList[2].ToString(); 
    }
}
