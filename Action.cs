using System;
namespace KRR_Proj
{
    public interface IAction
    {
        string Name { get; }
        int Time { get; }
        List<(string fluent, bool value)>? FluentsToSet(IState from);

        (IState to, int time) Act(IState from)
        {
            List<(string fluent, bool value)>? fluentsToSet = FluentsToSet(from);

            var result = from.SetFluents(fluentsToSet);

            int exec_time = 1;
            if (result.Equal(from)) exec_time = 0;
            else if (Time < 0) exec_time = 1;
            else exec_time = Time;

            return (result, exec_time);
        }

        public static (IState to_state, int time) TransitionFunc(IState from_state, IAction[] actions, int t = 0)
        {
            if (actions == null || actions.Length == 0)
            {
                return (from_state, t);
            }
            else
            {
                var (next_state, next_time) = actions[0].Act(from_state);
                return TransitionFunc(next_state, actions[1..], t + next_time);
            }
        }

        static IAction MakeAction(string name) { return new ZeroAction(name); }

        IAction AddEffect((string fluent, bool value) result, List<(string fluent, bool value)>? conditions = null)
        {
            return new ActionAddEffectDecorator(this, result, conditions);
        }

        IAction SetTime(int t)
        {
            return new ActionSetTimeDecorator(this, t);
        }
    }

    public class ZeroAction : IAction
    {
        public string Name { get; }
        public int Time { get { return -1; } }
        public List<(string fluent, bool value)>? FluentsToSet(IState from) { return null; }
        public ZeroAction(string name) { Name = name; }
    }

    public class ActionSetTimeDecorator : IAction
    {
        public string Name { get { return baseAction_.Name; } }


        IAction baseAction_;
        int t_;

        public ActionSetTimeDecorator(IAction input, int t)
        {
            baseAction_ = input;
            t_ = t;

            if (baseAction_.Time >= 0 && baseAction_.Time != t)
            {
                throw new Exception($"Action Time Confliction");
            }

        }

        public int Time
        {
            get { return t_; }
        }
        public List<(string fluent, bool value)>? FluentsToSet(IState from) { return baseAction_.FluentsToSet(from); }
    }

    public class ActionAddEffectDecorator : IAction
    {
        public string Name { get { return baseAction_.Name; } }

        IAction baseAction_;
        (string fluent, bool value) result_;
        List<(string fluent, bool value)>? conditions_;

        public ActionAddEffectDecorator(IAction baseAction, (string fluent, bool value) result, List<(string fluent, bool value)>? conditions)
        {
            baseAction_ = baseAction;
            result_ = result;
            conditions_ = conditions;
        }

        public int Time { get { return baseAction_.Time; } }
        public List<(string fluent, bool value)>? FluentsToSet(IState from)
        {
            var base_res = baseAction_.FluentsToSet(from);

            if (from.TestConditions(conditions_))
            {
                var res = new List<(string fluent, bool value)>();
                if (base_res != null)
                {
                    base_res.ForEach(FluentVal =>
                    {
                        if (FluentVal.fluent == result_.fluent && FluentVal.value != result_.value)
                        {
                            throw new Exception($"Action {this.ToString()} has Effect Confliction on State {from.ToString()}");
                        }
                        res.Add(FluentVal);
                    });
                }
                res.Add(result_);
                return res;

            }
            else return base_res;

        }
    }

}

