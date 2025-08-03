//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;

namespace Assignment
{
    /// <summary>
    /// Helper class for professional console user interface operations
    /// Provides consistent formatting, color coding, and user input handling
    /// </summary>
    public class ConsoleHelper
    {
        #region Constants
        private const int DEFAULT_WIDTH = 60;
        private const char HEADER_CHAR = '=';
        private const char SEPARATOR_CHAR = '-';
        #endregion

        #region Display Methods

        /// <summary>
        /// Displays a formatted section header
        /// </summary>
        /// <param name="title">The header title</param>
        /// <param name="width">Width of the header line</param>
        public void DisplaySectionHeader(string title, int width = DEFAULT_WIDTH)
        {
            Console.WriteLine();
            Console.WriteLine(new string(HEADER_CHAR, width));
            Console.WriteLine($"{title.ToUpper().PadLeft((width + title.Length) / 2)}");
            Console.WriteLine(new string(HEADER_CHAR, width));
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a sub-section separator
        /// </summary>
        /// <param name="title">Optional title for the separator</param>
        /// <param name="width">Width of the separator line</param>
        public void DisplaySeparator(string title = null, int width = DEFAULT_WIDTH)
        {
            Console.WriteLine();
            if (!string.IsNullOrEmpty(title))
            {
                Console.WriteLine($"--- {title} ---");
            }
            else
            {
                Console.WriteLine(new string(SEPARATOR_CHAR, width));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a success message in green
        /// </summary>
        /// <param name="message">The success message</param>
        public void DisplaySuccessMessage(string message)
        {
            DisplayColoredMessage("✅ SUCCESS: " + message, ConsoleColor.Green);
        }

        /// <summary>
        /// Displays an error message in red
        /// </summary>
        /// <param name="message">The error message</param>
        public void DisplayErrorMessage(string message)
        {
            DisplayColoredMessage("❌ ERROR: " + message, ConsoleColor.Red);
        }

        /// <summary>
        /// Displays a warning message in yellow
        /// </summary>
        /// <param name="message">The warning message</param>
        public void DisplayWarningMessage(string message)
        {
            DisplayColoredMessage("⚠️  WARNING: " + message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Displays an information message in cyan
        /// </summary>
        /// <param name="message">The information message</param>
        public void DisplayInfoMessage(string message)
        {
            DisplayColoredMessage("ℹ️  INFO: " + message, ConsoleColor.Cyan);
        }

        /// <summary>
        /// Displays a message in the specified color
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="color">The console color to use</param>
        private void DisplayColoredMessage(string message, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }

        /// <summary>
        /// Displays a progress indicator
        /// </summary>
        /// <param name="message">The progress message</param>
        public void DisplayProgress(string message)
        {
            Console.Write($"⏳ {message}... ");
        }

        /// <summary>
        /// Completes a progress indicator
        /// </summary>
        /// <param name="success">Whether the operation was successful</param>
        public void CompleteProgress(bool success = true)
        {
            Console.WriteLine(success ? "✅ Done!" : "❌ Failed!");
        }

        #endregion

        #region Input Methods

        /// <summary>
        /// Gets user input with a prompt
        /// </summary>
        /// <param name="prompt">The input prompt</param>
        /// <returns>User input string</returns>
        public string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// Gets a valid integer input within specified range
        /// </summary>
        /// <param name="prompt">The input prompt</param>
        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (inclusive)</param>
        /// <param name="maxAttempts">Maximum number of attempts</param>
        /// <returns>Valid integer or null if failed</returns>
        public int? GetValidIntegerInput(string prompt, int min, int max, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    var input = GetUserInput($"{prompt}({min}-{max}): ");

                    if (int.TryParse(input, out int value))
                    {
                        if (value >= min && value <= max)
                        {
                            return value;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"Value must be between {min} and {max}.");
                        }
                    }
                    else
                    {
                        throw new FormatException("Please enter a valid number.");
                    }
                }
                catch (Exception ex)
                {
                    var remainingAttempts = maxAttempts - attempt;
                    if (remainingAttempts > 0)
                    {
                        DisplayWarningMessage($"{ex.Message} {remainingAttempts} attempts remaining.");
                    }
                    else
                    {
                        DisplayErrorMessage("Maximum attempts exceeded.");
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a yes/no answer from the user
        /// </summary>
        /// <param name="prompt">The yes/no prompt</param>
        /// <param name="defaultValue">Default value if user just presses Enter</param>
        /// <returns>True for yes, false for no</returns>
        public bool GetYesNoInput(string prompt, bool? defaultValue = null)
        {
            string defaultText = defaultValue.HasValue
                ? (defaultValue.Value ? " [Y/n]" : " [y/N]")
                : " [y/n]";

            while (true)
            {
                var input = GetUserInput(prompt + defaultText + ": ").ToLower();

                if (string.IsNullOrEmpty(input) && defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }

                switch (input)
                {
                    case "y":
                    case "yes":
                        return true;
                    case "n":
                    case "no":
                        return false;
                    default:
                        DisplayWarningMessage("Please enter 'y' for yes or 'n' for no.");
                        break;
                }
            }
        }

        /// <summary>
        /// Gets user input with validation using a custom validator function
        /// </summary>
        /// <param name="prompt">The input prompt</param>
        /// <param name="validator">Function to validate the input</param>
        /// <param name="errorMessage">Error message for invalid input</param>
        /// <param name="maxAttempts">Maximum number of attempts</param>
        /// <returns>Valid input or null if failed</returns>
        public string GetValidatedInput(string prompt, Func<string, bool> validator,
            string errorMessage, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                var input = GetUserInput(prompt);

                if (validator(input))
                {
                    return input;
                }

                var remainingAttempts = maxAttempts - attempt;
                if (remainingAttempts > 0)
                {
                    DisplayWarningMessage($"{errorMessage} {remainingAttempts} attempts remaining.");
                }
                else
                {
                    DisplayErrorMessage("Maximum attempts exceeded.");
                }
            }

            return null;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Pauses execution and waits for user to press any key
        /// </summary>
        /// <param name="message">Optional message to display</param>
        public void PauseForUser(string message = "Press any key to continue...")
        {
            Console.WriteLine();
            Console.Write(message);
            Console.ReadKey(true);
            Console.WriteLine();
        }

        /// <summary>
        /// Clears the console screen
        /// </summary>
        public void ClearScreen()
        {
            try
            {
                Console.Clear();
            }
            catch
            {
                // If clear fails, just add some blank lines
                Console.WriteLine(new string('\n', 10));
            }
        }

        /// <summary>
        /// Displays a loading animation for the specified duration
        /// </summary>
        /// <param name="message">Loading message</param>
        /// <param name="durationMs">Duration in milliseconds</param>
        public void ShowLoadingAnimation(string message, int durationMs = 1000)
        {
            Console.Write($"{message} ");

            var chars = new[] { '|', '/', '-', '\\' };
            var end = DateTime.Now.AddMilliseconds(durationMs);

            int charIndex = 0;
            while (DateTime.Now < end)
            {
                Console.Write(chars[charIndex]);
                System.Threading.Thread.Sleep(100);
                Console.Write('\b');
                charIndex = (charIndex + 1) % chars.Length;
            }

            Console.WriteLine("✅");
        }

        /// <summary>
        /// Formats and displays a table with headers and data
        /// </summary>
        /// <param name="headers">Column headers</param>
        /// <param name="data">Data rows</param>
        /// <param name="columnWidths">Width for each column</param>
        public void DisplayTable(string[] headers, string[][] data, int[] columnWidths)
        {
            if (headers == null || data == null || columnWidths == null)
                throw new ArgumentNullException("Table parameters cannot be null");

            if (headers.Length != columnWidths.Length)
                throw new ArgumentException("Headers and column widths must have the same length");

            // Display headers
            Console.WriteLine();
            for (int i = 0; i < headers.Length; i++)
            {
                Console.Write($"{headers[i],-columnWidths[i]}");
            }
            Console.WriteLine();

            // Display separator
            var totalWidth = columnWidths.Sum();
            Console.WriteLine(new string('=', totalWidth));

            // Display data
            foreach (var row in data)
            {
                for (int i = 0; i < Math.Min(row.Length, columnWidths.Length); i++)
                {
                    var cellValue = row[i] ?? "";
                    if (cellValue.Length > columnWidths[i] - 1)
                    {
                        cellValue = cellValue.Substring(0, columnWidths[i] - 4) + "...";
                    }
                    Console.Write($"{cellValue,-columnWidths[i]}");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a centered message with decorative borders
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="width">Total width of the display</param>
        /// <param name="borderChar">Character to use for borders</param>
        public void DisplayCenteredMessage(string message, int width = DEFAULT_WIDTH, char borderChar = '*')
        {
            Console.WriteLine();
            Console.WriteLine(new string(borderChar, width));

            var padding = (width - message.Length - 2) / 2;
            var leftPadding = padding;
            var rightPadding = width - message.Length - leftPadding - 2;

            Console.WriteLine($"{borderChar}{new string(' ', leftPadding)}{message}{new string(' ', rightPadding)}{borderChar}");
            Console.WriteLine(new string(borderChar, width));
            Console.WriteLine();
        }

        /// <summary>
        /// Gets a selection from a list of options
        /// </summary>
        /// <param name="options">List of options to choose from</param>
        /// <param name="prompt">Prompt message</param>
        /// <returns>Selected option index or -1 if cancelled</returns>
        public int GetMenuSelection(string[] options, string prompt = "Please select an option:")
        {
            if (options == null || options.Length == 0)
                throw new ArgumentException("Options cannot be null or empty");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(prompt);
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"{i + 1,3}. {options[i]}");
                }
                Console.WriteLine($"{options.Length + 1,3}. Cancel");
                Console.WriteLine();

                var selection = GetValidIntegerInput("Enter your choice", 1, options.Length + 1);

                if (selection.HasValue)
                {
                    if (selection.Value == options.Length + 1)
                    {
                        DisplayInfoMessage("Selection cancelled.");
                        return -1;
                    }
                    return selection.Value - 1;
                }

                DisplayWarningMessage("Please make a valid selection.");
            }
        }

        #endregion
    }
}