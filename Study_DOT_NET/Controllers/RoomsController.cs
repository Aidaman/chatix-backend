using Microsoft.AspNetCore.Mvc;
using Study_DOT_NET.Models;
using Study_DOT_NET.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study_DOT_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly RoomsService _roomsService;

        public RoomsController(RoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        [HttpGet]
        public async Task<List<Room>> Get()
        {
            return await this._roomsService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Room>> Get(string id)
        {
            Room room = await this._roomsService.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Room newRoom)
        {
            await this._roomsService.CreateAsync(newRoom);

            //CreatedAtAction() - creates ICreatedAtAction object that basically returns 201 response with Location header
            return CreatedAtAction(nameof(Get), new { id = newRoom.Id }, newRoom);
        }

        [HttpPut("id;length(24)")]
        public async Task<IActionResult> Update(string id, Room updatedRoom)
        {
            Room room = await this._roomsService.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            updatedRoom.Id = room.Id;

            await this._roomsService.UpdateAsync(id, updatedRoom);

            //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
            return NoContent();
        }

        [HttpDelete("id;length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            Room room = await this._roomsService.GetAsync(id);

            if (room is null)
            {
                return NotFound();
            }

            await this._roomsService.RemoveAsync(id);

            //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
            return NoContent();
        }


    }
}
