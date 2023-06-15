using System;
using System.Text;

namespace KRR_Proj
{
    public interface IStructure
    {
        //IState[] States { get; }
        IState InitialState { get; }
        List<IAction> Actions { get; }

        List<IState> StateList { get; }
        List<List<int>> TransitionMat { get; }

        (IState to_state, int time) TransitionFunc(IState from_state, IAction[] actions, int t = 0)
        {
            if (actions == null)
            {
                return (from_state, t);
            }
            else
            {
                var (next_state, next_time) = actions[0].Act(from_state);
                return TransitionFunc(next_state, actions[1..], t + next_time);
            }
        }

        bool Query(IQueryStatement qs) { return qs.QueryModel(this); }

        List<IAction> ActionSequence(string[]? actionNames)
        {
            var result = new List<IAction>();

            if (actionNames == null || actionNames.Length == 0 || (actionNames.Length == 1 && actionNames[0] == "NULL")) return result;

            actionNames = String.Join(null, actionNames).Replace(" ", null).Split(',');
            for (int i=0; i<actionNames.Length; ++i)
            {
                var action = Actions.Find(x=>x.Name == actionNames[i]);
                if (action == null) throw new Exception($"Action Name {actionNames[i]} Not Found in Structure");
                result.Add(action);
            }

            return result;
        }

        public string? ToString();
    }

    public class BasicStructure : IStructure
    {
        //public IState InitialState { get; }
        public IState InitialState
        {
            get { return StateList[InitialStateIndex]; }
        }

        public int InitialStateIndex { get; }
        public List<IAction> Actions { get; }
        public List<IState> StateList { get; }
        public List<List<int>> TransitionMat { get; }

        public override string? ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{\n  \"Actions\":\n  [\n");
            foreach (var action in Actions)
            {
                sb.Append($"    \"{action.Name.Replace("\"", "\\\"")}\",\n");
                // sb.Append("");
            }
            sb.Remove(sb.Length-2,2);
            sb.Append("\n  ],\n");

            sb.Append("  \"States\":\n  [\n");
            foreach (var state in StateList)
            {
                sb.Append($"    \"{state.ToString().Replace("\"", "\\\"")}\",\n");
                // sb.Append(',');
            }
            sb.Remove(sb.Length-2,2);
            sb.Append("\n  ],\n");

            sb.Append("  \"TransitionMat\":\n  [\n");
            for (int i = 0; i < TransitionMat.Count; ++i)
            {
                sb.Append("    [");
                for (int j = 0; j < TransitionMat[0].Count; ++j)
                {
                    sb.Append(' ');
                    sb.Append(TransitionMat[i][j]);
                    if (j != TransitionMat[0].Count - 1)
                        sb.Append(',');
                }
                sb.Append(" ],\n");
            }
            sb.Remove(sb.Length-2,2);
            sb.Append("\n  ]\n}");
            Console.WriteLine(sb.ToString());

            return sb.ToString();
        }

        public BasicStructure(List<IAction> actions, List<IState> stateList, List<List<int>> transitionMat, int initState)
        {
            //InitialState = init_state;
            InitialStateIndex = initState;
            Actions = actions;

            StateList = stateList;
            TransitionMat = transitionMat;
        }
    }


}

