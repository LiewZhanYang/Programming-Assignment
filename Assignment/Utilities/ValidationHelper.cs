//==========================================================
// Student Number : S10259432
// Student Name : Liew Zhan Yang
// Partner Number : S10257777
// Partner Name : Amicus Lee Ming Ge
//==========================================================

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assignment
{
    /// <summary>
    /// Static helper class for input validation across the application
    /// Provides consistent validation rules and error messages
    /// </summary>
    public static class ValidationHelper
    {
        #region Name Validation

        /// <summary>
        /// Validates if a name is acceptable for customer registration
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var trimmedName = name.Trim();

            // Check length constraints
            if (trimmedName.Length < 2 || trimmedName.Length > 50)
                return false;

            // Check for valid characters (letters, spaces, hyphens, apostrophes)
            var namePattern = @"^[a-zA-Z\s\-']+$";
            if (!Regex.IsMatch(trimmedName, namePattern))
                return false;

            // Check that it's not all spaces or special characters
            if (!trimmedName.Any(char.IsLetter))
                return false;

            // Check for reasonable word count (1-4 words)
            var words = trimmedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 1 || words.Length > 4)
                return false;

            // Check each word has reasonable length
            if (words.Any(word => word.Length < 1 || word.Length > 20))
                return false;

            return true;
        }

        /// <summary>
        /// Gets a descriptive error message for invalid names
        /// </summary>
        /// <param name="name">The invalid name</param>
        /// <returns>Specific error message</returns>
        public static string GetNameValidationError(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name cannot be empty.";

            var trimmedName = name.Trim();

            if (trimmedName.Length < 2)
                return "Name must be at least 2 characters long.";

            if (trimmedName.Length > 50)
                return "Name cannot exceed 50 characters.";

            var namePattern = @"^[a-zA-Z\s\-']+$";
            if (!Regex.IsMatch(trimmedName, namePattern))
                return "Name can only contain letters, spaces, hyphens, and apostrophes.";

            if (!trimmedName.Any(char.IsLetter))
                return "Name must contain at least one letter.";

            var words = trimmedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 4)
                return "Name cannot have more than 4 words.";

            if (words.Any(word => word.Length > 20))
                return "Individual name parts cannot exceed 20 characters.";

            return "Invalid name format.";
        }

        #endregion

        #region Member ID Validation

        /// <summary>
        /// Validates if a member ID is acceptable
        /// </summary>
        /// <param name="memberId">The member ID to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidMemberId(int memberId)
        {
            return memberId >= AppConstants.MIN_MEMBER_ID && memberId <= 999999;
        }

        /// <summary>
        /// Gets a descriptive error message for invalid member IDs
        /// </summary>
        /// <param name="memberId">The invalid member ID</param>
        /// <returns>Specific error message</returns>
        public static string GetMemberIdValidationError(int memberId)
        {
            if (memberId < AppConstants.MIN_MEMBER_ID)
                return $"Member ID must be at least {AppConstants.MIN_MEMBER_ID}.";

            if (memberId > 999999)
                return "Member ID cannot exceed 999999.";

            return "Invalid member ID.";
        }

        #endregion

        #region Date Validation

        /// <summary>
        /// Validates if a date of birth is acceptable
        /// </summary>
        /// <param name="dateOfBirth">The date of birth to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidDateOfBirth(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = CalculateAge(dateOfBirth, today);

            // Must be in the past
            if (dateOfBirth.Date >= today)
                return false;

            // Reasonable age constraints
            if (age < 0 || age > AppConstants.MAX_AGE_YEARS)
                return false;

            // Not too far in the past (reasonable birth year)
            if (dateOfBirth.Year < 1900)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a descriptive error message for invalid dates of birth
        /// </summary>
        /// <param name="dateOfBirth">The invalid date of birth</param>
        /// <returns>Specific error message</returns>
        public static string GetDateOfBirthValidationError(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = CalculateAge(dateOfBirth, today);

            if (dateOfBirth.Date >= today)
                return "Date of birth must be in the past.";

            if (dateOfBirth.Year < 1900)
                return "Date of birth cannot be before year 1900.";

            if (age > AppConstants.MAX_AGE_YEARS)
                return $"Age cannot exceed {AppConstants.MAX_AGE_YEARS} years.";

            return "Invalid date of birth.";
        }

        /// <summary>
        /// Calculates age in years between two dates
        /// </summary>
        /// <param name="birthDate">The birth date</param>
        /// <param name="referenceDate">The reference date (usually today)</param>
        /// <returns>Age in years</returns>
        public static int CalculateAge(DateTime birthDate, DateTime referenceDate)
        {
            var age = referenceDate.Year - birthDate.Year;
            if (birthDate.Date > referenceDate.AddYears(-age))
                age--;
            return age;
        }

        #endregion

        #region Ice Cream Validation

        /// <summary>
        /// Validates if the number of scoops is acceptable
        /// </summary>
        /// <param name="scoops">Number of scoops</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidScoopCount(int scoops)
        {
            return scoops >= AppConstants.MIN_SCOOPS && scoops <= AppConstants.MAX_SCOOPS;
        }

        /// <summary>
        /// Validates if an ice cream option is valid
        /// </summary>
        /// <param name="option">The ice cream option</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidIceCreamOption(string option)
        {
            if (string.IsNullOrWhiteSpace(option))
                return false;

            return AppConstants.VALID_ICE_CREAM_OPTIONS.Contains(option, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validates if a waffle flavour is valid
        /// </summary>
        /// <param name="flavour">The waffle flavour</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidWaffleFlavour(string flavour)
        {
            if (string.IsNullOrWhiteSpace(flavour))
                return false;

            return AppConstants.VALID_WAFFLE_FLAVOURS.Contains(flavour, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region Points and Rewards Validation

        /// <summary>
        /// Validates if a points amount is valid for redemption
        /// </summary>
        /// <param name="points">Points to redeem</param>
        /// <param name="availablePoints">Available points</param>
        /// <param name="orderTotal">Order total for calculating maximum redeemable points</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidPointsRedemption(int points, int availablePoints, double orderTotal)
        {
            if (points <= 0)
                return false;

            if (points > availablePoints)
                return false;

            // Cannot redeem more points than the order is worth
            var maxRedeemablePoints = (int)(orderTotal / AppConstants.POINTS_TO_DOLLAR_RATIO);
            if (points > maxRedeemablePoints)
                return false;

            return true;
        }

        /// <summary>
        /// Validates if a membership tier is valid
        /// </summary>
        /// <param name="tier">The membership tier</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidMembershipTier(string tier)
        {
            if (string.IsNullOrWhiteSpace(tier))
                return false;

            var validTiers = new[] { AppConstants.TIER_ORDINARY, AppConstants.TIER_SILVER, AppConstants.TIER_GOLD };
            return validTiers.Contains(tier, StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region File and Data Validation

        /// <summary>
        /// Validates if a file path is safe and accessible
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return false;

            try
            {
                // Check for invalid characters
                var invalidChars = System.IO.Path.GetInvalidPathChars();
                if (filePath.Any(c => invalidChars.Contains(c)))
                    return false;

                // Check if path is rooted (absolute) - we prefer relative paths for security
                if (System.IO.Path.IsPathRooted(filePath))
                    return false;

                // Check file extension is CSV
                var extension = System.IO.Path.GetExtension(filePath);
                if (!extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if a CSV line has the expected number of fields
        /// </summary>
        /// <param name="csvLine">The CSV line to validate</param>
        /// <param name="expectedFieldCount">Expected number of fields</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidCsvLine(string csvLine, int expectedFieldCount)
        {
            if (string.IsNullOrWhiteSpace(csvLine))
                return false;

            var fields = csvLine.Split(',');
            return fields.Length >= expectedFieldCount;
        }

        #endregion

        #region Date String Validation

        /// <summary>
        /// Validates and parses a date string in the application's standard format
        /// </summary>
        /// <param name="dateString">The date string to validate</param>
        /// <param name="parsedDate">The parsed date if successful</param>
        /// <returns>True if valid and parsed successfully, false otherwise</returns>
        public static bool TryParseDate(string dateString, out DateTime parsedDate)
        {
            parsedDate = default;

            if (string.IsNullOrWhiteSpace(dateString))
                return false;

            try
            {
                parsedDate = DateTime.ParseExact(dateString, AppConstants.DATE_FORMAT, CultureInfo.InvariantCulture);
                return IsValidDateOfBirth(parsedDate);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if a year is reasonable for business operations
        /// </summary>
        /// <param name="year">The year to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidBusinessYear(int year)
        {
            var currentYear = DateTime.Now.Year;
            return year >= 2000 && year <= currentYear;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Sanitizes input by removing dangerous characters and trimming whitespace
        /// </summary>
        /// <param name="input">The input to sanitize</param>
        /// <returns>Sanitized input</returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Remove control characters and trim
            var sanitized = new string(input.Where(c => !char.IsControl(c) || char.IsWhiteSpace(c)).ToArray());
            return sanitized.Trim();
        }

        /// <summary>
        /// Validates if an email address has a basic valid format
        /// </summary>
        /// <param name="email">The email address to validate</param>
        /// <returns>True if valid format, false otherwise</returns>
        public static bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates if a phone number has a reasonable format
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate</param>
        /// <returns>True if valid format, false otherwise</returns>
        public static bool IsValidPhoneNumberFormat(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove common separators for validation
            var cleaned = Regex.Replace(phoneNumber, @"[\s\-\(\)\.]+", "");

            // Check if it contains only digits and optional plus sign at start
            var phonePattern = @"^\+?\d{8,15}$";
            return Regex.IsMatch(cleaned, phonePattern);
        }

        #endregion
    }
}