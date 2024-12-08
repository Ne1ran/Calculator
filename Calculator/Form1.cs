namespace Calculator
{
    public partial class Calculator : Form
    {
        private string _currentOperation = string.Empty; // Поле для текущей операции
        private List<string> _operationsArr = new List<string>() { "/", "*", "-", "+" }; // Список математических операций

        private bool _blocked; // Переменные для контроля вводимых операций
        private bool _hasInputNumber;
        private bool _hasOperationSign;
        private bool _isProcessableOperation;

        public Calculator()
        {
            InitializeComponent(); // Инициализация
        }

        private void eval_button_Click(object sender, EventArgs e) // Обработка кнопки вычисления
        {
            if (_blocked) // Здесь и ниже проверки, может ли пользовать нажать на кнопку для ее работы
            {
                return;
            }

            if (!_hasInputNumber)
            {
                return;
            }

            if (!_isProcessableOperation)
            {
                return;
            }

            string result_text = string.Empty;
            float result = 0f;
            try // Обертка от исключительных ситуаций
            {
                result = CalculateEvaluation(); // Подсчет
                result_text = result.ToString();
            }
            catch (Exception)
            {
                _blocked = true; // Выводим текст об ошибке и блокируем калькулятор до сброса
                result_text = "Ошибка при вычислении!";
            }
            finally
            {
                _hasInputNumber = true; // Сбрасываем параметры после подсчетов
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            Reset(); // Очистка калькулятора
        }

        private void delete_button_Click(object sender, EventArgs e) // Кнопка удаления
        {
            if (_blocked) // Проверки, можем ли стереть последнюю запись
            {
                return;
            }

            if (_currentOperation == string.Empty)
            {
                return;
            }

            output_field.Text = output_field.Text.Substring(0, output_field.Text.Length - 1).Trim(); // Стирание
            _currentOperation = _currentOperation.Substring(0, _currentOperation.Length - 1).Trim();

            if (_currentOperation.Length == 0)
            {
                _hasInputNumber = false; // Выставляем поля для проверок
                _isProcessableOperation = false;
                _hasOperationSign = false;
                return;
            }

            string[] changedOperationArr = _currentOperation.Split(' '); // Здесь и ниже проверяем, что осталось после операции удаления и можем ли считать дальше

            int digitsCount = 0;
            int operationsCount = 0;

            for (int i = changedOperationArr.Length - 1; i >= 0; i--)
            {
                var operation = changedOperationArr[i];
                if (_operationsArr.Contains(operation))
                {
                    operationsCount++;
                }

                if (float.TryParse(operation, out float number))
                {
                    digitsCount++;
                }
            }

            if (operationsCount == 0) // Выставляем различные флаги по итогам проверок
            {
                _hasOperationSign = false;
                _isProcessableOperation = false;
            }

            if (digitsCount == 1)
            {

                _isProcessableOperation = false;
            }

            if (digitsCount > 1)
            {
                _isProcessableOperation = true;
            }

        }

        private void divide_button_Click(object sender, EventArgs e) // Здесь и ниже обрабатываем математические операции
        {
            AddOperationSign(" / "); // Добавляем проблемы по обоим сторонам от знака, для парсинга далее
        }

        private void multiply_button_Click(object sender, EventArgs e)
        {
            AddOperationSign(" * ");
        }

        private void subtract_button_Click(object sender, EventArgs e)
        {
            AddOperationSign(" - ");
        }

        private void plus_button_Click(object sender, EventArgs e)
        {
            AddOperationSign(" + ");
        }

        private void digit0_button_Click(object sender, EventArgs e) // Ввод чисел
        {
            AddDigit("0");
        }

        private void digit1_button_Click(object sender, EventArgs e)
        {
            AddDigit("1");
        }

        private void digit2_button_Click(object sender, EventArgs e)
        {
            AddDigit("2");
        }

        private void digit3_button_Click(object sender, EventArgs e)
        {
            AddDigit("3");
        }

        private void digit4_button_Click(object sender, EventArgs e)
        {
            AddDigit("4");
        }

        private void digit5_button_Click(object sender, EventArgs e)
        {
            AddDigit("5");
        }

        private void digit6_button_Click(object sender, EventArgs e)
        {
            AddDigit("6");
        }

        private void digit7_button_Click(object sender, EventArgs e)
        {
            AddDigit("7");
        }

        private void digit8_button_Click(object sender, EventArgs e)
        {
            AddDigit("8");
        }

        private void digit9_button_Click(object sender, EventArgs e)
        {
            AddDigit("9");
        }

        private void AddOperationSign(string operationSign)
        {
            if (_blocked) // Обработка знака и флагов
            {
                return;
            }

            if (_hasInputNumber)
            {
                _hasOperationSign = true;
                AddOperationString(operationSign);
            }
        }

        private void AddDigit(string digit)
        {
            if (_blocked) // Обработка цифры и флагов
            {
                return;
            }

            _hasInputNumber = true;
            AddOperationString(digit);
            if (_hasOperationSign)
            {
                _isProcessableOperation = true;
            }
        }

        private void AddOperationString(string operation)
        {
            _currentOperation += operation; // Добавляем символ в строку
            output_field.Text = _currentOperation;
        }

        private void Reset()
        {
            _blocked = false; // Сброс текста и блокировок
            _currentOperation = string.Empty;
            output_field.Text = string.Empty;
        }
        private float CalculateEvaluation() // Считаем текущую строку
        {
            float result = 0f;
            string[] operationArray = _currentOperation.Split(' '); // Парсим ее по пробелу
            string operationToDo = string.Empty;
            for (int i = 0; i < operationArray.Length; i++)
            {
                string operation = operationArray[i];
                if (i == 0)
                {
                    result = float.Parse(operation); // Первая строка операции точно число
                    continue;
                }

                if (_operationsArr.Contains(operation)) // Записываем операцию
                {
                    operationToDo = operation;
                    continue;
                }

                var operationNumber = float.Parse(operation); // Парсим число в конце
                if (operationToDo == string.Empty) // Если операции каким-то образом не оказалось, то кидаем ошибку
                {
                    throw new Exception();
                }

                switch (operationToDo) // Обрабатываем символ операции
                {
                    case "/":
                        result /= operationNumber;
                        break;
                    case "-":
                        result -= operationNumber;
                        break;
                    case "*":
                        result *= operationNumber;
                        break;
                    case "+":
                        result += operationNumber;
                        break;
                }
            }

            return result;
        }

        private void first_func_button_Click(object sender, EventArgs e) // Считываем нажатие на кнопку первой функции
        {
            if (_currentOperation == string.Empty) // Проверки на возможность операции
            {
                return;
            }

            string[] operationArr = _currentOperation.Split(' ');
            if (operationArr.Length == 0)
            {
                return;
            }

            if (!float.TryParse(operationArr[0], out float number))
            {
                return;
            }

            string result_text = string.Empty;
            double result = 0f;
            try // Обработка исключительных ситуаций при вычислениях
            {
                result = Math.Pow(number, 3) * (Math.Cos(number) / Math.Sin(number)); // Вычисляем результат
                result_text = Math.Round(result, 4).ToString();
            }
            catch (Exception)
            {
                _blocked = true;
                result_text = "Ошибка при вычислении!";
            }
            finally
            {
                _hasInputNumber = true; // Меняем флаги и выводим результат
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }

        private void second_func_button_Click(object sender, EventArgs e) // Считываем нажатие на кнопку второй функции
        {
            if (_currentOperation == string.Empty)
            {
                return;
            }

            string[] operationArr = _currentOperation.Split(' ');
            if (operationArr.Length == 0)
            {
                return;
            }

            if (!float.TryParse(operationArr[0], out float number))
            {
                return;
            }

            string result_text = string.Empty;
            double result = 0f;
            try
            { // Обработка исключительных ситуаций при вычислениях
                double step1_result = Math.Pow(number + 1, (1 / 3)) / (Math.Pow(number, 2) + 1); // Промежуточные результаты
                double step2_result = Math.Pow(Math.Pow(number, 2) - 2 * number, (1 / 3)) / (Math.Pow(number, 2) - 1);
                result = step1_result - step2_result; // Вычисляем результат
                result_text = Math.Round(result, 4).ToString();
            }
            catch (Exception)
            {
                _blocked = true;
                result_text = "Ошибка при вычислении!";
            }
            finally
            {
                _hasInputNumber = true; // Вывод результата + флаги
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }

        private void third_func_button_Click(object sender, EventArgs e) // Считываем нажатие на кнопку третьей функции
        {
            if (_currentOperation == string.Empty)
            {
                return;
            }

            string[] operationArr = _currentOperation.Split(' ');
            if (operationArr.Length == 0)
            {
                return;
            }

            if (!float.TryParse(operationArr[0], out float number))
            {
                return;
            }

            string result_text = string.Empty;
            double result = 0f;
            try
            { // Обработка исключительных ситуаций при вычислениях
                double step1_result = Math.Pow(number, (3 / 2)) * number / 2; // Промежуточные результаты
                double step2_result = Math.Pow(number, 2) / 2 * Math.Acos(number);
                result = step1_result + step2_result; // Вычисляем результат
                result_text = Math.Round(result, 4).ToString();
            }
            catch (Exception)
            {
                _blocked = true;
                result_text = "Ошибка при вычислении!";
            }
            finally
            {
                _hasInputNumber = true; // Вывод результатов и флаги
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }
    }
}
