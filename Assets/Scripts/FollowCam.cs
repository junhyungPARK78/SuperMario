using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	public float followSpeed = 20;
    public Transform TargetPlayer = null;

	void Update()
    {
        if (TargetPlayer != null)
        {
            Vector3 start = transform.position;
            Vector3 end = Vector3.MoveTowards(start, TargetPlayer.position, followSpeed * Time.deltaTime); // 카메라의 위치가 마리오를 따라가게 해준다.

            end.z = start.z;
            if (end.x < 0f) end.x = 0f;
            if (end.x > 94f) end.x = 94f;
            if (end.y < 0f) end.y = 0f;

            transform.position = end;
        }
    }
}
