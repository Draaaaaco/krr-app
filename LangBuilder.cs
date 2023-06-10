using System;
namespace KRR_Proj
{
	public interface ILanguage
	{
		IEnumerable<IDescriptionStatement> Statements { get; }
        void AddStatement(IDescriptionStatement s);
		IEnumerable<IStructure> MakeModels();

    }

	public class Language: ILanguage
    {
		//List<IDescriptionStatement> statements = new List<IDescriptionStatement>();
		ModelBuilder modelBuilder = new ModelBuilder();

        public IEnumerable<IDescriptionStatement> Statements
		{
			get
			{
				return modelBuilder.Statements;
            }
		}

        public void AddStatement(IDescriptionStatement s)
		{
			//statements.Add(s);
			modelBuilder.AddDescStatement(s);
        }

		public IEnumerable<IStructure> MakeModels()
		{
			//var models = modelBuilder.Build();
			//int i = 0;
			//foreach(var model in models)
			//{
			//	Console.WriteLine($"Model {i}");
			//	//model.InitialState
			//}

            return modelBuilder.Build();
        }
	}
}

