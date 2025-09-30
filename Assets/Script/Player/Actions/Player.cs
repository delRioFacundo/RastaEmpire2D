using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Player : MonoBehaviour
{
    [SerializeField] string currentState;
    [SerializeField] string previusState;

    public string CurrentState { get { return currentState; } set { currentState = value; } }
    public string PreviusState { get { return previusState; } set { previusState = value; } }

    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public PlayerMotor motor;
    [HideInInspector] public PlayerInputReader input;
    [HideInInspector] public PlayerInteractDetector interactDetector;
    [HideInInspector] public Player_Animation player_Animation;
    [HideInInspector] public CursorManager cursorManager;
    public IInteractable currentInteractable;

    public PlantHarvestUI plantHarvest;

    public PlantInteractable CurrentPlant { get; set; }


    void Awake()
    {
        stateMachine = new StateMachine();
        motor = GetComponent<PlayerMotor>();
        input = GetComponent<PlayerInputReader>();
        interactDetector = GetComponent<PlayerInteractDetector>();
        player_Animation = GetComponentInChildren<Player_Animation>();
        cursorManager = GetComponent<CursorManager>();

        SubscribeToEvents();

    }
    void Start()
    {
        stateMachine.ChangeState(new State_Idle(this));
    }

    void Update()
    {
        stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Debugging: Force change to Harvest state
            stateMachine.ChangeState(new State_Harvest(this));
        }

    }
    void UnsubscribeToEvent()
    {
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.ShaderSmoked, ShaderSmoked);
        EventsManagerSpaar.UnsubscribeToEvent(EventTypeSpaar.ChangeStatePlayer, changeStatePlayer);
    }

    void SubscribeToEvents()
    {
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.ShaderSmoked, ShaderSmoked);
        EventsManagerSpaar.SubscribeToEvent(EventTypeSpaar.ChangeStatePlayer, changeStatePlayer);
    }

    private void changeStatePlayer(object[] parameterContainer)
    {
        var _state = (string)parameterContainer[0];

        switch (_state)
        {
            case "Idle":
                stateMachine.ChangeState(new State_Idle(this));
                break;
            case "Move":
                stateMachine.ChangeState(new State_Move(this));
                break;
            case "Interact":
                stateMachine.ChangeState(new State_Interact(this));
                break;
            case "Harvest":
                stateMachine.ChangeState(new State_Harvest(this));
                break;
            case "Planting":
                stateMachine.ChangeState(new State_Planting(this)); // por ahora no hay estado planting,
                break;
            case "Water":
                stateMachine.ChangeState(new State_Water(this)); // por ahora no hay estado planting,
                break;
            /* case "Fertilizing":
                stateMachine.ChangeState(new State_Fertilizing(this)); // por ahora no hay estado fertilizing,
                break; */
            default:
                Debug.LogWarning($"[Player] Estado no reconocido: {_state}");
                break;
        }
    }


    private void ShaderSmoked(object[] parameterContainer)
    {
        if (!(bool)parameterContainer[0])
            stateMachine.ChangeState(new State_Idle(this));
    }

}
