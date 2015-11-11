using UnityEngine;
using System.Collections;

public class Question : MonoBehaviour
{
    public Transform CoinPool; // 코인 오브젝트풀의 부모 오브젝트

    void CreateCoin()
    {
        for (int i = 0; i < CoinPool.childCount; i++) // 코인 오브젝트풀의 자식을을 검색한다.
        {
            if (!CoinPool.GetChild(i).gameObject.activeInHierarchy) // 현재 비활성화된 코인을 찾는다.
            {
                CoinPool.GetChild(i).transform.position = transform.position + Vector3.up * 0.6f; // 퀘스천 블럭의 위쪽 0.6f 거리에 동전을 옮겨둔다.
                CoinPool.GetChild(i).gameObject.SetActive(true); // 동전을 활성화 시켜 화면에 보이게 한다.
                break; // 동전 1개가 활성화 됐으므로 루프에서 빠져나간다.
            }
        }
    }

	void OnCollisionEnter2D(Collision2D col)
	{
        if (!LeanTween.isTweening(gameObject)) // 트윈 명령이 작동되고 있으면 퀘스천 블럭이 움직이고 있는 것이므로 충돌을 금지한다.
        {
            if (col.gameObject.name.Equals("mario")) // 마리오와 충돌했다면
            {
                if (col.transform.position.y < transform.position.y) // 마리오가 블럭보다 아래쪽인지 확인
                {
                    float dist = Mathf.Abs(transform.position.x - col.transform.position.x); // 마리오와의 거리 계산

                    if (dist <= 0.5f) // 마리오가 0.5f 이내에 있으면 아래쪽에서 점프해서 충돌했다고 판단
                    {
                        LeanTween.moveY(gameObject, transform.position.y + 0.2f, 0.2f).setLoopPingPong(1); // 블럭을 위쪽으로 0.2f 만큼 이동했다가 다시 내려가게 한다.
                        CreateCoin(); // 코인을 생성한다.
                    }
                }
            }
        }
	}
}
