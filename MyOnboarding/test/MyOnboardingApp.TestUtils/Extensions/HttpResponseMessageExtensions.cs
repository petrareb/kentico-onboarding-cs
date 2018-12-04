using System.Net.Http;
using System.Threading.Tasks;

namespace MyOnboardingApp.TestUtils.Extensions
{
    // Use when comparing the content of response messages
    public static class HttpResponseMessageExtensions
    {
        public static async Task<string> GetLowerCaseTextFromMessageAsync(this HttpResponseMessage message)
            => (await message.Content.ReadAsStringAsync())?.ToLower();
    }
}