package net.fisenko.dictum.core.business.service;

import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.domain.Quote;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

@Slf4j
public class QuotesService {

    private final QuoteRepository quoteRepository;

    public QuotesService(QuoteRepository quoteRepository) {
        this.quoteRepository = quoteRepository;
    }

    public Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset) {
        return quoteRepository.searchQuotes(language, query, limit, offset);
    }

    public Mono<Quote> getRandomQuote(String language) {
        return quoteRepository.getRandomQuote(language);
    }

    public Mono<Quote> getQuote(String language, String id) {
        return quoteRepository.getQuote(language, id);
    }
}
