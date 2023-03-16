using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusScoreController : MonoBehaviour
{
    [SerializeField] Sprite[] plusScoreSprite;
    float animSpeed = 0.018f;
    Image plusScoreImage;
    // Start is called before the first frame update
    void Start()
    {
        plusScoreImage = GetComponent<Image>();
        animSpeed = 0.018f;
        StartCoroutine("AnimatePlusScore");
    }

    // Update is called once per frame
    void Update()
    {
        plusScoreImage.color += new Color(0, 0, 0, 0.1f);
    }

    IEnumerator AnimatePlusScore()
    {
        for (int i = 0; i < 10; i++)
        {
            plusScoreImage.sprite = plusScoreSprite[i];
            yield return new WaitForSeconds(animSpeed);
        }
        Destroy(gameObject);
    }

}
