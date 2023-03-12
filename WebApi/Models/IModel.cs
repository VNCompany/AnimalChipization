namespace WebApi.Models;

public interface IModel
{
    /// <summary>
    /// Валидация всех свойств модели
    /// </summary>
    /// <returns>True, если значения всех свойств модели валидны, false в противном случае</returns>
    bool Validate();
}