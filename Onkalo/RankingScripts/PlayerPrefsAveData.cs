using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
 
public class PlayerPrefsSaveData : ISaveData
{
    //所持金のセーブ
    public void SaveMoney(BigInteger money)
    {
        //PlayerPrefsを使用した場合の所持金セーブ処理
        PlayerPrefs.SetString("MONEY", money.ToString()); 
    }
    //羊頭数のセーブ
    public void SaveSheepCnt(int id, int cnt)
    {
        //PlayerPrefsを使用した場合の頭数セーブ処理
        PlayerPrefs.SetInt($"SHEEP{id}", cnt);
    }
    //所持金のロード
    public BigInteger LoadMoney()
    {
        //PlayerPrefsを使用した場合の所持金ロード処理
        return BigInteger.Parse(PlayerPrefs.GetString("MONEY", "0"));
    }
    //羊頭数のロード
    public int LoadSheepCnt(int id)
    {
        //PlayerPrefsを使用した場合の頭数ロード処理
        return PlayerPrefs.GetInt($"SHEEP{id}", 0);
    }

    //自前実装=======================
    //マイスコアのセーブ
    public void SaveScore(int id, int score)
    {
        //PlayerPrefsを使用した場合の頭数セーブ処理
        PlayerPrefs.SetInt($"SCORE{id}", score);
    }

    //マイスコアのロード
    public int LoadScore(int id)
    {
        //PlayerPrefsを使用した場合の頭数ロード処理
        return PlayerPrefs.GetInt($"SCORE{id}", 0);
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