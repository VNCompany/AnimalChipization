using System.Text.RegularExpressions;

namespace WebApi.Internal;

public enum DateTimeValidationStatus { Null = 0, Failure = 1, Success = 2 }
public record DateTimeValidationResult(DateTimeValidationStatus Status, DateTime Value = default);

/// <summary>
/// Класс методов для работы с датой и временем в формате ISO-8601 
/// </summary>
public static class DateTimeMethods
{
    static readonly Regex regex = new Regex(@"^\d{4}-\d{2}-\d{2}(T|\s)\d{2}:\d{2}:\d{2}(Z|\+\d{2}:\d{2})$");

    /// <summary>
    /// Возвращает текущую дату и время, исключая миллисекунды
    /// </summary>
    /// <returns>DateTime без миллисекунд</returns>
    public static DateTime NowWithoutMilliseconds()
    {
        var now = DateTime.Now;
        return new DateTime(
            now.Year, now.Month, now.Day,
            now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
    }

    /// <summary>
    /// Получить из строки дату и время в формате ISO-8601
    /// </summary>
    /// <param name="formattedDateTime">строка с датой и временем в формате ISO-8601</param>
    /// <returns>Структура, представляющая результат парсинга и валидации даты и времени</returns>
    public static DateTimeValidationResult Parse(string? formattedDateTime)
    {
        if (formattedDateTime is null)
            return new(DateTimeValidationStatus.Null);

        if (regex.IsMatch(formattedDateTime))
        {
            if (DateTime.TryParse(formattedDateTime, out DateTime dateTime))
                return new(DateTimeValidationStatus.Success, dateTime);
        }
        return new(DateTimeValidationStatus.Failure);
    }
}