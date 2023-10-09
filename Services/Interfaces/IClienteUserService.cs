using ESFE.Chatbot.Models;

namespace ESFE.Chatbot.Services.Interfaces;

public interface IClienteUserService
{
  Task<bool> Create(ClientUser model);
  Task<bool> Update(ClientUser model);
  Task<bool> Delete(int id);
  Task<IQueryable<ClientUser>> GetAll();
  Task<ClientUser?> GetById(int id);
  Task<ClientUser?> Validate(string email, string password);
  Task<ClientUser?> GetByEmail(string email);
  Task<bool> RestartAccount(bool restart, string password, string token);
  Task<bool> ConfirmAccount(string token, int userID);
}
