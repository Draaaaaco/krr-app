using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace KRR_Proj
{
    public enum States
    {
        MAIN, EXIT, CREATE, QUERY
    }

    public enum CommandStatus
    {
        SUCCESS,
        FAILED,
        INFO_LANG,
        INFO_MODEL,
        
        QUERY_TRUE,
        QUERY_FALSE,
        STATE_CHANGED,
        BUILD_SUCCESS
    }
    public static class CommandFactory
    {
        static Dictionary<string, Func<string[]?, ICommand?>> dict = new Dictionary<string, Func<string[]?, ICommand?>>();

        public static ICommand? Make(string? line, States state = States.MAIN)
        {
            if (line == null || line.Trim().Length == 0) return null;

            Regex regex = new Regex(@"[ ](?=(?:[^""]*""[^""]*"")*[^""]*$)", RegexOptions.Multiline);
            string[] s = regex.Split(line);

            switch (state)
            {
                case States.MAIN:
                    string command_name = s[0];
                    if (!dict.ContainsKey(command_name))
                    {
                        throw new Exception($"Command {command_name} Not Found");
                    }

                    //if (s.Length > 1) return dict[s[0]](s[1..]);
                    //else return dict[s[0]](null);
                    return dict[s[0]](s[1..]);

                case States.CREATE:
                    return dict["AddDesc"](s);
                case States.QUERY:
                    return dict["Query"](s);
            }
            return null;
        }

        static void RegisterCommands()
        {
            dict["new"] = (string[]? s) => new BeginCreateLangCommand();
            dict["continue"] = (string[]? s) => new ContinueCreateLangCommand();
            dict["AddDesc"] = (string[]? s) =>
            {
                if (s[0] == "DONE") return new EndCreateLangCommand();
                else if (s[0] == "ABORT") return new AbortCreateCommand();
                else return new LangAddDescCommand(s);
            };
            dict["Query"] = (string[]? s) =>
            {
                //if (s[0] == "suffices")return new ()
                if (s[0] == "DONE") return new EndQueryCommand();
                else return new QueryCommand(s);
            };
            dict["show"] = (string[]? s) =>
            {
                if (s == null || s.Length == 0) return new ShowStatementsCommand();
                if (s[0] == "lang") return new ShowStatementsCommand();
                else if (s[0] == "model") return new ShowModelCommand();

                return new ShowStatementsCommand();
            };
            dict["build"] = (string[]? s) => new BuildModelCommand();
            dict["exit"] = (string[]? s) => new ExitCommand();
            dict["query"] = (string[]? s) => new BeginQueryCommand();
        }

        static CommandFactory()
        {
            RegisterCommands();
        }

    }


    public static class CommandInvoker
    {

        public static States state = States.MAIN;

        public static ILanguage Lang = new Language();
        public static List<IStructure> Models;

        public static void Loop()
        {
            while (state != States.EXIT)
            {
                try
                {
                    string? s = Console.In.ReadLine();
                    ICommand? command = CommandFactory.Make(s, state);
                    if (command != null) command.Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


    }

    public interface ICommand
    {
        (CommandStatus, string[]) Execute();
    }

    public class BeginCreateLangCommand : ICommand
    {
        //string[] Parts;
        //public BeginCreateLangCommand(string[] parts)
        //{
        //      }
        public (CommandStatus, string[]) Execute()
        {
            CommandInvoker.Lang = new Language();
            CommandInvoker.Models = new List<IStructure>();
            CommandInvoker.state = States.CREATE;

            return (CommandStatus.STATE_CHANGED, new string[] { "State changed" });
        }
    }

    public class BeginQueryCommand : ICommand
    {
        //string[] Parts;
        //public BeginCreateLangCommand(string[] parts)
        //{
        //      }
        public (CommandStatus, string[]) Execute()
        {
            if (CommandInvoker.Models.Count == 0)
            {
                throw new Exception("No Model for the Language");
            }

            CommandInvoker.state = States.QUERY;
            return (CommandStatus.STATE_CHANGED, new string[] { "State changed" });
        }
    }

    public class ContinueCreateLangCommand : ICommand
    {
        public (CommandStatus, string[]) Execute()
        {
            CommandInvoker.state = States.CREATE;
            return (CommandStatus.STATE_CHANGED, new string[] { "State changed" });
        }
    }

    public class AbortCreateCommand : ICommand
    {
        public (CommandStatus, string[]) Execute()
        {
            CommandInvoker.Lang = new Language();
            CommandInvoker.state = States.MAIN;
            return (CommandStatus.SUCCESS, new string[] { "State changed" });

        }
    }

    public class QueryCommand : ICommand
    {
        string[] parts;
        public QueryCommand(string[] parts)
        {
            this.parts = parts;
        }

        public (CommandStatus, string[]) Execute()
        {
            if (CommandInvoker.Models.Count == 0) throw new Exception("No Model To Query");

            bool res = true;
            foreach (var model in CommandInvoker.Models)
            {
                res &= model.Query(IQueryStatement.Parse(parts));
            }
            if (res)
            {
                return (CommandStatus.QUERY_TRUE, new string[] { "It is a consequence of D" });
                // Console.WriteLine("It is a consequence of D");
            }
            else
            {
                Console.WriteLine("It is NOT a consequence of D");
                return (CommandStatus.QUERY_FALSE, new string[] { "It is NOT a consequence of D" });
            }

        }
    }

    public class LangAddDescCommand : ICommand
    {
        string[] parts;
        public LangAddDescCommand(string[] parts)
        {
            this.parts = parts;
        }
        public (CommandStatus, string[]) Execute()
        {
            CommandInvoker.Lang.AddStatement(IDescriptionStatement.Parse(parts));
            return (CommandStatus.SUCCESS, new string[] { "State changed" });
        }
    }

    public class EndCreateLangCommand : ICommand
    {
        //public EndCreateLangCommand(string[] parts)
        //{
        //}
        public (CommandStatus, string[]) Execute()
        {
            //CommandInvoker.Lang.MakeModels();
            CommandInvoker.state = States.MAIN;
            return (CommandStatus.STATE_CHANGED, new string[] { "State changed" });
        }
    }

    public class EndQueryCommand : ICommand
    {
        //public EndCreateLangCommand(string[] parts)
        //{
        //}
        public (CommandStatus, string[]) Execute()
        {
            //CommandInvoker.Lang.MakeModels();
            CommandInvoker.state = States.MAIN;
            return (CommandStatus.SUCCESS, new string[] { "State changed" });

        }
    }

    public class ShowStatementsCommand : ICommand
    {
        public (CommandStatus, string[]) Execute()
        {
            List<string> langList = new List<string>();

            foreach (var s in CommandInvoker.Lang.Statements)
            {

                // sb.Append($"{Count}: ");
                langList.Add(s.Content);
                // sb.Append(";&#10;");


            }
            return (CommandStatus.INFO_LANG, langList.ToArray());

        }
    }

    public class ShowModelCommand : ICommand
    {
        public (CommandStatus, string[]) Execute()
        {
            if (CommandInvoker.Models.Count == 0)
            {
                // Console.WriteLine("No Model");
                return (CommandStatus.FAILED, new string[] { "No Model" });
            }
            List<string> modelList = new List<string>();

            foreach (var s in CommandInvoker.Models)
            {
                string? str = s.ToString();
                if (str == null)
                {
                    modelList.Add("null");
                }
                else
                {
                    modelList.Add(str);
                }
            }
            return (CommandStatus.INFO_MODEL, modelList.ToArray());

        }
    }

    public class BuildModelCommand : ICommand
    {
        public (CommandStatus, string[]) Execute()
        {
            CommandInvoker.Models = CommandInvoker.Lang.MakeModels().ToList();
            return (CommandStatus.BUILD_SUCCESS, new string[] { "Model build success." });

        }
    }


    public class ExitCommand : ICommand
    {
        public (CommandStatus, string[]) Execute()
        {
            CommandInvoker.state = States.EXIT;
            return (CommandStatus.STATE_CHANGED, new string[] { "State changed" });

        }
    }
}

