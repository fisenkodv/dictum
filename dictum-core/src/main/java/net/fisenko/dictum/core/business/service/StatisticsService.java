package net.fisenko.dictum.core.business.service;

import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.domain.Statistics;
import reactor.core.publisher.Mono;

@Slf4j
public class StatisticsService {

    private final AuthorRepository authorRepository;
    private final QuoteRepository quoteRepository;

    public StatisticsService(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        this.authorRepository = authorRepository;
        this.quoteRepository = quoteRepository;
    }

    public Mono<Statistics> getStatistics(String language) {
        return Mono.zip(authorRepository.getAuthorsCount(language),
                        quoteRepository.getQuotesCount(language),
                        Statistics::new);
    }
}
