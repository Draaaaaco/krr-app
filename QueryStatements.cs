using System;
namespace KRR_Proj
{
    public interface IQueryStatement : IStatement
    {
        bool QueryModel(IStructure s);

        public static IQueryStatement Parse(string[] parts)
        {
            string content = String.Join(' ', parts);


            if (parts[1].ToLower() == "suffices")
            {

                List<string> actionNames = new List<string>();

                return new TimeRealizationQueryStatement(content, int.Parse(parts[0]), parts[2..]);
            }
            else
            {
                int sep_idx=-1;
                for(int i=0; i < parts.Length; ++i)
                {
                    if (parts[i] == "holds")
                    {
                        if (parts[i + 1] != "after") throw new Exception($"Invalid Query Statement {content}");
                        sep_idx = i;
                        break;
                    } 
                }
                if (sep_idx == -1) throw new Exception($"Invalid Query Statement {content}");

                List<(string fluent, bool value)> literals = new List<(string fluent, bool value)>();
                for(int i=0; i<sep_idx; ++i)
                {
                    literals.Add(FluentExpr.Parse(parts[i]));
                }
                return new ConditionQueryStatement(content, literals, parts[(sep_idx + 2)..]);
            }
            
        }
    }

    public class TimeRealizationQueryStatement: IQueryStatement
    {
        public string Content { get; }
        public int TimeLimit { get; }
        public string[]? ActionNames { get; }

        public TimeRealizationQueryStatement(string content, int timeLimit, string[]? actionNames)
        {
            Content = content;
            TimeLimit = timeLimit;
            ActionNames = actionNames;
        }

        public bool QueryModel(IStructure s)
        {
            List<IAction> actions = s.ActionSequence(ActionNames);
            var (state, time) = IAction.TransitionFunc(s.InitialState, actions.ToArray());

            return time <= TimeLimit;
        }
    }

    public class ConditionQueryStatement: IQueryStatement
    {
        public string Content { get; }

        public List<(string fluent, bool value)> Literals { get; }
        public string[]? ActionNames { get; }

        public ConditionQueryStatement(string content, List<(string fluent, bool value)> literals, string[]? actionNames)
        {
            Content = content;
            Literals = literals;
            ActionNames = actionNames;
        }

        public bool QueryModel(IStructure s)
        {
            List<IAction> actions = s.ActionSequence(ActionNames);
            var (state, time) = IAction.TransitionFunc(s.InitialState, actions.ToArray());
            foreach(var (fluent, value) in Literals)
            {
                if (state[fluent] != value) return false;
            }

            return true;
        }
    }
}

