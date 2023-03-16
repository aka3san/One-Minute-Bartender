using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public int Score;

    public static int STscore=-1;

    [SerializeField] Text text;

    [SerializeField] AudioSource BGM;

    [SerializeField] AudioSource SE;

    void Start(){
        // if(STscore==-1) STscore=Score;

        // STscore = GameManager.Getscore();

        Score =  GameManager.Getscore();

        text.text=Score.ToString();

        BGM.volume = AudioSettingManager.Audio_BGM();

        SE.volume = AudioSettingManager.Audio_SE();
    }


    IEnumerator waitAudio(){
        yield return new WaitForSeconds(0.25f);
        SceneManager.sceneLoaded += LoadedToAudioSetting;
        SceneManager.LoadScene("AudioSetting");
    }
    public void MovingAudioSettingSecene(){
        SE.Play();
        StartCoroutine("waitAudio");
    }
    private void LoadedToAudioSetting(Scene next, LoadSceneMode mode)
    {
        // シーン切り替え後のスクリプトを取得
        var audioSettingManager= GameObject.FindWithTag("AudioSetting").GetComponent<AudioSettingManager>();
        // データを渡す処理。
        audioSettingManager.from="Result";
        // イベントから削除
        SceneManager.sceneLoaded -= LoadedToAudioSetting;
    }




    IEnumerator waitGame(){
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("GameScene");
    }
    public void MovingGameSecene(){
        // STscore=-1;//STの初期化
        SE.Play();
        StartCoroutine("waitGame");
    }



    IEnumerator waitTitle(){
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Title");
    }
    public void MovingTitleSecene(){
        // STscore=-1;//STの初期化
        SE.Play();
        StartCoroutine("waitTitle");
    }
}
