using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
 
public class DebugSaveData : ISaveData
{
    public void SaveMoney(BigInteger money)
    {
    }
 
    public void SaveSheepCnt(int id, int cnt)
    {
    }
 
    public BigInteger LoadMoney()
    {
        return 10000000000; //デバッグ用に大金
    }
 
    public int LoadSheepCnt(int id)
    {
        return 5; //デバッグ用に最初から全羊が５匹居る
    }

    //自前実装=======================
    //マイスコアのセーブ
    public void SaveScore(int id, int score)
    {
        //score0に強制的にセーブ
        PlayerPrefs.SetInt($"SCORE{id}", Random.Range(10,20));
    }

    //マイスコアのロード
    public int LoadScore(int id)
    {
        //PlayerPrefsを使用した場合の頭数ロード処理
        return PlayerPrefs.GetInt($"SCORE{id}", -1);
    }

    //最後のマイスコアの確認
    public int CheckLatestScore(){
        int id=0;
        while (PlayerPrefs.HasKey ($"SCORE{id}"))
        {
            Debug.Log("最終id確認"+id);

            id++;

            //エラー処理
            if(id==99)  break;
        }

        return id;
    }
}