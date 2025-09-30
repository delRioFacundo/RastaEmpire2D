public class StateMachine {

	IState currentState;

	public void ChangeState(IState newState){
		if (currentState != null)
			currentState.ExitState ();

		currentState = newState;
		currentState.EnterState ();
	}

	public void Update(){
		if (currentState != null)
			currentState.UpdateState ();
	}

	public void FixedUpdate(){
		if (currentState != null)
			currentState.FixedUpdateState ();
	}

	public void LateUpdate(){
		if (currentState != null)
			currentState.LateUpdateState ();
	}
}
