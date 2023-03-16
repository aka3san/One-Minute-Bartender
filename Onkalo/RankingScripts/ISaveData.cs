using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
 
public interface ISaveData
{
    //所持金のセーブ
    void SaveMoney(BigInteger money);
    //羊頭数のセーブ
    void SaveSheepCnt(int id, int cnt);
 
    //所持金のロード
    BigInteger LoadMoney();
    //羊頭数のロード
    int LoadSheepCnt(int id);

    //自前実装=================
    //スコアのセーブ
    void SaveScore(int id, int score);

    //スコアの確認
    //idはスコアのid
    int LoadScore(int id);

    //直前のidの確認
    int CheckLatestScore();

}