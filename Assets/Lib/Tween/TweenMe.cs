using UnityEngine;
using System;
using Mog;

public class TweenMe : MonoBehaviour {

	public enum ShowEffect {
		None,
		FromLeft,
		FromRight,
		FromUp,
		FromBottom,
		Scale0to1,
		Alpha,
	}

	public enum ScaleDimension {
		ALL,
		X,
		Y,
		Z,
	}

	public ShowEffect showEffect;
	public ScaleDimension scaleDimension = ScaleDimension.ALL;
	public bool timeIndependent = true;


	public float showTime = 0.4F;
	public float closeTime = 0.3F;
	public bool startPositionCanChange = false;
    public bool showOnEnable = false;
	public float showDelay = 0F;
	float origX;
	RectTransform rt;
	Vector2 origLocalPos, startingPos;
	public LeanTweenType enableTweenType = LeanTweenType.linear;
	public LeanTweenType disableTweenType = LeanTweenType.linear;

	private CanvasGroup cg;
	private bool isInited = false;


    private int? tweenId = null;
	void Awake() {
		if (!isInited) {
			Init();
		}
	}

	protected virtual void Init() {
		isInited = true;
		rt = this.GetComponent<RectTransform>();
		cg = this.GetComponent<CanvasGroup>();
		if (showEffect == ShowEffect.None) {
			showTime = 0F;
			closeTime = 0F;
		}
		origLocalPos = rt.anchoredPosition;
	}

    void CancelTween() {
        if (tweenId.HasValue) {
            LeanTween.cancel(tweenId.Value);
        }
    }

    void OnEnable() {
        if (showOnEnable) {
			Show();
		}
    }

	public void Show(Action onComplete=null) {
		if (!isInited) {
			Init();
			return;
		}
		this.gameObject.SetActive(true);
        CancelTween();

		LTDescr tween = null;
		if (showEffect == ShowEffect.Alpha) {
//			Debug.LogWarning("Tweening alpha");
			if (cg != null) cg.alpha = 0F;
            tween = LeanTween.value(this.gameObject,
			                (val) => { if (cg != null) cg.alpha = val; },
			0, 1,  showTime);
		} else if (showEffect == ShowEffect.Scale0to1) {
			this.gameObject.transform.localScale =  GetScale();
            tween = LeanTween.scale(this.gameObject, Vector3.one, showTime);
		} else {
			if (startPositionCanChange) {
				origLocalPos = rt.anchoredPosition;
			}

			if (showEffect == ShowEffect.FromLeft) {
				startingPos = new Vector2(origLocalPos.x - rt.rect.width, origLocalPos.y);
			} else if (showEffect == ShowEffect.FromRight) {
				startingPos = new Vector2(origLocalPos.x + rt.rect.width, origLocalPos.y);
			} else if (showEffect == ShowEffect.FromUp) {
				startingPos = new Vector2(origLocalPos.x, origLocalPos.y + rt.rect.height);
			} else if (showEffect == ShowEffect.FromBottom) {
				startingPos = new Vector2(origLocalPos.x, origLocalPos.y - rt.rect.height);
			}

			rt.anchoredPosition = startingPos;
            tween = LeanTween.value(this.gameObject, (pos)=> { 
				rt.anchoredPosition = pos;
			}, startingPos, origLocalPos, showTime);
		}

        tween.setDelay(showDelay).setUseEstimatedTime(timeIndependent).setEase(enableTweenType).setOnComplete(()=> { if (onComplete != null) onComplete(); });
        tweenId = tween.id;
	}

	public void Close() {
		Close(null);
	}

    public void CloseDisable() {
        Close(()=> {
            if (this != null) {
                this.gameObject.SetActive(false);
            }
        });
    }

	public void Close(Action onClose) {
		if (this == null || this.gameObject == null)
			return;
		if (!isInited) Init();
        CancelTween();
		LTDescr tween = null;
		if (showEffect == ShowEffect.Alpha) {
            tween = LeanTween.value(this.gameObject,
			                (val) => { if (cg != null) cg.alpha = val; },
			1, 0,  closeTime);
		} else if (showEffect == ShowEffect.Scale0to1) {
			this.gameObject.transform.localScale = Vector3.one;
            tween = LeanTween.scale(this.gameObject, GetScale(), closeTime);
		} else {
			rt.anchoredPosition = origLocalPos;
            tween = LeanTween.value(this.gameObject, (pos)=> { 
				rt.anchoredPosition = pos;
			}, origLocalPos, startingPos, closeTime);
		}
		tween.setUseEstimatedTime(timeIndependent).setEase(disableTweenType).setOnComplete(()=> { if (onClose != null) onClose(); });
        tweenId = tween.id;
	}

	Vector3 GetScale() {
		switch (this.scaleDimension) {
		case ScaleDimension.ALL:
			return Vector3.zero;
		case ScaleDimension.X:
			return new Vector3(0, 1, 1);
		case ScaleDimension.Y:
			return new Vector3(1, 0, 1);
		case ScaleDimension.Z:
			return new Vector3(1, 1, 0);
		}
		return Vector3.one;
	}
}
