using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{

    /// <summary>
    /// Retrieves all maps
    /// </summary>
    /// <returns>list of map objects</returns>
    [HttpGet]
    public IEnumerable<Map> GetAllMaps()
    {
        return MapDataAccess.GetMaps();
    }

    /// <summary>
    /// gets all square maps where the columns == row
    /// </summary>
    /// <returns> list of square map objects</returns>
    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMaps()
    {
        var maps = MapDataAccess.GetMaps();
        return maps.Where(m => m.Columns == m.Rows);
    }

    /// <summary>
    /// gets a map by its ID.
    /// </summary>
    /// <param name="id">The ID of the map</param>
    /// <returns>The requested map if found</returns>
    /// <response code="200">Map found</response>
    /// <response code="404">Map not found</response>
    [HttpGet("{id}")]
    public IActionResult GetMapById(int id)
    {
        var map = MapDataAccess.GetMapById(id);
        if (map == null) return NotFound();
        return Ok(map);
    }


    /// <summary>
    /// Creates a new map
    /// </summary>
    /// <param name="newMap">The map object to create</param>
    /// <returns>The newly created map</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/maps
    ///     {
    ///       "name": "TestMap",
    ///       "description": "Testing layout",
    ///       "columns": 4,
    ///       "rows": 4,
    ///       "isSquare": true
    ///     }
    /// </remarks>
    /// <response code="201">Map successfully created</response>
    /// <response code="400">Invalid map data</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public IActionResult AddMap(Map newMap)
    {
        if (newMap == null) return BadRequest();

        newMap.CreatedDate = DateTime.Now;
        newMap.ModifiedDate = DateTime.Now;

        var added = MapDataAccess.AddMap(newMap);
        return CreatedAtAction(nameof(GetMapById), new { id = added.Id }, added);
    }

    /// <summary>
    /// Updates an existing map
    /// </summary>
    /// <param name="id">The ID of the map to update</param>
    /// <param name="updatedMap">The updated map object</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">Update successful</response>
    /// <response code="404">Map not found</response>
    /// <response code="500">Update failed on the server</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPut("{id}")]
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        var map = MapDataAccess.GetMapById(id);
        if (map == null) return NotFound();

        updatedMap.ModifiedDate = DateTime.Now;
        var success = MapDataAccess.UpdateMap(id, updatedMap);
        return success ? NoContent() : StatusCode(500, "Update failed.");
    }


    /// <summary>
    /// Deletes a map by ID
    /// </summary>
    /// <param name="id">The ID of the map to delete</param>
    /// <returns>No content if deletion is successful</returns>
    /// <response code="204">Map successfully deleted</response>
    /// <response code="404">Map not found</response>
    /// <response code="500">Deletion failed on the server</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id)
    {
        var map = MapDataAccess.GetMapById(id);
        if (map == null) return NotFound();

        var success = MapDataAccess.DeleteMap(id);
        return success ? NoContent() : StatusCode(500, "Delete failed.");
    }


    /// <summary>
    /// Checks whether a coordinate (x, y) is inside the map bound
    /// </summary>
    /// <param name="id">The ID of the map</param>
    /// <param name="x">The x coordinate</param>
    /// <param name="y">The y coordinate</param>
    /// <returns>True if coordinate is valid on the map; otherwise false</returns>
    /// <response code="200">Coordinate check result</response>
    /// <response code="400">Invalid coordinate values</response>
    /// <response code="404">Map not found</response>
    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        var map = MapDataAccess.GetMapById(id);
        if (map == null) return NotFound();
        if (x < 0 || y < 0) return BadRequest();

        bool isOnMap = x < map.Columns && y < map.Rows;
        return Ok(isOnMap);
    }
}
