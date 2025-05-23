using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    /// <summary>
    /// Get all the robot commands.
    /// </summary>
    /// <returns>A list of RobotCommand objects.</returns>
    [HttpGet]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        return RobotCommandDataAccess.GetRobotCommands();
    }

    /// <summary>
    /// Get only the robot commands that are move based.
    /// </summary>
    /// <returns>A filtered list of RobotCommand where IsMoveCommand is true.</returns>
    [HttpGet("move")]
    public IEnumerable<RobotCommand> GetMoveCommandsOnly()
    {
        var allCommands = RobotCommandDataAccess.GetRobotCommands();
        return allCommands.Where(c => c.IsMoveCommand);
    }


    /// <summary>
    /// Retrieves a specific robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the robot command.</param>
    /// <returns>The matching ID of Robotcommand or 404 if not found.</returns>
    /// <response code="200">Robot command found.</response>
    /// <response code="404">Robot command not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetRobotCommand")]
    public IActionResult GetRobotCommandById(int id)
    {
        var command = RobotCommandDataAccess.GetRobotCommandById(id);
        if (command == null) return NotFound();
        return Ok(command);
    }

    /// <summary>
    /// Creates a robot command.
    /// </summary>
    /// <param name="newCommand">The robot command object from the client.</param>
    /// <returns>The created RobotCommand object</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/robot-commands
    ///     {
    ///       "name": "RUN",
    ///       "isMoveCommand": true,
    ///       "description": "Salsa on the Moon"
    ///     }
    /// </remarks>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Invalid request payload</response>
    /// <response code="409">Duplicate command name</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand == null) return BadRequest();

        newCommand.CreatedDate = DateTime.Now;
        newCommand.ModifiedDate = DateTime.Now;

        var added = RobotCommandDataAccess.AddRobotCommand(newCommand);
        return CreatedAtRoute("GetRobotCommand", new { id = added.Id }, added);
    }


    /// <summary>
    /// Updates an existing robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the command to update.</param>
    /// <param name="updatedCommand">The updated Robotcommand.</param>
    /// <returns>No content if successful or error status.</returns>
    /// <response code="204">Successfully updated</response>
    /// <response code="404">Command not found</response>
    /// <response code="500">Update failed due to server error</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        var existing = RobotCommandDataAccess.GetRobotCommandById(id);
        if (existing == null) return NotFound();

        updatedCommand.ModifiedDate = DateTime.Now;
        var success = RobotCommandDataAccess.UpdateRobotCommand(id, updatedCommand);
        return success ? NoContent() : StatusCode(500, "Update failed.");
    }
    

    /// <summary>
    /// Deletes robot command by ID.
    /// </summary>
    /// <param name="id">The ID of the command to delete.</param>
    /// <returns>No content if successful or error status.</returns>
    /// <response code="204"> Deleted </response>
    /// <response code="404">Command not found</response>
    /// <response code="500">Delete failed due to server error</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public IActionResult DeleteRobotCommand(int id)
    {
        var existing = RobotCommandDataAccess.GetRobotCommandById(id);
        if (existing == null) return NotFound();

        var success = RobotCommandDataAccess.DeleteRobotCommand(id);
        return success ? NoContent() : StatusCode(500, "Delete failed.");
    }
}
