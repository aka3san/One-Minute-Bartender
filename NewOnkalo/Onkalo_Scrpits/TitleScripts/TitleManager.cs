using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class TitleManager : MonoBehaviour
{

    [SerializeField] AudioSource BGM;

    [SerializeField] AudioSource SE;

    void Start(){
        BGM.volume = AudioSettingManager.Audio_BGM();

        SE.volume = AudioSettingManager.Audio_SE();
    }



    IEnumerator waitAudio(){
        //1秒停止
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
        audioSettingManager.from="Title";
        // イベントから削除
        SceneManager.sceneLoaded -= LoadedToAudioSetting;
    }




    IEnumerator waitGame(){
        //1秒停止
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene("GameScene");
    }
    public void MovingGameSecene(){
        SE.Play();
        StartCoroutine("waitGame");
    }


    //オンラインランキング用のメソッド
    public void rankload()
    {
        SE.Play();
        //ないちさんのにscoreを与えて呼び出す
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (0,0);
        Rankingfalse.NameInputONOFF=0;
    }
}
