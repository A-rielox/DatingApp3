using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    //

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
        
    }


    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    //
    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }


    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    //
    public async Task<Message> GetMessage(int id)
    {
        var message = await _context.Messages.FindAsync(id);

        return message;
    }


    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    //
    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = _context.Messages.OrderBy(m => m.MessageSent)
                                     .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(m => m.RecipientUsername == messageParams.Username && m.RecipientDeleted == false),
            "Outbox"=> query.Where(m => m.SenderUsername == messageParams.Username && m.SenderDeleted == false),
            _       => query.Where(m => m.RecipientUsername == messageParams.Username && m.RecipientDeleted == false
                                                            && m.DateRead == null)
        };

        var messagesQuery = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

        var pagedMessages = await PagedList<MessageDto>
                              .CreateAsync(messagesQuery, messageParams.PageNumber,
                                           messageParams.PageSize);

        return pagedMessages;
    }


    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    //
    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName,
                                                                string recipientUserName)
    {
        var query = _context.Messages
            .Where(
                m => m.RecipientUsername == currentUserName && m.RecipientDeleted == false &&
                m.SenderUsername == recipientUserName ||
                m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
                m.SenderUsername == currentUserName
            )
            .OrderByDescending(m => m.MessageSent)
            .AsQueryable();

        var unreadMessages = query.Where(m => m.DateRead == null &&
                                m.RecipientUsername == currentUserName).ToList();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }

            //await _context.SaveChangesAsync();  ---- x UnitOfWork guardo en el controller (MessagesController)
        }

        return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    /*              previo ProjectTo
    {
        var messages = await _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(
                m => m.RecipientUsername == currentUserName && m.RecipientDeleted == false &&
                m.SenderUsername == recipientUserName ||
                m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
                m.SenderUsername == currentUserName
            )
            .OrderByDescending(m => m.MessageSent)
            .ToListAsync();

        var unreadMessages = messages.Where(m => m.DateRead == null &&
                                m.RecipientUsername == currentUserName).ToList();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }

            //await _context.SaveChangesAsync();  ---- x UnitOfWork guardo en el controller (MessagesController)
        }

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }
    */


    /////////////////////////////////////////////////////
    /////////////////////////////////////////////////////
    //
    //
    //                  ------------ x UnitOfWork
    //
    //public async Task<bool> SaveAllAsync()
    //{
    //    return await _context.SaveChangesAsync() > 0;
    //}
}
