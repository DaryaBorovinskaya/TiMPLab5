


namespace Task2
{
    /// <summary>
    /// ПОЛИЗ (польская инверсная запись)
    /// </summary>
    public class POLIZ
    {
        private string _inputLine;
        private string _polizLine;
        private Stack<string> _operations = new();
        public string InputLine => _inputLine;
        public string PolizLine => _polizLine;

        /// <summary>
        /// Преобразование таких операций, как **, унарный + и унарный -, для размещения в стеке
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        private string ConvertSpecialOperationForStack(string operation)
        {
            switch (operation) 
            {
                // Буквы d от слова degree (степень).
                case "**":
                    return "d**d";

                // Буквы u от слова unary (унарный).
                case "+":
                    return "u+u";

                case "-":
                    return "u-u";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Преобразование таких операций, как **, унарный + и унарный -, из строк в стеке
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        private string ConvertSpecialOperationFromStack(string operation)
        {
            switch (operation)
            {
                case "d**d":
                    return "**";
                case "u+u":
                    return "+";
                case "u-u":
                    return "-";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Получение приоритета операции
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public int GetPriority(string operation)
        {
            switch(operation) 
            {
                case "(":
                    return 0;
                case ")": 
                    return 1;
                case "+":
                    return 2;
                case "-": 
                    return 2;
                case "*":
                    return 3;
                case ":":
                    return 3;
                case "d**d":      
                    return 4;
                case "u+u":
                    return 5;
                case "u-u":
                    return 5;
            }
            return -1;
        }

        /// <summary>
        /// Проверка на корректные символы в математическом выражении.
        /// </summary>
        /// <param name="inputLine"></param>
        /// <returns></returns>
        public bool IsRightExpression(string inputLine)
        {
            if(inputLine == string.Empty)
                return false;

            for(int i = 0; i < inputLine.Length; i++) 
            {
                if (inputLine[i] >= '0' && inputLine[i] <= '9')
                    continue;
                if (i == 0 && (inputLine[i] == ')' || inputLine[i] == '*' || inputLine[i] == ':'))
                    return false;
                if (i == inputLine.Length -1 && inputLine[i] == '*')
                    return false;
                if (inputLine[i] > '9' && inputLine[i] != ':')
                    return false;
                if (inputLine[i] < '0' && inputLine[i] != '+' && inputLine[i] != '-' 
                    && inputLine[i] != '(' && inputLine[i] != ')' && inputLine[i] != '*')
                    return false;
            }

            _inputLine = inputLine;
            return true;
        }

        /// <summary>
        /// Перевод математического выражения в ПОЛИЗ  
        /// </summary>
        public void ConvertToPOLIZline()
        {
            string result = string.Empty;
            string operationDegree;
            int countUntilBracket = 1, copyCountUntilBracket = 0;
            for (int i = 0; i < _inputLine.Length;i++)
            {
                // Число добавляется сразу в итоговую строку (минуя стек).
                if (_inputLine[i] >= '0' && _inputLine[i] <= '9')
                    result += _inputLine[i];

                else
                {
                    // Если стек пустой.
                    if (_operations.Count == 0)
                    {
                        if (_inputLine[i] == '(' || _inputLine[i] == ':')
                            _operations.Push(_inputLine[i].ToString());
                        else
                        {
                            // Если первый символ в строке это оператор.
                            if (i == 0)
                            {
                                if (_inputLine[i] == '+' || _inputLine[i] == '-')
                                    _operations.Push(ConvertSpecialOperationForStack(_inputLine[i].ToString()));
                            }
                            // Если оператор в строке это не последний символ в строке.
                            else if (i != _inputLine.Length - 1)
                            {
                                if(i < _inputLine.Length - 1 && _inputLine[i+1] == '*')
                                    _operations.Push(ConvertSpecialOperationForStack(_inputLine[i].ToString() + _inputLine[i+1].ToString()));
                                else
                                    _operations.Push(_inputLine[i].ToString());
                            }
                        }
                    }

                    // Если стек не пустой.
                    else if (_operations.Count >= 1)
                    {
                        if (_inputLine[i] == '(')
                            _operations.Push(_inputLine[i].ToString());

                        if (_inputLine[i] == ')')
                        {
                            countUntilBracket = 1;
                            copyCountUntilBracket = 0;
                            // Определяем количество символом до ( включительно
                            for (int l = 0; _operations.ElementAt(l) != "(" &&  l < _operations.Count;l++)
                                countUntilBracket+=1;
                            

                            copyCountUntilBracket = countUntilBracket - 1;
                            for (int l = 0; _operations.ElementAt(l) != "(" && l < _operations.Count; l++)
                            {
                                if (copyCountUntilBracket > 0)
                                {
                                    if (_operations.ElementAt(l)[0] == 'd' || _operations.ElementAt(l)[0] == 'u')
                                        result += ConvertSpecialOperationFromStack(_operations.ElementAt(l));
                                    else
                                        result += _operations.ElementAt(l);
                                    copyCountUntilBracket--;
                                }
                            };

                            for (int j = 0; j < countUntilBracket; j++)
                                _operations.Pop();

                        }

                        if (_inputLine[i] == '+' || _inputLine[i] == '-' || _inputLine[i] == ':')
                        {
                            // Если приоритет текущей операции больше находящейся в стеке.
                            if (GetPriority(_inputLine[i].ToString()) > GetPriority(_operations.Peek()))
                                _operations.Push(_inputLine[i].ToString());

                            // Если приоритет текущей операции не больше находящейся в стеке.
                            else
                            {
                                foreach (string op in _operations)
                                {
                                    if (op[0] == 'd' || op[0] == 'u')
                                        result += ConvertSpecialOperationFromStack(op);
                                    else
                                        result += op;
                                }

                                _operations.Clear();
                                _operations.Push(_inputLine[i].ToString());
                            }

                        }

                        if (_inputLine[i] == '*')
                        {
                            // Если текущий оператор это **.
                            if (_inputLine[i - 1] == '*')
                                continue;
                            
                            if (i< _inputLine.Length-1 && _inputLine[i+1] == '*')
                            {
                                operationDegree = ConvertSpecialOperationForStack(_inputLine[i].ToString() + _inputLine[i + 1].ToString());

                                // Если приоритет текущей операции больше находящейся в стеке.
                                if (GetPriority(operationDegree) > GetPriority(_operations.Peek()))
                                    _operations.Push(operationDegree);

                                // Если приоритет текущей операции не больше находящейся в стеке.
                                else
                                {
                                    foreach (string op in _operations)
                                    {
                                        if (op[0] == 'd' || op[0] == 'u')
                                            result += ConvertSpecialOperationFromStack(op);
                                        else
                                            result += op;
                                    }

                                    _operations.Clear();
                                    _operations.Push(operationDegree);
                                }
                            }

                            // Если текущий оператор это *.
                            else if(i < _inputLine.Length - 1 && _inputLine[i + 1] != '*')
                            {
                                // Если приоритет текущей операции больше находящейся в стеке.
                                if (GetPriority(_inputLine[i].ToString()) > GetPriority(_operations.Peek()))
                                    _operations.Push(_inputLine[i].ToString());

                                // Если приоритет текущей операции не больше находящейся в стеке.
                                else
                                {
                                    foreach (string op in _operations)
                                    {
                                        if (op[0] == 'd' || op[0] == 'u')
                                            result += ConvertSpecialOperationFromStack(op);
                                        else
                                            result += op;
                                    }

                                    _operations.Clear();
                                    _operations.Push(_inputLine[i].ToString());
                                }
                            }
                        }
                    }
                }
            }

            if(_operations.Count > 0) 
            { 
                foreach (string op in _operations) 
                {
                    if (op[0] == 'd' || op[0] == 'u')
                        result += ConvertSpecialOperationFromStack(op);
                    else
                        result += op;
                }
                _operations.Clear();
            }

            _polizLine = result;
        }

    }
}
