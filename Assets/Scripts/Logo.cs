using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour
{
    public RectTransform Mask;

	void Start()
	{
		// 슈퍼마리오 로고를 1초 동안 5f배까지 스케일을 확대한다. easeOutElastic은 처음엔 빨리 확대했다가 느려지면서 마지막엔 튕기는 듯한 효과를 준다.
        LeanTween.scale(gameObject, new Vector3(5f, 5f, 5f), 1.0f).setEase(LeanTweenType.easeOutElastic);
        
        // 스케일 확대가 끝난 1초 후에는 로고의 좌표를 0f, 2.5f, 0f로 1로 동안 이동시키되 PingPong 옵션을 줘서 무한적으로 반복하게 한다.
        LeanTween.move(gameObject, new Vector3(0f, 2.5f, 0f), 1.0f).setDelay(1.0f).setLoopPingPong().setEase(LeanTweenType.easeInOutSine);
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스가 눌린 경우 아무키나 터치된 것으로 간주한다.
        {
            Mask.gameObject.SetActive(true); // 페이드아웃용 흰색 패널을 활성화시킨다.
            LeanTween.alpha(Mask, 1f, 1.0f).setOnComplete(Complete); // 알파값을 1f까지 서서히 증가시켜 패널이 나타나게 한다.
        }
    }

    void Complete()
    {
        Application.LoadLevel(1); // 빌드셋팅에서 1번에 설정된 Scene을 불러온다.
    }
}
