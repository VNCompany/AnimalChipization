namespace WebApi.Models;

/// <summary>
/// Модель представления для сущности
/// </summary>
/// <typeparam name="T">Сущность</typeparam>
public interface IModel<T> where T : class
{
    /// <summary>
    /// Валидация всех свойств модели
    /// </summary>
    /// <returns>True, если значения всех свойств модели валидны, false в противном случае</returns>
    bool Validate();

    /// <summary>
    /// Заполняет сущность значениями из модели
    /// </summary>
    /// <returns>Сущность</returns>
    T ToEntity(T entity);
}