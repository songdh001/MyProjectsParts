using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IActivatable
{

    public Transform startPoint;  // 시작 지점
    public Transform endPoint;    // 도착 지점
    public float moveSpeed = 2f;  // 움직이는 속도

    private bool isTriggered = false; // 레버/스위치 체크

    private Vector3 targetPosition;  //지금 플랫폼이 움직이려는 위치


    [Header("왕복운동 설정")]
    public bool repeatMoving = true; // 에디터에서 선택 가능

    [Header("스위치 의존 여부")]
    public bool useSwitchControl = true; // 에디터에서 선택 가능


    public void Activate()
    {
        isTriggered = true;
    }


    public void Deactivate()
    {
        if (!repeatMoving)
        {
            targetPosition = targetPosition == startPoint.position ? endPoint.position : startPoint.position;
        }
        isTriggered = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = endPoint.position; // 처음에는 엔드 포인트로 이동 나중엔 스타트 포인트로 바뀔 거임
                                            // 왔다갔다 할 수 있게

        if (!useSwitchControl) //만약 스위치에 의존하지 않는다면 자동으로 트루로 동작하도록 설정
            isTriggered = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTriggered) return; //스위치 등으로 활성화 되어야 이동

        // 목적 지점까지 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 목표 지점에 거의 도달했으면 방향 반전
        // (타겟 위치가 스타트 포인트면 바꿔주고 아니면 스타트 포인트로 목적지 설정)

        if (repeatMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = targetPosition == startPoint.position ? endPoint.position : startPoint.position;
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 같이 이동하게 자식으로 설정
            // (좌우로 이동해도 플레이어가 따라올 수 있도록)
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 떨어질 때 부모 해제
            other.transform.SetParent(null);
        }
    }

}
