using DataLayer.Entities;
using WebApi.Models;

namespace WebApi.Services;

/// <summary>
/// Сервис авторизации
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Авторизация пользователя по токену
    /// </summary>
    /// <param name="token">Токен авторизации в base64</param>
    /// <param name="account">Аккаунт пользователя. Имеет значение null, если авторизация не прошла</param>
    /// <returns>Возвращает true, если авторизация прошла успешно, false в противном случае</returns>
    bool Authorize(string token, out Account? account);

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="accountModel">Модель аккаунта</param>
    /// <returns>Новый аккаунт при успехе, иначе null</returns>
    Account? Register(AccountModel accountModel);
}