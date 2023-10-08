using ESFE.Chatbot.Models;

namespace ESFE.Chatbot;

public class ClientUserRepository
{
  private readonly ChatBotDbContext _db;

  public ClientUserRepository(ChatBotDbContext db)
  {
    _db = db;
  }
  public async Task<bool> Create(ClientUser model)
  {
    try
    {
      _db.Add(model);
      await _db.SaveChangesAsync();
      return true;
    }
    catch
    {
      // Manejo de errores
      return false;
    }
  }

  public async Task<bool> Update(ClientUser model)
  {
    try
    {
      _db.Update(model);
      await _db.SaveChangesAsync();
      return true;
    }
    catch
    {
      // Manejo de errores
      return false;
    }
  }

  public async Task<bool> Delete(int id)
  {
    var user = await _db.ClientUsers.FindAsync(id);
    if (user == null)
      return false;

    try
    {
      _db.Remove(user);
      await _db.SaveChangesAsync();
      return true;
    }
    catch
    {
      // Manejo de errores
      return false;
    }
  }

  public async Task<IQueryable<ClientUser>> GetAll()
  {
    return await Task.FromResult(_db.ClientUsers.AsQueryable());
  }

  public async Task<ClientUser?> GetById(int id)
  {
    return await _db.ClientUsers.FindAsync(id);
  }

}
