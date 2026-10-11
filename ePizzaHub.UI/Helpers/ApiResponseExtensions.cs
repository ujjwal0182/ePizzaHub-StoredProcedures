using System.Net.Http.Json;

namespace ePizzaHub.UI.Helpers
{
    public static class ApiResponseExtensions
    {
        private class ApiProblem
        {
            public string? Title { get; set; }
            public string? Detail { get; set; }
        }

        public static async Task<string> ReadErrorAsync(this HttpResponseMessage response)
        {
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ApiProblem>();
                return problem?.Detail ?? problem?.Title ?? "Something went wrong.";
            }
            catch
            {
                return "Something went wrong.";
            }
        }
    }
}