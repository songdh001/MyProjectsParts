using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Animator �Ķ���� �̸��� �̸� �ؽ÷� ��ȯ�� ĳ�� (���� ����ȭ)
    private static readonly int IsUsed = Animator.StringToHash("IsUsed");

    protected Animator animator;
    protected virtual void Awake()
    {
        // �ִϸ����� ������Ʈ�� �ڽĿ��� ������
        animator = GetComponentInChildren<Animator>();
    }

    public void PortalUsed()
    {
        // ��Ż �۵�
        animator.SetBool(IsUsed, true);
    }

    public void PortalUnUsed()
    {
        // ��Ż �ٽ� �����̵���
        animator.SetBool(IsUsed, false);
    }

    public Portal targetPortal; // ����� ��Ż A�� B�� �ִٸ� �� �ٿ��� �ٿ��� ��
    public float teleportCooldown = 1f; //��Ÿ�� �־���� �� �ƴϸ� ��Ż�� ������ �������� �Դٰ�����

    private bool canTeleport = true; //���� ������ ���¸� bool ������ ǥ��

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTeleport) return; //��Ż ��Ÿ�� ���̸� ����

        if (collision.CompareTag("Player") || collision.CompareTag("Cube"))//ť�곪 ĳ���͸� ����
        {
            //�ڷ���Ʈ�� ���� ��ü�� �ڷ���Ʈ ��Ű�� �ڷ�ƾ
            //������Ʈ ���� �������� �ڷ�ƾ�� ���� ������ �� ���� �ݺ��ϰ� �� �ܿ��� ������� �ʾƼ� �ڿ������� �����ϴٰ� ��
            StartCoroutine(Teleport(collision));
            PortalUsed();
        }
    }

    private IEnumerator Teleport(Collider2D obj)
    {
        // ��Ż ������ �ٽ� �� ���� ��� ��Ȱ��ȭ ��Ű��
        canTeleport = false;
        //�ݴ��� ��Ż�� �� ���� ��� ��Ȱ��ȭ ��Ű��
        targetPortal.canTeleport = false;

        // ���� ��ü �ٷ� �����̵� ��Ű��
        obj.transform.position = targetPortal.transform.position;

        // ��� ���� ó�� (���� ���� ����)
        yield return new WaitForSeconds(teleportCooldown);

        //�ð� ������ �ٽ� Ȱ��ȭ ����
        canTeleport = true;
        targetPortal.canTeleport = true;
        PortalUnUsed();
    }
}
