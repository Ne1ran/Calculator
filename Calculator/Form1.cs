namespace Calculator
{
    public partial class Calculator : Form
    {
        private string _currentOperation = string.Empty; // ���� ��� ������� ��������
        private List<string> _operationsArr = new List<string>() { "/", "*", "-", "+" }; // ������ �������������� ��������

        private bool _blocked; // ���������� ��� �������� �������� ��������
        private bool _hasInputNumber;
        private bool _hasOperationSign;
        private bool _isProcessableOperation;

        public Calculator()
        {
            InitializeComponent(); // �������������
        }

        private void eval_button_Click(object sender, EventArgs e) // ��������� ������ ����������
        {
            if (_blocked) // ����� � ���� ��������, ����� �� ���������� ������ �� ������ ��� �� ������
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
            try // ������� �� �������������� ��������
            {
                result = CalculateEvaluation(); // �������
                result_text = result.ToString();
            }
            catch (Exception)
            {
                _blocked = true; // ������� ����� �� ������ � ��������� ����������� �� ������
                result_text = "������ ��� ����������!";
            }
            finally
            {
                _hasInputNumber = true; // ���������� ��������� ����� ���������
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            Reset(); // ������� ������������
        }

        private void delete_button_Click(object sender, EventArgs e) // ������ ��������
        {
            if (_blocked) // ��������, ����� �� ������� ��������� ������
            {
                return;
            }

            if (_currentOperation == string.Empty)
            {
                return;
            }

            output_field.Text = output_field.Text.Substring(0, output_field.Text.Length - 1).Trim(); // ��������
            _currentOperation = _currentOperation.Substring(0, _currentOperation.Length - 1).Trim();

            if (_currentOperation.Length == 0)
            {
                _hasInputNumber = false; // ���������� ���� ��� ��������
                _isProcessableOperation = false;
                _hasOperationSign = false;
                return;
            }

            string[] changedOperationArr = _currentOperation.Split(' '); // ����� � ���� ���������, ��� �������� ����� �������� �������� � ����� �� ������� ������

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

            if (operationsCount == 0) // ���������� ��������� ����� �� ������ ��������
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

        private void divide_button_Click(object sender, EventArgs e) // ����� � ���� ������������ �������������� ��������
        {
            AddOperationSign(" / "); // ��������� �������� �� ����� �������� �� �����, ��� �������� �����
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

        private void digit0_button_Click(object sender, EventArgs e) // ���� �����
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
            if (_blocked) // ��������� ����� � ������
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
            if (_blocked) // ��������� ����� � ������
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
            _currentOperation += operation; // ��������� ������ � ������
            output_field.Text = _currentOperation;
        }

        private void Reset()
        {
            _blocked = false; // ����� ������ � ����������
            _currentOperation = string.Empty;
            output_field.Text = string.Empty;
        }
        private float CalculateEvaluation() // ������� ������� ������
        {
            float result = 0f;
            string[] operationArray = _currentOperation.Split(' '); // ������ �� �� �������
            string operationToDo = string.Empty;
            for (int i = 0; i < operationArray.Length; i++)
            {
                string operation = operationArray[i];
                if (i == 0)
                {
                    result = float.Parse(operation); // ������ ������ �������� ����� �����
                    continue;
                }

                if (_operationsArr.Contains(operation)) // ���������� ��������
                {
                    operationToDo = operation;
                    continue;
                }

                var operationNumber = float.Parse(operation); // ������ ����� � �����
                if (operationToDo == string.Empty) // ���� �������� �����-�� ������� �� ���������, �� ������ ������
                {
                    throw new Exception();
                }

                switch (operationToDo) // ������������ ������ ��������
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

        private void first_func_button_Click(object sender, EventArgs e) // ��������� ������� �� ������ ������ �������
        {
            if (_currentOperation == string.Empty) // �������� �� ����������� ��������
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
            try // ��������� �������������� �������� ��� �����������
            {
                result = Math.Pow(number, 3) * (Math.Cos(number) / Math.Sin(number)); // ��������� ���������
                result_text = Math.Round(result, 4).ToString();
            }
            catch (Exception)
            {
                _blocked = true;
                result_text = "������ ��� ����������!";
            }
            finally
            {
                _hasInputNumber = true; // ������ ����� � ������� ���������
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }

        private void second_func_button_Click(object sender, EventArgs e) // ��������� ������� �� ������ ������ �������
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
            { // ��������� �������������� �������� ��� �����������
                double step1_result = Math.Pow(number + 1, (1 / 3)) / (Math.Pow(number, 2) + 1); // ������������� ����������
                double step2_result = Math.Pow(Math.Pow(number, 2) - 2 * number, (1 / 3)) / (Math.Pow(number, 2) - 1);
                result = step1_result - step2_result; // ��������� ���������
                result_text = Math.Round(result, 4).ToString();
            }
            catch (Exception)
            {
                _blocked = true;
                result_text = "������ ��� ����������!";
            }
            finally
            {
                _hasInputNumber = true; // ����� ���������� + �����
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }

        private void third_func_button_Click(object sender, EventArgs e) // ��������� ������� �� ������ ������� �������
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
            { // ��������� �������������� �������� ��� �����������
                double step1_result = Math.Pow(number, (3 / 2)) * number / 2; // ������������� ����������
                double step2_result = Math.Pow(number, 2) / 2 * Math.Acos(number);
                result = step1_result + step2_result; // ��������� ���������
                result_text = Math.Round(result, 4).ToString();
            }
            catch (Exception)
            {
                _blocked = true;
                result_text = "������ ��� ����������!";
            }
            finally
            {
                _hasInputNumber = true; // ����� ����������� � �����
                _hasOperationSign = false;
                _isProcessableOperation = false;
                _currentOperation = result.ToString();
                output_field.Text = result_text;
            }
        }
    }
}
