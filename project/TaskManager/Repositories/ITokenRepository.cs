using TaskManager.Models.Domain;

namespace TaskManager.Repositories;

public interface ITokenRepository
{
    string CreateJWTToken(UserModel user);
}