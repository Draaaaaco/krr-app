
using Microsoft.AspNetCore.Mvc;

namespace KRR_Proj.Controllers;

[ApiController]
[Route("[controller]")]
public class KnowledgeController : ControllerBase
{
    private readonly ILogger<KnowledgeController> _logger;

    public KnowledgeController(ILogger<KnowledgeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(1, 5);
    }

    [Route("state")]
    [HttpGet]
    public ActionResult GetStatus()
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "*");
        // Response.Headers.Add('Access-Control-Allow-Credentials', 'true');

        return Ok(new StateResponse(CommandInvoker.state.ToString()));
        // return Content($"State: {CommandInvoker.state}", "text/html");
    }


    [Route("command")]
    [HttpPost]
    public ActionResult ExecuteCommand([FromBody] CommandRequest commandRequest)
    {
        Response.Headers.Add("Access-Control-Allow-Origin", "*");
        try
        {

            string? text = commandRequest.Text;
            string[] output = new string[]{"No command."};
            if (text == null)
            {
                return Ok(new CommandResponse(CommandStatus.FAILED, output));
            }

            List<string> commandTextList = text.Split('\n').ToList();
            // bool flag = false;
            CommandStatus status = CommandStatus.FAILED;
            
            ICommand? command;
            foreach (string singleText in commandTextList)
            {
                Console.WriteLine(singleText);
                Console.WriteLine(";;;");
                command = CommandFactory.Make(singleText, CommandInvoker.state);

                if (command != null)
                {
                    // flag = true;
                    (status, output) = command.Execute();

                }
            }
            return Ok(new CommandResponse(status, output));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return Ok(new CommandResponse(CommandStatus.FAILED, new string[]{e.ToString()}));
        }
        
    }


}


public class CommandRequest
{
    public string? Text { get; set; }
}

public class StateResponse
{
    public StateResponse(string state)
    {
        State = state;
    }
    public string State { get; set; }
}

public class CommandResponse
{
    public CommandResponse(CommandStatus status, string[] text)
    {
        this.Status = status.ToString();
        this.Text = text;
    }
    public string Status { get; set; }
    public string[] Text { get; set; }
}



// try
// 				{
//                     string? s = Console.In.ReadLine();
//                     ICommand? command = CommandFactory.Make(s, state);
//                     if (command != null) command.Execute();
//                 }
// 				catch(Exception e)
// 				{
// 					Console.WriteLine(e);
// 				}