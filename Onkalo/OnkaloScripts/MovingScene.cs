using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingScene : MonoBehaviour
{


    public void MovingRankingSecene(){
        // SceneManager.LoadScene("OnlineTestRanking");
        //ランキングへの移動
    }

    public void MovingSettingSecene(){
        SceneManager.LoadScene("AudioSetting");
    }

    // public void MovingGameSecene(){
    //     // シーン切替時に呼ばれるイベントに登録
    //     SceneManager.sceneLoaded += LoadedToGameScene;
    //     SceneManager.LoadScene("GameScene");
    // }

    public void MovingResult(){
        SceneManager.LoadScene("Result");
    }

    // public void MovingTitle(){
    //     SceneManager.sceneLoaded += LoadedToTitle;
    //     SceneManager.LoadScene("Title");
    // }



}
