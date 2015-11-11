using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour
{
    Animator anim;

	void Start()
	{
        anim = GetComponent<Animator>(); // 마리오의 애니메이터 컴포넌트
        Physics2D.IgnoreLayerCollision(8, 10); // 코인의 부모와 물리적 충돌을 금지한다. isTrigger = true인 자식은 제외
		Physics2D.IgnoreLayerCollision(8, 11); // 적캐릭터의 부모와 물리적 충돌을 금지한다. isTrigger = true인 자식은 제외
	}
	
	void Update()
	{
	    if (Input.GetKey(KeyCode.LeftArrow)) // 왼쪽키가 눌린 경우
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // 이미지를 반전시킨다.
            if (rigidbody2D.velocity.x > -10f) rigidbody2D.AddRelativeForce(Vector2.right * -15f); // X축 가속값이 -10 이상인 경우 오른쪽으로 가속시킨다.
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
			if (rigidbody2D.velocity.x < 10f) rigidbody2D.AddRelativeForce(Vector2.right * 15f); // X축 가속값이 10 이하인 경우 왼쪽으로 가속시킨다.
        }
        Debug.DrawRay(transform.position, Vector3.down * 1.0f, Color.red); // 삭제해도 되는 코드. 레이저의 방향을 보여주기 위한 선 그리기

        if (Input.GetKeyDown(KeyCode.Space)) // 점프키를 눌렀을때
        {
            if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f, 1 << 9)) // 캐릭터의 위치에서 아래쪽으로 땅(layerMask = 9)이 있는지 체크한다.
            {
                audio.Play(); // 점프 사운드 연주
                rigidbody2D.AddForce(Vector2.up * 700f); // 위쪽으로 700f의 파워로 점프
                anim.SetTrigger("jump"); // 점프 애니메이션 실행
            }
        }

        if (rigidbody2D.velocity.magnitude >= 0.1f) // 가속력이 0.1f 보다 크면 키보드를 누르지 않고 있어도 움직이고 있다고 판단해 달리기 애니메이션을 한다.
        {
            anim.SetBool("run", true);
        }
        else // 0.1f 이하이면 멈춘거나 다름 없다고 판단해 대기 애니메이션을 한다.
        {
            anim.SetBool("run", false);
        }
    }
}
