using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    public float fadeDelay;
    private float fadeTimer;
    public GameObject afterImage;
    public bool createAfterimage = false;
    // Start is called before the first frame update
    void Start()
    {
        fadeTimer = fadeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(createAfterimage)
        {
            if(fadeTimer > 0)
            {
                fadeTimer -= Time.deltaTime;
            } else {
                GameObject thisAfterimage = Instantiate(afterImage, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                thisAfterimage.GetComponent<SpriteRenderer>().sprite = currentSprite;
                fadeTimer = fadeDelay;
                Destroy(thisAfterimage, 0.7f);
            }
        }
    }
}
