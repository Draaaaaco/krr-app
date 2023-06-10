using System;
using System.Collections.Generic;

namespace KRR_Proj
{
	public class ModelBuilder
	{
        public Dictionary<string, IAction> ActionDict = new Dictionary<string, IAction>();
        public HashSet<string> Fluents = new HashSet<string>();
        public Dictionary<string, bool> FluentInitialStateDict = new Dictionary<string, bool>();
        List<IDescriptionStatement> descStatements = new List<IDescriptionStatement>();

        public IEnumerable<IDescriptionStatement> Statements { get { foreach (var s in descStatements) yield return s; } }

        List<ValueStatement> ValueConstraints = new List<ValueStatement>();

        List<IStructure> Models = new List<IStructure>();

        public ModelBuilder()
		{
        }

        public List<IAction> Actions { get { return ActionDict.Values.ToList(); } }

        public void AddDescStatement(IDescriptionStatement s)
        {
            foreach(var actionName in s.RequireActionNames)
            {
                ActionDict[actionName] = IAction.MakeAction(actionName);
            }

            foreach(var fluentName in s.RequireFluentNames)
            {
                Fluents.Add(fluentName);
            }

            descStatements.Add(s);
        }

        public List<IStructure> Build()
        {
            foreach(IDescriptionStatement s in descStatements)
            {
                s.BuildModel(this);
            }

            int i = 0;
            foreach(var init_state in IterateInitStates())
            {
                Console.WriteLine($"InitState {i}: {init_state}");
                try
                {
                    var builder = new StructureBuilder(init_state, Actions, ValueConstraints);
                    var model = builder.Build();
                    if (model != null) Models.Add(model);
                    Console.WriteLine($"A Model of the Language");
                }
                catch(NotModelOfLangException e)
                {
                    Console.WriteLine($"NOT A Model of the Language");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine($"Structure Not Valid");
                }

                ++i;

                
            }

            return Models;
        }

        public void BuildActionTimeStatement(ActionTimeStatement s)
        {
            ActionDict[s.ActionName] = ActionDict[s.ActionName].SetTime(s.Time);
        }
        public void BuildValueStatement(ValueStatement s)
        {

            if(s.ActionNames.Count == 0)
            {
                FluentInitialStateDict[s.result.fluent] = s.result.value;
            }
            else
            {
                ValueConstraints.Add(s);
            }
        }
        public void BuildEffectStatement(EffectStatement s)
        {
            ActionDict[s.ActionName] = ActionDict[s.ActionName].AddEffect(s.Result, s.Conditions);
        }

        IEnumerable<IState> IterateInitStates()
        {
            List<string> total_fluents = new List<string>();
            List<bool> total_vals = new List<bool>();
            foreach(var item in FluentInitialStateDict)
            {
                total_fluents.Add(item.Key);
                total_vals.Add(item.Value);
            }
            int start_index = total_vals.Count;
            foreach(var fluent in Fluents)
            {
                if(!FluentInitialStateDict.ContainsKey(fluent))
                {
                    total_fluents.Add(fluent);
                    total_vals.Add(false);
                }
            }

            foreach (var v in Utils.IterateProductOfList(total_vals, start_index, new List<bool> { false, true }))
            {
                yield return new StateVector(total_fluents, v);
            }
        }


    }
}

