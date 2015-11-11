using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    bool isDead = false; // 동전이 마리오에게 먹힌 상태인지를 나타낸다.
    public SpriteRenderer sprite; // 스프라이트 컴포넌트를 인스펙터에서 연결해둔다.

	// 오브젝트 풀링에서 사용할 것이므로 Start() 함수가 아니라 SetActive(true)가 호출될때 콜백되는 함수인 OnEnable()때 초기화해준다.
	// 오브젝트 풀링의 핵심은 OnEnable()에서 오브젝트가 방금 생성된 것처럼 모든 값을 초기화 해주는데 있다.
	void OnEnable()
	{
        isDead = false;
        sprite.enabled = true; // 스프라이트 렌더러를 다시 켜준다.
        rigidbody2D.AddForce(new Vector2(Random.Range(-0.3f, 0.3f), 1f) * 400f); // Y축은 1f이므로 위쪽 방향 X축은 -0.3f ~ 0.3f까지 랜덤한 방향으로 동전을 날려준다.
        Invoke("DestroyCoin", 5.0f); // 마리오가 먹지 못하면 5초 후에 동전을 다시 비활성화 시킨다.
	}

	// 동전과 마리오에는 isTrigger가 true 상태인 충돌 처리용 컬라이더를 1개씩 더 추가해준다.
	// 특히 동전은 자식과 부모의 Layer 값을 다르게 해서 부모는 마리오와 충돌을 금지하고, isTrigger가 true인 자식은 충돌하게 해서
	// 동전을 먹을 수 있게 해준다. 그래야만 마리오가 물리 엔진 떄문에 동전을 밀고 나가는 일이 없게 된다.
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Equals("mario")) // 충돌한 컬라이더의 주인이 마리오인지 확인
        {
            if (!isDead) // 동전이 살아있는 상태면
            {
                audio.Play(); // 동전에 연결된 먹는 소리 플레이
                sprite.enabled = false; // 스프라이트를 꺼서 보이지 않게 한다.
                CancelInvoke("DestroyCoin"); // 기존에 실행되고 있던 Invoke가 있으면 취소시킨다.
                Invoke("DestroyCoin", 0.5f); // 0.5초 후에 코인을 비활성화 한다. 동전 먹는 소리가 다 연주될때까지 기다리기 위함이다.
                isDead = true; // 죽은 동전으로 처리한다.
            }
        }
    }

    void DestroyCoin()
    {
        gameObject.SetActive(false); // 오브젝트 풀에서 이 동전을 비활성화 시켜 재사용이 가능한 상태로 만든다.
    }
}
