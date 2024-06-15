using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] HealthBar _healthBar;
    public HealthBar healthBar { get { return _healthBar; } }

    [SerializeField] ParryBar _parryBar;
    public ParryBar parryBar { get { return _parryBar; } }

    [SerializeField] DeathMenu _deathMenu;
    public DeathMenu deathMenu { get { return _deathMenu; } }

    [SerializeField] RotatingObject _stunIcon;
    public RotatingObject stunIcon { get { return _stunIcon; } }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}