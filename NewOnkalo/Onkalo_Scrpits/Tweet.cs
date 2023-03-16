using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweet : MonoBehaviour
{
    int score;

    void Start(){
        score = GameManager.Getscore();
    }
    public void ScoreTweet(){
        string tweet ="One Minute Bartenderを遊びました。 スコアは"+score+"でした！\n";
        naichilab.UnityRoomTweet.Tweet ("oneminutebartender", tweet, "unityroom","ニコラシカ","ワンミニッツバーテンダー");
    }
}
