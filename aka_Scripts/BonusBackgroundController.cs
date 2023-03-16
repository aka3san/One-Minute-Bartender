using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusBackgroundController : MonoBehaviour
{
    [SerializeField] Sprite[] bonusBackgroundSprite;
    float animSpeed = 0.018f;
    Image bonusBackgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        bonusBackgroundImage = GetComponent<Image>();
        animSpeed = 0.018f;
        StartCoroutine("AnimateBonusBackground");
    }

    // Update is called once per frame
    void Update()
    {
        bonusBackgroundImage.color += new Color(0, 0, 0, 0.1f);
    }

    IEnumerator AnimateBonusBackground()
    {
        while(true)
        {
            for(int i=0; i<56; i++)
            {
                bonusBackgroundImage.sprite = bonusBackgroundSprite[i];
                yield return new WaitForSeconds(animSpeed);
            } 
        }
    }

    
}
