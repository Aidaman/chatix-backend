using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Study_DOT_NET.Shared.Commands.Messages;
using Study_DOT_NET.Shared.ConfigClasses;
using Study_DOT_NET.Shared.Services;

namespace Study_DOT_NET.Hubs;

public class MessagesHub : Hub
{
    private readonly CreateMessageCommand _createMessageCommand;
    private readonly DeleteMessageCommand _deleteMessageCommand;
    private readonly ReadMessageCommand _readMessageCommand;
    private readonly RoomsService _roomService;
    private readonly UpdateMessageCommand _updateMessageCommand;
    private readonly UserRegistryService _userRegistryService;

    public MessagesHub(CreateMessageCommand createMessageCommand, DeleteMessageCommand deleteMessageCommand,
        UpdateMessageCommand updateMessageCommand, ReadMessageCommand readMessageCommand, RoomsService roomService,
        UserRegistryService userRegistryService)
    {
        _createMessageCommand = createMessageCommand;
        _deleteMessageCommand = deleteMessageCommand;
        _updateMessageCommand = updateMessageCommand;
        _readMessageCommand = readMessageCommand;
        _roomService = roomService;
        _userRegistryService = userRegistryService;
    }

    /// <summary>
    ///     This method generates new MessageConfig instance from json
    /// </summary>
    /// <param name="message">JSON representation of the message content that front-end gives here</param>
    /// <returns>new MessageConfig instance</returns>
    /// <exception cref="NullReferenceException">If instead of JSON null was given; or if deserialization was unsuccessful</exception>
    private MessageConfig GenerateMessageConfig(List<string> message)
    {
        return JsonSerializer.Deserialize<MessageConfig>(message[0]
                                                         ?? throw new NullReferenceException(
                                                             "message parameter is null"))
               ?? throw new NullReferenceException("unsuccessful deserialization result is null");
    }

    /// <summary>
    ///     Method that calls to generate new messages
    ///     It calls CreateMessageCommand class
    /// </summary>
    /// <param name="message"> JSON representation of MessageConfig class, that CreateMessageCommand class  require to work </param>
    /// <exception cref="ApplicationException">If new message is null then something went wrong ¯\_(ツ)_/¯</exception>
    public async Task CreateMessage(List<string> message)
    {
        try
        {
            var messageConfig = GenerateMessageConfig(message);

            _createMessageCommand._messageConfig = messageConfig;
            var createdMessage = await _createMessageCommand.Execute();

            // Room room = await this._roomService.GetAsync(createdMessage.RoomId);
            // foreach (string id in room.ParticipantsIds)
            // {
            //     var connection = this._userRegistryService.GetConnectionId(id);
            //     Console.WriteLine($"connection is {(connection ?? "null")}");
            //     if (connection is not null)
            //     {
            //         Console.WriteLine($"\\{connection} is not null/");
            //         await Clients.Client(connection).SendAsync("newMessage", createdMessage 
            //             ?? throw new NoNullAllowedException("sending null is forbidden"));
            //     }
            // }

            await Clients.All.SendAsync("newMessage", createdMessage
                                                      ?? throw new ApplicationException(
                                                          "message prototype, occasionally, is not the message object"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, message);
            throw;
        }
    }

    /// <summary>
    ///     Method that calls to edit already created messages
    ///     It calls UpdateMessageCommand class
    /// </summary>
    /// <param name="message"> JSON representation of MessageConfig class, that UpdateMessageCommand class  require to work </param>
    /// <exception cref="ApplicationException">If new message is null then something went wrong ¯\_(ツ)_/¯</exception>
    public async Task UpdateMessage(List<string> message)
    {
        try
        {
            var messageConfig = GenerateMessageConfig(message);

            _updateMessageCommand._messageConfig = messageConfig;
            var updatedMessage = await _updateMessageCommand.Execute();

            await Clients.All.SendAsync("messageUpdated", updatedMessage
                                                          ?? throw new ApplicationException(
                                                              "message prototype, occasionally, is not the message object"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, message);
            throw;
        }
    }

    /// <summary>
    ///     Method that calls to delete messages
    ///     It calls DeleteMessageCommand class
    /// </summary>
    /// <param name="message"> JSON representation of MessageConfig class, that DeleteMessageCommand class  require to work </param>
    /// <exception cref="ApplicationException">If message is null then something went wrong ¯\_(ツ)_/¯</exception>
    public async Task DeleteMessage(List<string> message)
    {
        try
        {
            var messageConfig = GenerateMessageConfig(message);

            _deleteMessageCommand._messageConfig = messageConfig;
            var deletedMessage = await _deleteMessageCommand.Execute();

            await Clients.All.SendAsync("messageDeleted", deletedMessage
                                                          ?? throw new ApplicationException(
                                                              "message prototype, occasionally, is not the message object"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, message);
            throw;
        }
    }

    /// <summary>
    ///     Method that calls to mark message as read by someone (add to message.read array new record)
    ///     It calls ReadMessageCommand class
    /// </summary>
    /// <param name="message"> JSON representation of MessageConfig class, that ReadMessageCommand class  require to work </param>
    /// <exception cref="ApplicationException">If message is null then something went wrong ¯\_(ツ)_/¯</exception>
    public async Task ReadMessage(List<string> message)
    {
        try
        {
            var messageConfig = GenerateMessageConfig(message);

            _readMessageCommand._messageConfig = messageConfig;
            var updatedMessage = await _readMessageCommand.Execute();

            await Clients.All.SendAsync("messageUpdated", updatedMessage
                                                          ?? throw new ApplicationException(
                                                              "message prototype, occasionally, is not the message object"));

            await Clients.All.SendAsync("messageRead", updatedMessage
                                                       ?? throw new ApplicationException(
                                                           "message prototype, occasionally, is not the message object"));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, message);
            throw;
        }
    }
}