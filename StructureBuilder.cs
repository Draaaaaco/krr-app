using System;
namespace KRR_Proj
{
    public class StructureBuilder
    {
        List<IState> StateList = new List<IState>();
        List<List<int>> transition_mat;

        IState InitialState { get; }
        List<IAction> Actions { get; }
        List<ValueStatement> Constraints { get; }

        public IAction FindAction(string name)
        {
            var res = Actions.Find(x => x.Name == name);
            if (res == null) throw new Exception($"Action Name {name} Not Found");
            return res;
        }

        public StructureBuilder(IState initial_state, IEnumerable<IAction> actions, IEnumerable<ValueStatement>? constraints)
        {
            //StateList.Add(initial_state);
            InitialState = initial_state;
            Actions = new List<IAction>(actions);
            Constraints = new List<ValueStatement>();
            if (constraints != null)
            {
                foreach (var s in constraints) Constraints.Add(s);
            }

            transition_mat = new List<List<int>>();
        }

        void AddState(IState state)
        {
            StateList.Add(state);
            transition_mat.Add(new List<int>(Enumerable.Repeat(-1, Actions.Count)));

            int now_state_idx = StateList.Count - 1;

            int action_idx = 0;
            foreach (var action in Actions)
            {
                var (nextState, time) = action.Act(state);

                int next_state_idx = nextState.IndexIn(StateList);

                if (next_state_idx == -1)
                {
                    AddState(nextState);
                    next_state_idx = nextState.IndexIn(StateList);
                }
                var previous_val = transition_mat[now_state_idx][action_idx];

                if (previous_val != -1 && previous_val != next_state_idx)
                {
                    throw new Exception("Confliction in Transition Mat");
                }
                transition_mat[now_state_idx][action_idx] = next_state_idx;

                ++action_idx;
            }

        }

        public IStructure Build()
        {
            //return null;
            //try
            //{
            AddState(InitialState);
            //IStructure TargetStructure = new BasicStructure(InitialState, Actions);

            foreach (var constraint in Constraints)
            {
                List<IAction> actions = new List<IAction>();
                constraint.ActionNames.ForEach(x => actions.Add(FindAction(x)));

                if (actions == null) continue;
                //var (to_state, total_time) = TargetStructure.TransitionFunc(InitialState, actions.ToArray());

                var (to_state, total_time) = IAction.TransitionFunc(InitialState, actions.ToArray());

                if (to_state[constraint.result.fluent] != constraint.result.value)
                {
                    throw new NotModelOfLangException($"Structure with InitialState {InitialState} is not Compatible with the rule {constraint.Content}");
                }
            }
            //IStructure TargetStructure = new BasicStructure(Actions, StateList, transition_mat, 0);

            return new BasicStructure(Actions, StateList, transition_mat, 0);
            //         }
            //catch
            //{
            //	return null;
            //}

        }
    }
}

