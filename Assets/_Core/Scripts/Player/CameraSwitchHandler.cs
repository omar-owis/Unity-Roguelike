using Cinemachine;
using UnityEngine;
using DungeonMan.Player.StateMachine;

public class CameraSwitchHandler : MonoBehaviour
{
    [SerializeField] InputManager _inputManager;
    [SerializeField] CinemachineFreeLook _openWorldCam;
    [SerializeField] CinemachineFreeLook _combatCam;
    PlayerStateMachine _playerStateMachine;

    private void OnEnable()
    {
        _inputManager.inventoryEvent += ToggleCameraInput;
    }

    private void Start()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        SwitchToOpenWorldCam();
    }

    private void OnDisable()
    {
        _inputManager.inventoryEvent -= ToggleCameraInput;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(_openWorldCam.Priority == 10)
            {
                SwitchToCombatCam();
            }
            else
            {
                SwitchToOpenWorldCam();
            }
        }
    }

    void SwitchToOpenWorldCam()
    {
        _openWorldCam.Priority = 10;
        _combatCam.Priority = 0;
        _playerStateMachine.OpenWorldCam = true;
    }

    void SwitchToCombatCam()
    {
        _openWorldCam.Priority = 0;
        _combatCam.Priority = 10;
        _playerStateMachine.OpenWorldCam = false;
    }

    void ToggleCameraInput()
    {
        _openWorldCam.GetComponent<CinemachineInputProvider>().enabled = !_openWorldCam.GetComponent<CinemachineInputProvider>().enabled;
    }
}
