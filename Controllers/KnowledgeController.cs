
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
    [HttpPost]
    public ActionResult GetStatus()
    {
        return Content($"State: {CommandInvoker.state}", "text/html");
    }


    [Route("command")]
    [HttpPost]
    public ActionResult ExecuteCommand([FromBody] CommandRequest commandRequest)
    {
        try
        {
            ICommand? command = CommandFactory.Make(commandRequest.Text, CommandInvoker.state);
            if (command != null) {
                (CommandStatus status, string output) = command.Execute();
                return Ok(new CommandResponse(status, output));
            }else{
                return Ok(new CommandResponse(CommandStatus.FAILED, "No command."));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new CommandResponse(CommandStatus.FAILED, e.ToString()));
        }
        // return Ok(commandRequest);
        // return Content($"Show: {commandText.Text}", "text/html");
    }


}


public class CommandRequest
{
    public string? Text { get; set; }
}

public class CommandResponse
{
    public CommandResponse(CommandStatus status, string text) {
        this.Status = status;
        this.Text = text;
    }
    public CommandStatus Status {get; set;}
    public string Text { get; set; }
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