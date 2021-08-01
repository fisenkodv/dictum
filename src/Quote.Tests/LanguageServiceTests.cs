using Dictum.Business.Models.Domain;
using Dictum.Business.Services;
using Xunit;

namespace Dictum.Tests
{
    public class LanguageServiceTests
    {
        [Theory]
        [InlineData("Nothing is softer or more flexible than water, yet nothing can resist it.", "EN")]
        [InlineData("На вершину горы взбираются не для того, чтобы увидеть небо, а чтобы увидеть равнину.", "RU")]
        public async void It_should_detect_language(string input, string languageCode)
        {
            LanguageService langugeService = new LanguageService(new FakeLanguageRepository());
            Language detectedLanguage = await langugeService.Detect(input);

            Assert.Equal(detectedLanguage.Code, languageCode);
        }
    }
}
