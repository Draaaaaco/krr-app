using System;
using System.Text;

namespace KRR_Proj
{
	public class FluentExpr
	{
		public static (string fluent, bool value) Parse(string v)
		{
			return new FluentExpr(v).Value;
		}
        public (string fluent, bool value) Value { get; }
		public FluentExpr(string v)
		{
			if (v[0] == '~')
			{
				Value = (v[1..], false);
			}
			else
			{
				Value = (v, true);
			}
		}

        //bool Value(StateVector s) { return s[Name]; }
    }


	public interface IState
	{
		int ID { get; }
        bool this[string name] { get; }
		public bool Equal(IState b);

		IEnumerable<string> FluentNames { get; }

		IState FlipFluents(IEnumerable<string> fluents);
		IState SetFluents(IEnumerable<(string fluent, bool value)>? fluentsToSet);


        public int IndexIn(IEnumerable<IState> states)
		{
			int i = 0;
            foreach (IState s in states)
            {
                if (Equal(s))
				{
					return i;
				}
				++i;
            }

			return -1;
        }

		public bool IsIn(IEnumerable<IState> states)
		{
			return IndexIn(states) != -1;
		}

        public bool TestConditions(IEnumerable<(string fluent, bool value)>? conditions)
        {
            if (conditions == null) return true;
            foreach (var cond in conditions)
            {
                if (this[cond.fluent] != cond.value)
                {
                    return false;
                }
            }

            return true;
        }

		public string? ToString();
    }

	public class StateVector: IState
	{
		public int ID { get; }
		//List<IFluent> Fluents;
		SortedDictionary<string, bool> vals;

        public IEnumerable<string> FluentNames
		{
			get
			{
				foreach(var key in vals.Keys)
				{
					yield return key;
				}
			}
		}

        public override string? ToString()
        {
            List<string> result = new List<string>();
            foreach (var fluent in FluentNames)
            {
				if (this[fluent]) result.Add(fluent);
				else result.Add("~" + fluent);
                //result.Add($"{fluent}:{this[fluent]}");
            }
            return '[' + String.Join(',', result) + ']';
        }

        public bool this[string name]
		{
			get { return vals[name]; }
		}

		public bool Equal(IState b)
		{
			//return vals.SequenceEqual(b.vals);
			foreach(string fluent in vals.Keys)
			{
				if (this[fluent] != b[fluent]) return false;
			}
			return true;
		}

		public StateVector(List<string> fluents, List<bool>? vals_=null)
		{
			vals = new SortedDictionary<string, bool>();

			if (vals_ != null && vals_.Count != fluents.Count) throw new Exception("Dim Error in StateVector Initialization");

			for(int i=0; i<fluents.Count; ++i)
			{
				if (vals_ != null) vals[fluents[i]] = vals_[i];
				else vals[fluents[i]] = false;
            }
		}

		public StateVector(StateVector b)
		{
			vals = new SortedDictionary<string, bool>();
			foreach(var z in b.vals)
			{
				vals.Add(z.Key, z.Value);
			}
        }

        public IState FlipFluents(IEnumerable<string> fluents)
		{
			var res = new StateVector(this);
			foreach(string fluent in fluents)
			{
				res.vals[fluent] = !(res.vals[fluent]);
			}

			return res;
		}

		public IState SetFluents(IEnumerable<(string fluent, bool value)>? fluentsToSet)
		{
            var res = new StateVector(this);
			if(fluentsToSet != null)
			{
                foreach (var (fluent, val) in fluentsToSet)
                {
					res.vals[fluent] = val;
                }
            }

            return res;
        }

    }







}

