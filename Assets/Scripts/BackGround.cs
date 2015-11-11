using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour
{
    Vector3 OriginPos; // 원거리 배경의 시작 위치
    float Ratio = 74f / 94f; // (근거리 배경의 거리 - 원거리 배경의 거리) / 근거리 배경의 거리

	void Start()
	{
        OriginPos = transform.position; // 원리 좌표를 저장해둔다.
	}
	
	void Update()
	{
        Vector3 pos = Camera.main.transform.position; // 카메라의 좌표를 가져온다.
        
        // 원래 좌표에서 카메라 좌표에 원거리 배경과 근거리 배경의 비율을 곱해 더해준다.
        transform.position = new Vector3(OriginPos.x + pos.x * Ratio, OriginPos.y + pos.y * Ratio, OriginPos.z);
	}
}
