namespace qBitTrackerUpdater
{
    public static class Utils
    {
        /// <summary>
        /// Splits a string of trackers into an array.
        /// </summary>
        public static string[] Split(string input, bool escape)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return [];
            }
            if (escape)
            {
                return [.. input.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
            }
            else
            {
                return [.. input.Split([@"\n", @"\r"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
            }
        }

        /// <summary>
        /// Joins an array of trackers into a single string.
        /// </summary>
        public static string Join(string[] input, bool escape)
        {
            if (input == null || input.Length == 0)
            {
                return string.Empty;
            }
            if (escape)
            {
                return string.Join('\n', input);
            }
            else
            {
                return string.Join(@"\n", input);
            }
        }
    }
}
