using Microsoft.AspNetCore.Mvc;
using TimeToDo.Controllers;

namespace TimeToDo.API.Controllers;

public class TasksController : ApiController
{
    [HttpGet]
    public ActionResult<bool> ToDoTestMethod()
    {
        return Ok(true);
    }
}
