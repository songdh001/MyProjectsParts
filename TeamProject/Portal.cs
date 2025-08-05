using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Animator 파라미터 이름을 미리 해시로 변환해 캐싱 (성능 최적화)
    private static readonly int IsUsed = Animator.StringToHash("IsUsed");

    protected Animator animator;
    protected virtual void Awake()
    {
        // 애니메이터 컴포넌트를 자식에서 가져옴
        animator = GetComponentInChildren<Animator>();
    }

    public void PortalUsed()
    {
        // 포탈 작동
        animator.SetBool(IsUsed, true);
    }

    public void PortalUnUsed()
    {
        // 포탈 다시 움직이도록
        animator.SetBool(IsUsed, false);
    }

    public Portal targetPortal; // 연결된 포탈 A와 B가 있다면 둘 다에게 붙여야 함
    public float teleportCooldown = 1f; //쿨타임 넣어줘야 함 아니면 포탈에 갖혀서 무한으로 왔다갔다함

    private bool canTeleport = true; //텔포 가능한 상태를 bool 값으로 표현

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTeleport) return; //포탈 쿨타임 중이면 리턴

        if (collision.CompareTag("Player") || collision.CompareTag("Cube"))//큐브나 캐릭터만 적용
        {
            //텔레포트에 닿은 물체를 텔레포트 시키는 코루틴
            //업데이트 문에 넣지말고 코루틴을 쓰면 텔포를 쓸 때만 반복하고 그 외에는 사용하지 않아서 자원관리에 유용하다고 함
            StartCoroutine(Teleport(collision));
            PortalUsed();
        }
    }

    private IEnumerator Teleport(Collider2D obj)
    {
        // 포탈 썼으면 다시 못 쓰게 잠시 비활성화 시키기
        canTeleport = false;
        //반대쪽 포탈도 못 쓰게 잠시 비활성화 시키기
        targetPortal.canTeleport = false;

        // 닿은 물체 바로 순간이동 시키기
        obj.transform.position = targetPortal.transform.position;

        // 잠깐 무적 처리 (무한 텔포 방지)
        yield return new WaitForSeconds(teleportCooldown);

        //시간 지나면 다시 활성화 해줌
        canTeleport = true;
        targetPortal.canTeleport = true;
        PortalUnUsed();
    }
}
