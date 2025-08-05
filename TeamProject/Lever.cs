using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //        ���� ����
    // ������ �۵���Ű�� ���� ������Ʈ cs�� IActivatable�� ���Դϴ�.
    // ex : class InteractiveWall : MonoBehaviour, IActivatable
    // �ش� ���Ͽ� public void Activate()�� public void Deactivate()�� ���� ����ġ ����� �۵��� ������ �߰��մϴ�.
    // ������ ���� �� �÷��̾ ������ �����Ͽ� "E" Ű�� ���� ��� ������ �����Ͽ� Activate()�� ����ִ� ������ ����˴ϴ�.




    //�� ��ư�� ������ ������Ʈ�� ����Ʈ�� �����ؼ� �� ���� ���� �� �۵� �����ϵ��� ����

    public List<GameObject> targetObjects;

    //��ư���� ������ ������Ʈ���� IActivatable �������̽��� ���ؼ� �۵��ϴ� �̰͵� ����Ʈ�� ����
    private List<IActivatable> activatables = new List<IActivatable>();

    // Animator �Ķ���� �̸��� �̸� �ؽ÷� ��ȯ�� ĳ�� (���� ����ȭ)
    private static readonly int IsSwitch = Animator.StringToHash("IsSwitch");

    //������ on/off �Ǿ��ִ��� bool ������ ����
    private bool isActivated = false;

    //��ó�� �÷��̾ �־�� �۵��� �� �ֵ��� bool ������ Ȯ��
    private bool playerInRange = false;
    private bool playerFInRange = false;
    private bool playerWInRange = false;



    protected Animator animator;


    protected virtual void Awake()
    {
        // �ִϸ����� ������Ʈ�� �ڽĿ��� ������
        animator = GetComponentInChildren<Animator>();
    }

    public void LeverSwitchOn()
    {
        // ���� ���� �ִϸ��̼� ����
        animator.SetBool(IsSwitch, true);
    }

    public void LeverSwitchOff()
    {
        // ���� ���� �ִϸ��̼� ����
        animator.SetBool(IsSwitch, false);
    }


    // Start is called before the first frame update
    // �ܵ����� ����Ǵ��� üũ
    // ������ �� ������ ������Ʈ�� �� IActivatable�� �ִ� ����� ã�� ����Ʈ�� ����
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
        if (playerFInRange && Input.GetKeyDown(KeyCode.E))//�÷��̾�F�� ��ó�� �ְ� EŰ�� ������ ����
        {
            Debug.Log("�÷��̾�F�� ���� �۵�");
            isActivated = !isActivated;

            foreach (var a in activatables)
            {
                if (isActivated) a.Activate();
                else a.Deactivate();
            }
            if (isActivated)
            {
                //�۵� �Ǿ��ٸ� �ִϸ��̼� ����
                LeverSwitchOn();
            }
            else
            {
                //�ٽ� ������ ������ �ִϸ��̼� ����
                LeverSwitchOff();
            }

        }

        if (playerWInRange && Input.GetKeyDown(KeyCode.KeypadEnter))//�÷��̾�W�� ��ó�� �ְ� ����Ű�� ������ ����
        {
            Debug.Log("�÷��̾�W�� ���� �۵�");
            isActivated = !isActivated;

            foreach (var a in activatables)
            {
                if (isActivated) a.Activate();
                else a.Deactivate();
            }
            if (isActivated)
            {
                //�۵� �Ǿ��ٸ� �ִϸ��̼� ����
                LeverSwitchOn();
            }
            else
            {
                //�ٽ� ������ ������ �ִϸ��̼� ����
                LeverSwitchOff();
            }

        }

    }


    //Player �±� �޸� �갡 ��ó�� �ִ��� ������ Ȯ��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerF"))
            {
                Debug.Log("�÷��̾�F ����");
                playerFInRange = true;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerW"))
            {
                Debug.Log("�÷��̾�W ����");
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
            Debug.Log("�÷��̾�F ����");
            playerFInRange = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerW"))
        {
            Debug.Log("�÷��̾�W ����");
            playerWInRange = false;
        }
    }
}
