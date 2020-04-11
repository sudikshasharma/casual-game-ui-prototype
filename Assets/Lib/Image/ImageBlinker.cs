using UnityEngine;
using Mog;
using UnityEngine.UI;

public class ImageBlinker : MogImage {

    public Sprite[] images;
    public float blinkTime = 0.5F;
    private int count;
    private float lastBlinkTime = 0F;

    protected void OnEnable () {
        count = 0;
    }

    int IncrementCount () {
        count = (count + 1) % images.Length;
        return count;
    }

    void Update () {
        if (images != null && images.Length > 0 && Time.realtimeSinceStartup - lastBlinkTime > blinkTime) {
            lastBlinkTime = Time.realtimeSinceStartup;
            base.Set(images[IncrementCount()]);
        }
    }
}
