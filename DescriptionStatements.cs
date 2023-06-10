using System;
namespace KRR_Proj
{
	public interface IStatement
	{
		string Content { get; }

    }



	public interface IDescriptionStatement: IStatement
	{
        List<string> RequireActionNames { get; }
        List<string> RequireFluentNames { get; }
		void BuildModel(ModelBuilder modelBuilder);

        public static IDescriptionStatement Parse(string[] parts)
        {
            string content = String.Join(' ', parts);

            if (parts[0].ToLower() == "initially")
            {
                return new ValueStatement(content, FluentExpr.Parse(parts[1]), null);
            }
            else if (parts[1].ToLower() == "lasts")
            {
                return new ActionTimeStatement(String.Join(' ', parts), parts[0], int.Parse(parts[2]));
            }
            else if (parts[1].ToLower() == "causes")
            {
                List<(string name, bool value)> conditions = new List<(string name, bool value)>();
				for(int i=0; i < parts.Length; ++i)
				{
					Console.Write($"{parts[i]},");
				}
				Console.WriteLine();

                if (parts.Length > 4 && parts[3].ToLower() == "if")
                {
					//string[] fluent_exprs = (String.Join(null, parts[3..])).Remove(' ').Split(',');
					string[] fluent_exprs = parts[4..];
                    for (int i = 0; i < fluent_exprs.Length; ++i)
                    {
                        conditions.Add(FluentExpr.Parse(fluent_exprs[i]));
                    }
                }
                return new EffectStatement(content, parts[0], FluentExpr.Parse(parts[2]), conditions);
            }
            else if (parts[1].ToLower() == "after" && parts.Length > 2)
            {
                string[] actions = String.Join(null, parts[2..]).Replace(" ", null).Split(',');
                return new ValueStatement(content, FluentExpr.Parse(parts[0]), new List<string>(actions));
            }

            throw new Exception("Invalid Expression");
        }
    }

	public class ActionTimeStatement: IDescriptionStatement
	{
		public string Content { get; }
		public string ActionName { get; }
		public int Time { get; }

		public List<string> RequireActionNames { get { return new List<string>() { ActionName }; } }
		public List<string> RequireFluentNames { get { return new List<string>(); } }

		public ActionTimeStatement(string content, string actionName, int time)
		{
			Content = content;
			ActionName = actionName;
			Time = time;
		}

		public void BuildModel(ModelBuilder b)
		{
			b.BuildActionTimeStatement(this);
        }
    }

	public class ValueStatement: IDescriptionStatement
	{
        public string Content { get; }
        public (string fluent, bool value) result { get; }
		public List<string> ActionNames { get; }

        public List<string> RequireActionNames { get { return ActionNames; } }
        public List<string> RequireFluentNames { get { return new List<string>(){ result.fluent }; } }

		public void BuildModel(ModelBuilder b)
		{
			b.BuildValueStatement(this);
		}

		public ValueStatement(string content, (string fluent, bool value) result, List<string>? actionNames=null)
		{
			Content = content;
			this.result = result;
			if (actionNames == null) actionNames = new List<string>();
            ActionNames = actionNames;
		}
    }

	public class EffectStatement: IDescriptionStatement
	{
        public string Content { get; }

        public string ActionName { get; }
		public (string fluent, bool value) Result { get; }
		public List<(string fluent, bool value)> Conditions { get; }

        public List<string> RequireActionNames { get { return new List<string>() {ActionName}; } }
        public List<string> RequireFluentNames {
			get {
				var res = new List<string>() { Result.fluent };
				foreach(var f in Conditions)
				{
					res.Add(f.fluent);
				}
				return res;
			}
		}

        public void BuildModel(ModelBuilder b)
        {
            b.BuildEffectStatement(this);
        }

		public EffectStatement(string content, string actionName, (string fluent, bool value) result, List<(string fluent, bool value)> conditions)
		{
			Content = content;
			ActionName = actionName;
			Result = result;
			Conditions = conditions;
		}
    }
}

