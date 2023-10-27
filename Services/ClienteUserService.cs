using ESFE.Chatbot.Models;
using ESFE.Chatbot.Repositories;
using ESFE.Chatbot.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESFE.Chatbot.Services;

public class ClienteUserService : IClienteUserService
{

    private readonly IGenericRepository<ClientUser> _userRepo;
    public ClienteUserService(IGenericRepository<ClientUser> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<bool> Create(ClientUser model) => await _userRepo.Create(model);

    public async Task<bool> Update(ClientUser model) => await _userRepo.Update(model);
    public async Task<bool> Delete(int id) => await _userRepo.Delete(id);

    public Task<IQueryable<ClientUser>> GetAll() => _userRepo.GetAll();
    public async Task<bool> ConfirmAccount(string token)
    {
        try
        {
            var users = await _userRepo.GetAll();
            ClientUser existingClientUser = await users.FirstAsync(a => a.Token == token);
            existingClientUser.ConfirmAccount = true;

            await _userRepo.Update(existingClientUser);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ClientUser?> GetByEmail(string email)
    {
        try
        {
            var users = await _userRepo.GetAll();
            var result = await users.FirstAsync(a => a.Email == email);
            return result;
        }
        catch
        {
            return null;
        }
    }

    public Task<ClientUser?> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RestartAccount(bool restart, string password, string token)
    {
        throw new NotImplementedException();
    }


    public async Task<ClientUser?> Validate(string email, string password)
    {
        try
        {
            var users = await _userRepo.GetAll();
            var result = await users.FirstAsync(a => a.Email == email && a.Password == password);
            return result;
        }
        catch
        {
            return null;
        }
    }

     public async Task<bool> ValidateConfirm(string email)
    {
        try
        {
            var users = await _userRepo.GetAll();
            var result = await users.FirstAsync(a => a.Email == email && a.ConfirmAccount == true);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
