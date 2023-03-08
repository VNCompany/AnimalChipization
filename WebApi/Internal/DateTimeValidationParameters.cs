namespace WebApi.Internal;

public enum DateTimeValidationStatus {
    /// <summary>
    /// Строка с датой и временем была NULL
    /// </summary>
    Null = 1,

    /// <summary>
    /// Дата и время имели неверные значения
    /// </summary>
    /// 
    InvalidDateTime = 2,

    /// <summary>
    /// Успешная валидация
    /// </summary>
    Success = 4
}

/// <summary>
/// Структура содержащая результат валидации даты и времени
/// </summary>
public struct DateTimeValidationParameters
{
    /// <summary>
    /// Статус валидации даты и времени
    /// </summary>
    public DateTimeValidationStatus Status { get; }

    /// <summary>
    /// Полученное дата и время в случае успеха, иначе default(DateTime)
    /// </summary>
    public DateTime DateTime { get; }

    public DateTimeValidationParameters(DateTimeValidationStatus status)
    {
        Status = status;
        DateTime = default;
    }

    public DateTimeValidationParameters(DateTimeValidationStatus status, DateTime dateTime)
    {
        Status = status;
        DateTime = dateTime;
    }
}