using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{
    //        ��ư ����
    //��ư���� �۵���Ű�� ���� ������Ʈ cs�� IActivatable�� ���Դϴ�.
    //ex : class InteractiveWall : MonoBehaviour, IActivatable
    //�ش� ���Ͽ� public void Activate()�� public void Deactivate()�� ���� ��ư�� �ö� ���� �� �۵��� ������ �߰��մϴ�.
    //������ ���� �� �÷��̾ ����ġ�� �ö� ���� ��� Activate()�� ����ִ� ������ ����˴ϴ�.
    // ����ġ���� �������� Deactivate()�� ����ִ� ������ ����˴ϴ�.





    //�� ��ư�� ������ ������Ʈ�� ����Ʈ�� �����ؼ� �� ���� ���� �� �۵� �����ϵ��� ����
    public List<GameObject> targetObjects;

    //��ư���� ������ ������Ʈ���� IActivatable �������̽��� ���ؼ� �۵��ϴ� �̰͵� ����Ʈ�� ����
    private List<IActivatable> activatables = new List<IActivatable>();

    // Animator �Ķ���� �̸��� �̸� �ؽ÷� ��ȯ�� ĳ�� (���� ����ȭ)
    private static readonly int IsSwitch = Animator.StringToHash("IsSwitch");

    protected Animator animator;

    private int playerCount = 0; // ���� ��ư ���� �ִ� �÷��̾� ��


    protected virtual void Awake()
    {
        // �ִϸ����� ������Ʈ�� �ڽĿ��� ������
        animator = GetComponentInChildren<Animator>();
    }


    public void GroundButtonSwitch()
    {
        // ���� ���� ���� ����       
        animator.SetBool(IsSwitch, true);
        
    }

    public void GroundButtonNoSwitch()
    {
        // ���� ���� ���� ����
        animator.SetBool(IsSwitch, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //������ Ÿ�� ������Ʈ���� �ݺ���
        foreach (var obj in targetObjects)
        {
            IActivatable a = obj.GetComponent<IActivatable>();
            if (a != null) activatables.Add(a);
        }
    }


    //�÷��̾�� �������� �� ��ȣ�ۿ� �� �� �ֵ���
    //Player�±׸� ���� ������Ʈ�� �۵� ����!!!!!

    //��ư�� �ö�
    private void OnTriggerEnter2D(Collider2D other)
    {
        //��ư�� ���� �ö󰡵� �������� �÷��̾ �ö��� ���� �۵��ǵ���
        GroundButtonSwitch();

        if (other.CompareTag("Player"))
        {
            playerCount++;

            // ó�� �ö�� ��츸 �۵�
            if (playerCount == 1)
            {
                foreach (var a in activatables)
                    a.Activate();
            }
        }
    }

    //��ư���� ������
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            playerCount = Mathf.Max(0, playerCount - 1); // ���� ����

            // ������ �÷��̾ ���� ���� �۵� ����
            if (playerCount == 0)
            {
                foreach (var a in activatables)
                    a.Deactivate();
                GroundButtonNoSwitch();
            }
        }
    }


}
