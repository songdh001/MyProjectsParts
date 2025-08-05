using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //        레버 사용법
    // 레버로 작동시키고 싶은 오브젝트 cs에 IActivatable를 붙입니다.
    // ex : class InteractiveWall : MonoBehaviour, IActivatable
    // 해당 파일에 public void Activate()와 public void Deactivate()를 통해 스위치 실행시 작동될 동작을 추가합니다.
    // 게임을 실행 후 플레이어가 레버에 접근하여 "E" 키를 누를 경우 레버가 동작하여 Activate()에 들어있는 내용이 실행됩니다.




    //이 버튼이 관리할 오브젝트를 리스트로 관리해서 한 번에 여러 개 작동 가능하도록 구현

    public List<GameObject> targetObjects;

    //버튼으로 관리할 오브젝트들은 IActivatable 인터페이스를 통해서 작동하니 이것도 리스트로 관리
    private List<IActivatable> activatables = new List<IActivatable>();

    // Animator 파라미터 이름을 미리 해시로 변환해 캐싱 (성능 최적화)
    private static readonly int IsSwitch = Animator.StringToHash("IsSwitch");

    //레버가 on/off 되어있는지 bool 값으로 관리
    private bool isActivated = false;

    //근처에 플레이어가 있어야 작동될 수 있도록 bool 값으로 확인
    private bool playerInRange = false;
    private bool playerFInRange = false;
    private bool playerWInRange = false;



    protected Animator animator;


    protected virtual void Awake()
    {
        // 애니메이터 컴포넌트를 자식에서 가져옴
        animator = GetComponentInChildren<Animator>();
    }

    public void LeverSwitchOn()
    {
        // 레버 동작 애니메이션 실행
        animator.SetBool(IsSwitch, true);
    }

    public void LeverSwitchOff()
    {
        // 레버 동작 애니메이션 실행
        animator.SetBool(IsSwitch, false);
    }


    // Start is called before the first frame update
    // 단독으로 실행되는지 체크
    // 시작할 때 연결한 오브젝트들 중 IActivatable가 있는 얘들을 찾아 리스트에 저장
    void Start()
    {
        foreach (var obj in targetObjects)
        {
            IActivatable a = obj.GetComponent<IActivatable>();
            if (a != null) activatables.Add(a);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerFInRange && Input.GetKeyDown(KeyCode.E))//플레이어F가 근처에 있고 E키를 눌러서 실행
        {
            Debug.Log("플레이어F가 레버 작동");
            isActivated = !isActivated;

            foreach (var a in activatables)
            {
                if (isActivated) a.Activate();
                else a.Deactivate();
            }
            if (isActivated)
            {
                //작동 되었다면 애니메이션 실행
                LeverSwitchOn();
            }
            else
            {
                //다시 레버를 누르면 애니메이션 실행
                LeverSwitchOff();
            }

        }

        if (playerWInRange && Input.GetKeyDown(KeyCode.KeypadEnter))//플레이어W가 근처에 있고 엔터키를 눌러서 실행
        {
            Debug.Log("플레이어W가 레버 작동");
            isActivated = !isActivated;

            foreach (var a in activatables)
            {
                if (isActivated) a.Activate();
                else a.Deactivate();
            }
            if (isActivated)
            {
                //작동 되었다면 애니메이션 실행
                LeverSwitchOn();
            }
            else
            {
                //다시 레버를 누르면 애니메이션 실행
                LeverSwitchOff();
            }

        }

    }


    //Player 태그 달린 얘가 근처에 있는지 없는지 확인
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerF"))
            {
                Debug.Log("플레이어F 닿음");
                playerFInRange = true;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerW"))
            {
                Debug.Log("플레이어W 닿음");
                playerWInRange = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerF"))
        {
            Debug.Log("플레이어F 떠남");
            playerFInRange = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerW"))
        {
            Debug.Log("플레이어W 떠남");
            playerWInRange = false;
        }
    }
}
