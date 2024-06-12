using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public Transform playerTransform;
    public PlayerController playerController;
    public float moveSpeed = 2f;

    private bool shouldRetreat = false;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player.GetComponent<Transform>();
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!shouldRetreat)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            StartCoroutine(RetreatAndResume());
        }
        if (other.CompareTag("PlayerBodyCollider"))
        {
            StartCoroutine(DealDamageAndDie());
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    IEnumerator RetreatAndResume()
    {
        shouldRetreat = true;
        float retreatTime = 3f; // Время отступления
        while (retreatTime > 0)
        {
            Vector3 retreatDirection = (transform.position - playerTransform.position).normalized;
            transform.position += retreatDirection * moveSpeed * Time.deltaTime;
            retreatTime -= Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        shouldRetreat = false;
    }

    IEnumerator DealDamageAndDie()
    {
        Destroy(gameObject);
        playerController.ReceivingDamage(25f);
        yield return null;
    }
}
