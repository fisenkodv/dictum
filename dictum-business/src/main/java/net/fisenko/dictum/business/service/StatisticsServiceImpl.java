package net.fisenko.dictum.business.service;

import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.Statistics;
import net.fisenko.dictum.core.service.StatisticsService;
import reactor.core.publisher.Mono;

@Slf4j
public class StatisticsServiceImpl implements StatisticsService {

    private final AuthorRepository authorRepository;
    private final QuoteRepository quoteRepository;

    public StatisticsServiceImpl(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        this.authorRepository = authorRepository;
        this.quoteRepository = quoteRepository;
    }

    @Override
    public Mono<Statistics> getStatistics(String language) {
        return Mono.zip(authorRepository.getAuthorsCount(language),
                        quoteRepository.getQuotesCount(language),
                        Statistics::new);
    }
}
