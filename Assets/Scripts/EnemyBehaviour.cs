using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    PlayerController player;
    BotController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<BotController>();
    }

    void Start()
    {
        player = PlayerController.Instance;
    }

    void Update()
    {
        FollowPlayer(3f);
    }

    public void FollowPlayer(float safeDistance = 2f)
    {
        Vector3 playerPos = player.transform.position;

        Vector3 dir = playerPos - transform.position;
        float singleStep = controller.rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, singleStep, 0f));

        if (Vector3.Distance(playerPos, transform.position) > safeDistance)
        {
            //Debug.Log(Vector3.Distance(playerPos, transform.position));
            controller.Move(1f);
        }
        else
        {
            controller.Attack();
        }
    }
}