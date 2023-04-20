using System.Text.Json;

using DataLayer.Entities;
using Microsoft.Xna.Framework;
using SharpMath2;

namespace WebApi.Models;

public class AreaModel : IModel<Area>
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public AreaPoint?[]? AreaPoints { get; set; }

    /// <summary>
    /// После валидации здесь лежат только не NULL точки
    /// </summary>
    public AreaPoint[] AreaPointsValidated { get; set; } = Array.Empty<AreaPoint>();
    
    public AreaModel() {}

    public AreaModel(Area entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        AreaPointsValidated = JsonSerializer.Deserialize<AreaPoint[]>(entity.Geometry) 
                              ?? throw new InvalidOperationException();
    }
    
    /// <summary>
    /// Валидируется название зоны, корректность точек. Проверка: зона не содержит точек-дубликатов, все точки не лежат
    /// на одной прямой, нет пересечений отрезков (ребер полигона)
    /// </summary>
    /// <returns>True, если валидация прошла успешно, иначе false</returns>
    public bool Validate()
    {
        return !string.IsNullOrWhiteSpace(Name)
               && AreaPoints is { Length: >= 3 }
               && CheckAreaPoints();
    }

    public Area ToEntity(Area entity)
    {
        entity.Name = Name!;
        entity.Geometry = JsonSerializer.Serialize(AreaPointsValidated);
        return entity;
    }

    /// <summary>
    /// Проверка на совпадение точек зон без учёта позиции начальной точки
    /// </summary>
    /// <param name="other">Вторая сравниваемая зона</param>
    /// <returns>True, если геометрия совпадает, иначе false</returns>
    public bool GeometryEquals(AreaModel other)
    {
        int i1 = 0;
        int i2 = 0;
        bool isRecycle = false;

        while (i2 < other.AreaPointsValidated.Length)
        {
            if (i1 == AreaPointsValidated.Length)
            {
                if (i2 == 0)
                    break;
                i1 = 0;
                isRecycle = true;
            }

            if (AreaPointsValidated[i1] == other.AreaPointsValidated[i2])
                i2++;
            else if (isRecycle)
            {
                i2 = 0;
                break;
            }
            else
                i2 = 0;

            i1++;
        }

        return i2 != 0;
    }

    public Polygon2 GetPolygon() 
        => new Polygon2(AreaPointsValidated.Select(ptr => (Vector2)ptr).ToArray());

    public bool Intersects(AreaModel other) 
        => Polygon2.Intersects(GetPolygon(), other.GetPolygon(), 
        Vector2.Zero, Vector2.Zero, false);

    private bool CheckAreaPoints()
    {
        if (AreaPoints == null) throw new NullReferenceException(nameof(AreaPoints));
        
        // Проверка на валидность точек
        AreaPointsValidated = new AreaPoint[AreaPoints.Length];
        for (int i = 0; i < AreaPoints.Length; i++)
        {
            if (AreaPoints[i] == null
                || AreaPoints[i]!.Value.Longitude == null || AreaPoints[i]!.Value.Latitude == null
                || AreaPoints[i]!.Value.Longitude < -180 || AreaPoints[i]!.Value.Longitude > 180
                || AreaPoints[i]!.Value.Latitude < -90 || AreaPoints[i]!.Value.Latitude > 90)
                return false;
            AreaPointsValidated[i] = AreaPoints[i]!.Value;
        }
        
        // Проверка на дубликаты точек
        HashSet<AreaPoint> areaPointsHs = new HashSet<AreaPoint>(AreaPointsValidated);
        if (areaPointsHs.Count != AreaPointsValidated.Length)
            return false;  // Были найдены дубликаты

        bool curve = false;
        for (int i = 2; i < AreaPointsValidated.Length; i++)
        {
            // Проверка на прямую
            if(!curve && IsCurve(AreaPointsValidated[i - 2], AreaPointsValidated[i - 1], AreaPointsValidated[i])) 
                curve = true;  // Есть кривая, соответственно точки не лежат на одной прямой
            
            // Проверяем, что этот отрезок не имеет пересечений с другими границами
            if (LineIntersects(AreaPointsValidated, i)) return false; // Пересечение найдено, геометрия неверная
        }
        
        // Проверка на то, лежат ли все точки на одной прямой
        if (!curve)
            return false;
        
        // Проверка на пересечения последнего отрезка, соединяющего первую и последнюю точки
        if (LineIntersects(
                AreaPointsValidated,
                AreaPointsValidated.Length - 1,
                new Line2(AreaPointsValidated[^1], AreaPointsValidated[0]),
                true))
            return false;

        return true;
    }
    
    private static bool LineIntersects(AreaPoint[] arr, int pos, Line2? line = null, bool firstStrict = false)
    {
        Line2 ln = line ?? new Line2(arr[pos], arr[pos - 1]);
        for (int i = pos - 1, j = 0; i > 0; i--, j++)
        {
            bool strict = j == 0 || (firstStrict && i == 1);
            if (Line2.Intersects(
                    ln,
                    new Line2(arr[i], arr[i - 1]),
                    Vector2.Zero, Vector2.Zero, strict))
                return true;
        }

        return false;
    }
    
    private static bool IsCurve(AreaPoint point1, AreaPoint point2, AreaPoint point3)
    {
        return (point3.Longitude - point1.Longitude) / (point2.Longitude - point1.Longitude) 
               != (point3.Latitude - point1.Latitude) / (point2.Latitude - point1.Latitude);
    }
}