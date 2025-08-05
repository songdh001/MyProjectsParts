using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{
    //        버튼 사용법
    //버튼으로 작동시키고 싶은 오브젝트 cs에 IActivatable를 붙입니다.
    //ex : class InteractiveWall : MonoBehaviour, IActivatable
    //해당 파일에 public void Activate()와 public void Deactivate()를 통해 버튼에 올라가 있을 시 작동될 동작을 추가합니다.
    //게임을 실행 후 플레이어가 스위치에 올라가 있을 경우 Activate()에 들어있는 내용이 실행됩니다.
    // 스위치에서 내려오면 Deactivate()에 들어있는 내용이 실행됩니다.





    //이 버튼이 관리할 오브젝트를 리스트로 관리해서 한 번에 여러 개 작동 가능하도록 구현
    public List<GameObject> targetObjects;

    //버튼으로 관리할 오브젝트들은 IActivatable 인터페이스를 통해서 작동하니 이것도 리스트로 관리
    private List<IActivatable> activatables = new List<IActivatable>();

    // Animator 파라미터 이름을 미리 해시로 변환해 캐싱 (성능 최적화)
    private static readonly int IsSwitch = Animator.StringToHash("IsSwitch");

    protected Animator animator;

    private int playerCount = 0; // 현재 버튼 위에 있는 플레이어 수


    protected virtual void Awake()
    {
        // 애니메이터 컴포넌트를 자식에서 가져옴
        animator = GetComponentInChildren<Animator>();
    }


    public void GroundButtonSwitch()
    {
        // 레버 동작 상태 진입       
        animator.SetBool(IsSwitch, true);
        
    }

    public void GroundButtonNoSwitch()
    {
        // 레버 비동작 상태 진입
        animator.SetBool(IsSwitch, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //각각의 타겟 오브젝트마다 반복문
        foreach (var obj in targetObjects)
        {
            IActivatable a = obj.GetComponent<IActivatable>();
            if (a != null) activatables.Add(a);
        }
    }


    //플레이어와 접촉했을 때 상호작용 할 수 있도록
    //Player태그를 가진 오브젝트만 작동 가능!!!!!

    //버튼에 올라감
    private void OnTriggerEnter2D(Collider2D other)
    {
        //버튼은 뭐가 올라가든 눌리지만 플레이어가 올라갔을 때만 작동되도록
        GroundButtonSwitch();

        if (other.CompareTag("Player"))
        {
            playerCount++;

            // 처음 올라온 경우만 작동
            if (playerCount == 1)
            {
                foreach (var a in activatables)
                    a.Activate();
            }
        }
    }

    //버튼에서 내려옴
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerCount = Mathf.Max(0, playerCount - 1); // 음수 방지

            // 마지막 플레이어가 나갈 때만 작동 해제
            if (playerCount == 0)
            {
                foreach (var a in activatables)
                    a.Deactivate();
                GroundButtonNoSwitch();
            }
        }
    }


}
