using System.ComponentModel.DataAnnotations.Schema;

namespace robot_controller_api;

public class RobotCommand
{
    public int Id { get; set; }

    [Column("name")] //map C# Name to postgre sql name
    public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("ismovecommand")]
    public bool IsMoveCommand { get; set; }

    [Column("createddate")]
    public DateTime CreatedDate { get; set; }

    [Column("modifieddate")]
    public DateTime ModifiedDate { get; set; }

    public RobotCommand(int id, string name, bool isMoveCommand, DateTime createdDate, DateTime modifiedDate, string? description = null)
    {
        Id = id;
        Name = name;
        IsMoveCommand = isMoveCommand;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description;
    }
}
