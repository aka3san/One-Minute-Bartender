using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTextController : MonoBehaviour
{
    private float animationLength;
    private float currentAnimationTime;

    // Start is called before the first frame update
    void Start()
    {
        currentAnimationTime = 0;
        AnimatorStateInfo animatorStateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        animationLength = animatorStateInfo.length;
    }

    // Update is called once per frame
    void Update()
    {
        currentAnimationTime += Time.deltaTime;
        if (currentAnimationTime >= animationLength)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
