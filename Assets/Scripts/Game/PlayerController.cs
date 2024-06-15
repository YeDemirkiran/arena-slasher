using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BotController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    public BotController controller { get; private set; }


    CameraController cameraController;
    GameObject deathMenu;

    [Header("Attack")]
    [Tooltip("X = duration \nY = magnitude \nZ = Frequency")] [SerializeField] Vector3 attackShake;


    public float health { get { return controller.health; } }
    public float maxHealth { get { return controller.maxHealth; } }    


    void Awake()
    {
        if (Instance)
        {
            Debug.Log("anan");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        controller = GetComponent<BotController>();
    }

    IEnumerator Start()
    {
        while (CameraController.Instance == null) { yield return null; }
        cameraController = CameraController.Instance;

        while (UIManager.Instance == null) { yield return null; }

        UIManager.Instance.healthBar.controller = controller;
        UIManager.Instance.parryBar.controller = controller;

        deathMenu = UIManager.Instance.deathMenu.gameObject;

        controller.stunIcon = UIManager.Instance.stunIcon;

        controller.onAttack += () => cameraController.Shake(attackShake.x, attackShake.y, attackShake.z);
        controller.onDeath += () => 
        {
            GameManager.Instance.PauseGamePure();
            deathMenu.SetActive(true);
        };

        controller.destroyOnDeath = false;
    }

    void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.Running) { return; }

        controller.Move(Input.GetAxis("Vertical"));
        controller.Rotate(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            controller.Attack();  
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            controller.Parry();
        }   
    }

    public void ResetPlayer()
    {
        deathMenu.SetActive(false);
        controller.ResetBot();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;

            controller.onAttack -= () => cameraController.Shake(attackShake.x, attackShake.y, attackShake.z);
            controller.onDeath -= () =>
            {
                GameManager.Instance.PauseGamePure();
            };
        }
    }
}