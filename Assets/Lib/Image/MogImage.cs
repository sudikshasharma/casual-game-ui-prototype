using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MogImage : MonoBehaviour {

    private Image img;
    private SpriteRenderer sr;

    protected void Awake() {
        this.img = this.GetComponent<Image>();
        this.sr = this.GetComponent<SpriteRenderer> ();
        if (img == null && sr == null) {
            Debug.LogError(this.gameObject.name + " must have image or sprite");
            this.enabled = false;
        }
    }

    public void Set(Sprite sr) {
        if (this.img != null) {
            this.img.sprite = sr;
        }
        if (this.sr != null) {
            this.sr.sprite = sr;
        }
    }

    public Sprite Get() {
        if (this.img != null) {
            return this.img.sprite;
        }
        if (this.sr != null) {
            return this.sr.sprite;
        }
        return null;
    }
}
