﻿using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context,
                          IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }




    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
                                 .FirstOrDefaultAsync(u => u.Id == id);

        return user;
    }


    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users
                                 .FirstOrDefaultAsync(u => u.UserName == username);

        return user;        
    }


    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();

        return users;
    }


    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public async Task<MemberDto> GetMemberAsync(string username)
    {
        var member = await _context.Users
                                   .Where(u => u.UserName == username)
                                   .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                   .FirstOrDefaultAsync();

        return member;
    }



    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
        var members = await _context.Users
                                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync();

        return members;
    }


    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }


    ////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    //
    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}