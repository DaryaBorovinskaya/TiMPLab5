using Task2;

POLIZ poliz = new();
string inputLine;
bool isRightExpression = true;

do
{
    // Проверка на корректный пользовательский ввод.
    Console.WriteLine("Введите символьное математическое выражение с целочисленными значениями.\nДоступные символы (,),+,-,*,:,** ");
    inputLine = Console.ReadLine();
    isRightExpression = poliz.IsRightExpression(inputLine);
    if (!isRightExpression)
        Console.WriteLine("ОШИБКА: введены некорректные символы.");
} while (!isRightExpression);

poliz.ConvertToPOLIZline();
// Вывод результата.
Console.WriteLine($"Результат ПОЛИЗ {poliz.PolizLine}");