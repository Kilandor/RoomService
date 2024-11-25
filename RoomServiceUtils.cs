using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService
{
    public static class RoomServiceUtils
    {
        public static (bool, bool) ParseBoolFromString(string inputString)
        {
            bool result;
            bool success = bool.TryParse(inputString, out result);
            return (success, result);
        }

        public static (bool, float) ParseFloatFromString(string inputString)
        {
            float result;
            bool success = float.TryParse(inputString, out result);
            return (success, result);
        }

        public static (bool, int) ParseIntFromString(string inputString)
        {
            int result;
            bool success = int.TryParse(inputString, out result);
            return (success, result);
        }

        public static (bool, int[]) ParseIntArrayFromString(string inputString)
        {
            try
            {
                // Remove brackets and split by commas
                var trimmed = inputString.Trim('[', ']');
                if (string.IsNullOrWhiteSpace(trimmed))
                    return (true, Array.Empty<int>()); // Return an empty array if the input is "[]"

                // Split into parts and parse each to int
                var result = trimmed
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                return (true, result);
            }
            catch
            {
                // If any error occurs, return false and an empty array
                return (false, Array.Empty<int>());
            }
        }
    }
}
