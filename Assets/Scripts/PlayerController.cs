using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BotController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    BotController controller;
    [SerializeField] new CameraController camera;

    [Header("Attack")]
    [Tooltip("X = duration \nY = magnitude \nZ = Frequency")] [SerializeField] Vector3 attackShake;

    public float health { get { return controller.health; } }
    public float maxHealth { get { return controller.maxHealth; } }    

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        controller = GetComponent<BotController>();
    }

    void Start()
    {
        controller.onAttack += () => camera.Shake(attackShake.x, attackShake.y, attackShake.z);
    }

    void Update()
    {
        controller.Move(Input.GetAxis("Vertical"));
        controller.Rotate(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            controller.Attack();  
        }

        //controller.ApplyGravity();
        //controller.ApplyMovement();   
    } 
}