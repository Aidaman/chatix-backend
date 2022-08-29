using Microsoft.AspNetCore.Mvc;
using Study_DOT_NET.Models;
using Study_DOT_NET.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Study_DOT_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesService _messagesService;

        public MessagesController(MessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        [HttpGet]
        public async Task<List<Message>> Get()
        {
            Console.WriteLine("GET messages");
            return await this._messagesService.GetAsync();
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Message>> Get(string id)
        {
            Console.WriteLine($"GET message by ID: {id}");
            Message message = await this._messagesService.GetAsync(id);
        
            if (message is null)
            {
                return NotFound();
            }
        
            return message;
        }

        [HttpGet("{roomId:length(24)}/{offset}/{limit}")]
        // public async Task<List<Message>?> Get(string roomId, int offset, int limit)
        public async Task<List<Message>?> Get(string roomId)
        {
            Console.WriteLine($"All is ok. roomId: {roomId}, offset: {0}, limit: {50}");
            List<Message>? messages = await this._messagesService.GetRoomContentAsync(roomId, 0, 50);

            if (messages is null)
            {
                return null;
            }

            return messages;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Message newMessage)
        {
            await this._messagesService.CreateAsync(newMessage);

            //CreatedAtAction() - creates ICreatedAtAction object that basically returns 201 response with Location header
            return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);
        }

        [HttpPut("id:length(24)")]
        public async Task<IActionResult> Update(string id, Message updatedMessage)
        {
            Message message = await this._messagesService.GetAsync(id);

            if (message is null)
            {
                return NotFound();
            }

            updatedMessage.Id = message.Id;

            await this._messagesService.UpdateAsync(id, updatedMessage);

            //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
            return NoContent();
        }

        [HttpDelete("id;length(24)")]
        public async Task<IActionResult> Delete(string id)
        {
            Message message = await this._messagesService.GetAsync(id);

            if (message is null)
            {
                return NotFound();
            }

            await this._messagesService.RemoveAsync(id);

            //NoContent() - creates NoContentResult object that basically returns response to server and signalling that there is empty response body
            return NoContent();
        }


    }
}
