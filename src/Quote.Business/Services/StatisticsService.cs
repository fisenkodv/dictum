using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models.Domain;

namespace Dictum.Business.Services
{
    public class StatisticsService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IQuoteRepository _quoteRepository;

        public StatisticsService(IAuthorRepository authorRepository, IQuoteRepository quoteRepository)
        {
            _authorRepository = authorRepository;
            _quoteRepository = quoteRepository;
        }

        public async Task<Statistics> GetStatistics()
        {
            var authorStatistics = await _authorRepository.GetStatistics();
            var quotesStatistics = await _quoteRepository.GetStatistics();

            return new Statistics {Authors = authorStatistics, Quotes = quotesStatistics};
        }
    }
}