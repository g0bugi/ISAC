using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutAndDestroy : MonoBehaviour
{
    // �̹����� ������ ������� �� Image ������Ʈ
    private Image targetImage;

    // �̹����� ������ ������� �� �ɸ��� �ð�
    public float fadeOutDuration = 2.0f;

    // ������� �����ϱ� �� ��� �ð�
    public float initialDelay = 1.0f;
    public Playermove playerMovementAndRotation;

    void Start()
    {
        // �� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� Image ������Ʈ�� �����ɴϴ�.
        targetImage = GetComponent<Image>();

        // Image ������Ʈ�� �ִ��� Ȯ��
        if (targetImage == null)
        {
            Debug.LogError("������Ʈ�� Image ������Ʈ�� �����ϴ�. ��ũ��Ʈ�� �����ϰų� Image ������Ʈ�� �߰����ּ���.");
            return;
        }

        // ���� ���� �� �ٷ� �÷��̾��� �������� �����ϴ�.
        if (playerMovementAndRotation != null)
        {
            playerMovementAndRotation.enabled = false;
        }

        // �ڷ�ƾ�� �����Ͽ� ���̵�ƿ� ȿ���� �����մϴ�.
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        // 1. ���̵�ƿ� ���� �� ���
        // ������ �ð���ŭ ��ٸ��ϴ�.
        yield return new WaitForSeconds(initialDelay);

        float timer = 0f;
        Color initialColor = targetImage.color; // �̹����� ���� ���� (���İ� ����)

        // 2. ���̵�ƿ� ȿ�� ����
        // Ÿ�̸Ӱ� fadeOutDuration�� ������ ������ �ݺ�
        while (timer < fadeOutDuration)
        {
            // ��� �ð��� ��ü �ð��� ���� ����(0~1)�� ��ȯ
            float progress = timer / fadeOutDuration;

            // ���� ���İ��� 1(������)���� 0(���� ����)���� Lerp(���� ����)
            float currentAlpha = Mathf.Lerp(1f, 0f, progress);

            // �̹����� �� ���İ� ����
            targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentAlpha);

            // ���� �����ӱ��� ���
            timer += Time.deltaTime;
            yield return null;
        }

        // 3. ������ ����� �� ������Ʈ ����
        // ���̵�ƿ��� ���� �� Ȥ�ö� ������ �Ϻ��� 0�� �� �Ǿ��� ��츦 ����� 0���� ����
        targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        if (playerMovementAndRotation != null)
        {
            playerMovementAndRotation.StartGettingUpAnimation();
        }

        Debug.Log($"playerMovementAndRotation: {playerMovementAndRotation}");
        // ���������� ������Ʈ�� �ı��Ͽ� �޸𸮿��� ����
        Destroy(gameObject);
    }
}