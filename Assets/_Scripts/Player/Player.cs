using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponController weapon { get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }
    public PlayerInteraction interaction { get; private set; }

    public Player_Health health { get; private set; }

    public Ragdoll ragdoll { get; private set; }

    public Animator anim { get; private set; }

    private void Awake()
    {
        controls = new PlayerControls();

        anim = GetComponentInChildren<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponController>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
        interaction = GetComponent<PlayerInteraction>();
        health = GetComponent<Player_Health>();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Character.UIMissionToolTipSwitch.performed += ctx => UI.instance.inGameUI.SwitchMissionToolTip();
        controls.Character.UIPause.performed += ctx => UI.instance.PauseSwitch();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
