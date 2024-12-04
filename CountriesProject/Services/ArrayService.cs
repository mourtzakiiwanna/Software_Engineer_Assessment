using Models;

namespace Services
{
    public class ArrayService
    {
        public int? GetSecondLargest(RequestObj request)
        {
            if (request?.RequestArrayObj == null || !request.RequestArrayObj.Any())
            {
                throw new ArgumentException("Request array cannot be null or empty.");
            }

            var distinctNumbers = request.RequestArrayObj.Distinct().OrderByDescending(n => n).ToList();

            if (distinctNumbers.Count < 2)
            { 
                throw new InvalidOperationException("Not enough distinct numbers to determine the second largest.");
            }

            return distinctNumbers[1];
        }
    }
}