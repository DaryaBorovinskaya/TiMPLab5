using Task1;

public partial class Program
{
    /// <summary>
    /// Проверка на верно введённое целочисленное положительное значение
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static bool IsOnlyNumbers(string line)
    {
        if (line == string.Empty)
            return false;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] < '0' || line[i] > '9' || (line[i] == '0' && i == 0))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Проверка на верно введённые целочисленные значения
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static bool IsOnlyDigitals(string line)
    {
        if (line == string.Empty)
            return false;

        for (int i = 0; i < line.Length; i++)
        {
            if (i == 0 && line[i] == '-')
                continue;
            if (line[i] < '0' || line[i] > '9' || (line[i] == '0' && i == 0 && line.Length > 1))
                return false;
        }

        return true;
    }



    /// <summary>
    /// Вычисление суммарной длины тени
    /// </summary>
    /// <param name="totalSum"></param>
    /// <param name="segments"></param>
    /// <param name="countPoints"></param>
    /// <param name="minAbscissa"></param>
    /// <param name="maxAbscissa"></param>
    private static void CalculateTotalSum(out int totalSum, Segment[] segments, int countPoints, int minAbscissa, int maxAbscissa )
    {
        totalSum = segments[0].Length;
        if (countPoints > 1)
        {
            // Полное перекрывание самым длинным отрезком всех остальных.
            for (int i = countPoints-1; i >=0 ; i--)
            {
                if (segments[i].BeginPoint <= minAbscissa && segments[i].EndPoint == maxAbscissa)
                {
                    totalSum = segments[i].Length;
                    return;
                }
            }

            
            for(int i = 1; i < countPoints-1; i++) 
            {
                if (segments[i].EndPoint > segments[i+1].BeginPoint)
                {
                    // Отрезки частично перекрывают друг друга.
                    totalSum +=  Math.Abs(segments[i+1].EndPoint - segments[i].BeginPoint);
                }
                else 
                {
                    // Тени соседних отрезков соединяются в одну или не пересекаются.
                    totalSum += segments[i].Length + segments[i+1].Length;
                }
                
            }

        }

    }
    
    public static void Main(string[] args)
    {
        string line,line2;
        bool isDigital = true;
        int countPoints=0;

        // Ограничение пользовательского ввода (можно вводить только целочисленное положительное значение).
        do
        {
            Console.Write("Введите количество отрезков: ");
            line = Console.ReadLine();
            if (IsOnlyNumbers(line))
            {
                isDigital = true;
                countPoints = Convert.ToInt32(line);
            }
            else
            {
                isDigital = false;
                Console.WriteLine("ОШИБКА: Введите целочисленное положительное значение. ");
            }
        } while (!isDigital);

        Segment[] segments = new Segment[countPoints];
        int beginPoint=0, endPoint=0;
        bool isRightSegment = true;
        for (int i = 1; i <= countPoints; i++)
        {
            // Ограничение пользовательского ввода (можно вводить только различные целочисленные значения).
            do
            {
                Console.WriteLine($"Введите абсциссы {i} отрезка (через Enter): ");
                line = Console.ReadLine();
                line2 = Console.ReadLine();
                if (IsOnlyDigitals(line) && IsOnlyDigitals(line2))
                {
                    isDigital = true;
                    beginPoint = Convert.ToInt32(line);
                    endPoint = Convert.ToInt32(line2);

                    if (beginPoint == endPoint)
                    {
                        Console.WriteLine("Точка не является отрезком!");
                        isRightSegment = false;
                    }
                    else
                        isRightSegment = true;
                }
                else
                {
                    isDigital = false;
                    Console.WriteLine("ОШИБКА: Введите целочисленные значения. ");
                }
            } while (!isRightSegment || !isDigital);

            segments[i - 1] = new Segment(beginPoint, endPoint);
        }

        // Наименьшая абсцисса среди всех отрезков и индекс отрезка с этой абсциссой.
        int minAbscissa = int.MaxValue, indexMinAbsc = 0;

        // Наименьшая абсцисса среди всех отрезков и индекс отрезка с этой абсциссой.
        int maxAbscissa = int.MinValue, indexMaxAbsc = countPoints - 1;

        // Поиск наименьшей и наибольшей абсцисс среди всех отрезков и соответствующих им индексов отрезков.
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].BeginPoint < minAbscissa)
            {
                minAbscissa = segments[i].BeginPoint;
                indexMinAbsc = i;
            }

            if (segments[i].EndPoint > maxAbscissa)
            {
                maxAbscissa = segments[i].EndPoint;
                indexMaxAbsc = i;
            }
        }

        Segment tempSegment;


        // Перемещение отрезка с наименьшей абсциссой в начало массива.
        tempSegment = segments[0];
        segments[0] = segments[indexMinAbsc];
        segments[indexMinAbsc] = tempSegment;

        // Перемещение отрезка с наибольшей абсциссой в конец массива.
        tempSegment = segments[countPoints - 1];
        segments[countPoints - 1] = segments[indexMaxAbsc];
        segments[indexMaxAbsc] = tempSegment;

        // Суммарная длина тени.
        int totalSum;
        CalculateTotalSum(out totalSum, segments, countPoints, minAbscissa, maxAbscissa);
        Console.WriteLine($"\nСуммарная длина тени всех отрезков: {totalSum}");

    }


}
