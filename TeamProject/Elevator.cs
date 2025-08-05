using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour, IActivatable
{
    //!!!! 스위치, 레버로 작동 가능하게 하려면 위에 IActivatable 이거 추가해야 함 !!!!!


    //엘리베이터 최고,최저점
    public Transform topPoint;
    public Transform bottomPoint;

    //엘리베이터 스피드
    public float speed = 2f;

    //작동하는지 어디로 가는지 체크
    private bool isActivated = false;
    private bool isMovingUp = false;

    //스위치나 레버 작동시켜서 활성화 했는지 체크
    private bool isTriggered = false;

    // 플레이어가 올라와 있는지 체크
    private bool playerOnElevator = false;

    [Header("스위치 의존 여부")]
    public bool useSwitchControl = true; // 에디터에서 선택 가능


    public void Activate()
    {
        if (useSwitchControl)
        {
            isTriggered = true;
        }

    }
    public void Deactivate()
    {
        if (useSwitchControl)
        {
            isTriggered = false;
        }

    }


    //플레이어와 접촉할 시(엘리베이터에 탔을 시) 활성화
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어나 큐브가 올라탈 시 확인
        if (collision.CompareTag("Player") || collision.CompareTag("Cube"))
        {
            Debug.Log("올라탐!");

            isActivated = true;
        }

        if (collision.CompareTag("Player"))
        {
            playerOnElevator = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerOnElevator = false;
        }
    }


    private void Start()
    {
        // 스위치 없이 시작하자마자 작동되도록 하고 싶을 경우
        if (!useSwitchControl)
        {
            isTriggered = true;
        }

    }


    // Update is called once per frame
    void Update()
    {
        //엘레베이터 움직여줌
        if (isActivated && isTriggered && playerOnElevator)
        {
            Debug.Log("이동중");

            MoveElevator();
        }

    }


    void MoveElevator()
    {
        //위로 움직인다면 탑포인트까지 이동, 아니면 바텀포인트까지 이동 디폴트는 바텀에서 탑으로 이동하는 거로 설정됨
        Vector3 target = isMovingUp ? topPoint.position : bottomPoint.position;

        //현재 위치에서 타겟 위치까지 일정 속도로 이동
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        //타겟 위치까지 도착하면 비활성화(다시 내렸다가 탈 경우 작동) + 이동 방향을 바꿔줌
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            Debug.Log("목적지 도착 방향 바꿈");
            isActivated = false;
            isMovingUp = !isMovingUp; // 왕복용 도착했다면 바꿔줘서 다른 쪽 방향으로 이동할 수 있도록 해줌
        }
    }


}
