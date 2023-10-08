using ESFE.Chatbot.Models;
using ESFE.Chatbot.Repositories;

namespace ESFE.Chatbot;

public class TypeUserRepository : IGenericRepository<TypeUser>
{
  private readonly ChatBotDbContext _db;
  public TypeUserRepository(ChatBotDbContext db){
    _db = db;
  }
  public Task<bool> Create(TypeUser model)
  {
    throw new NotImplementedException();
  }

  public Task<bool> Delete(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<IQueryable<TypeUser>> GetAll()
  {
    return await Task.FromResult(_db.TypeUsers.AsQueryable());
  }

  public Task<TypeUser?> GetById(int id)
  {
    throw new NotImplementedException();
  }

  public Task<bool> Update(TypeUser model)
  {
    throw new NotImplementedException();
  }
}
