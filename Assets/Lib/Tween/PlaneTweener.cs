using UnityEngine;
using UnityEngine.UI;
using System;
using Mog;

public class PlaneTweener : TweenMe
{
    public Image themeBg, rotatingBg;

    /*
	protected override void Init () {
		showTime = 0.8F;
		closeTime = 0.4F;
		enableTweenType = LeanTweenType.easeSpring;
		base.Init();
	}
	*/

    void OnEnable()
    {
        if (showOnEnable)
        {
            Show();
        }
    }

    public new void Show(Action onComplete = null)
    {
        ToggleBg(false);
        base.Show(() =>
        {
            if (this != null)
            {
                ToggleBg(true);
                if (onComplete != null)
                    onComplete();
            }
        });
    }


    public new void Close()
    {
        Close(null);
    }

    public new void Close(Action onClose)
    {
        ToggleBg(false);
        base.Close(() =>
        {
            if (this != null && this.gameObject != null)
            {
                this.gameObject.SetActive(false);
            }
            if (onClose != null)
                onClose();
        });
    }

    void ToggleBg(bool value)
    {
        if (this == null) return;
        if (rotatingBg != null && rotatingBg.gameObject != null)
            rotatingBg.gameObject.SetActive(value);
        if (themeBg != null && themeBg.gameObject != null)
            themeBg.gameObject.SetActive(value);
    }

}
