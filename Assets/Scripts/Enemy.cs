using UnityEngine;
using System.Collections;

public enum MOVE // 적캐릭터 이동을 나타내기 위한 열거형 변수
{
    STOP,
    LEFT,
    RIGHT,
}

public class Enemy : MonoBehaviour
{
    public Animator anim; // 에니메이터 컴포넌트를 연결해둔다.
    MOVE move = MOVE.STOP; // 기본값은 STOP을 지정해 멈춰있게 한다.
    public float Speed = 1.2f; // 적캐릭터의 이동 속도
    bool isDead = false; // 죽었는지 판단

	void Update()
	{
        if (!isDead) // 살아있으면
        {
            if (move == MOVE.STOP) // 이동 타입이 STOP인 경우 (최초 생성시 이 루틴을 타게 된다.)
            {
                if (Random.Range(0, 100) == 0) // 1%의 확률로
                {
                    move = Random.Range(0, 2) == 0 ? MOVE.LEFT : MOVE.RIGHT; // 0~1 사이의 난수를 발생시켜 좌우 중에 하나로 설정한다.
                }
            }
            else if (move == MOVE.LEFT)
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // 스케일을 원래대로 보이게 한다.
                transform.Translate(Vector3.left * Speed * Time.deltaTime); // 좌측으로 이동시킨다.

				// layerMask를 1 << 9로 지정한 것은 땅 타일의 레이어 값이 9이므로 땅만 체크하기 위한 것이다.
				// 캐릭터의 아래로 0.1f 거리까지 레이저를 쏴서 땅이 있는지 체크한다. 이 값이 false이면 공중에 떠 있는 것이므로 반대쪽으로 이동하게 한다.
				// 캐릭터의 좌측으로 0.5f 거리까지 레이저를 쏴서 땅이 있는지 체크한다. 이 값이 true이면 캐릭터 앞에 벽이 있는 것이므로 반대쪽으로 이동하게 한다.
                if (!Physics2D.Raycast(transform.position, Vector3.down, 0.1f, 1 << 9) || Physics2D.Raycast(transform.position, Vector3.left, 0.5f, 1 << 9))
                {
                    move = MOVE.RIGHT;
                }
                else if (Random.Range(0, 100) == 0) // 혹은 1%의 확률로 STOP 상태를 만든다. (잠깐 멈춰서 생각하게 하는듯한 연출)
                {
                    move = MOVE.STOP;
                }
            }
            else if (move == MOVE.RIGHT)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // X축 스케일을 -1f로 지정해 스프라이트를 좌우 반전시킨다.
                transform.Translate(Vector3.right * Speed * Time.deltaTime); // 우측으로 이동시킨다.

				// layerMask를 1 << 9로 지정한 것은 땅 타일의 레이어 값이 9이므로 땅만 체크하기 위한 것이다.
				// 캐릭터의 아래로 0.1f 거리까지 레이저를 쏴서 땅이 있는지 체크한다. 이 값이 false이면 공중에 떠 있는 것이므로 반대쪽으로 이동하게 한다.
				// 캐릭터의 우측으로 0.5f 거리까지 레이저를 쏴서 땅이 있는지 체크한다. 이 값이 true이면 캐릭터 앞에 벽이 있는 것이므로 반대쪽으로 이동하게 한다.
				if (!Physics2D.Raycast(transform.position, Vector3.down, 0.1f, 1 << 9) || Physics2D.Raycast(transform.position, Vector3.right, 0.5f, 1 << 9))
                {
                    move = MOVE.LEFT;
                }
				else if (Random.Range(0, 100) == 0) // 혹은 1%의 확률로 STOP 상태를 만든다. (잠깐 멈춰서 생각하게 하는듯한 연출)
                {
                    move = MOVE.STOP;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Equals("mario") && !isDead) // 마리오와 충돌한 경우
        {
            if (col.transform.position.y > transform.position.y) // 마리오가 적캐릭터보다 위쪽에 있는지 확인
            {
                float dist = Mathf.Abs(transform.position.x - col.transform.position.x); // 마리오와 적캐릭터의 거리를 구한다.

                if (dist <= 0.5f) // 서로의 거리가 0.5f 이내인 경우만 머리 위쪽에서 내려와 충돌했다고 판단
                {
                    isDead = true;
                    col.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 마리오의 가속 변수를 초기화
                    col.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 400f); // 적캐릭터와 충돌하는 순간 다시 위쪽으로 살짝 점프시킨다.
                    
                    // 트윈 함수를 이용해 적캐릭터의 알파값을 0.05초 동안 투명하게 만들되 6회 반복시켜, 사라졌다 나타났다를 반복하게 한다.
					// setLoopPingPong() 옵션은 알파값이 1 ~ 0으로 변하면 다시 0 ~ 1로 자동으로 되돌아가게 해준다.
                    LeanTween.alpha(gameObject, 0f, 0.05f).setDelay(1f).setLoopPingPong(6).setDestroyOnComplete(true);
                    anim.SetTrigger("dead");
                }
            }
        }
    }
}
