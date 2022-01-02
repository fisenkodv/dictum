package net.fisenko.dictum.business.service;

import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.business.exception.ResourceNotFoundException;
import net.fisenko.dictum.business.util.Reactive;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.Quote;
import net.fisenko.dictum.core.service.QuotesService;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

@Slf4j
public class QuotesServiceImpl implements QuotesService {

    private final QuoteRepository quoteRepository;

    public QuotesServiceImpl(QuoteRepository quoteRepository) {
        this.quoteRepository = quoteRepository;
    }

    @Override
    public Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset) {
        return quoteRepository.searchQuotes(language, query, limit, offset);
    }

    @Override
    public Mono<Quote> getRandomQuote(String language) {
        return quoteRepository.getRandomQuote(language);
    }

    @Override
    public Mono<Quote> getQuote(String language, String quoteId) {
        return quoteRepository.getQuote(language, quoteId)
                              .filterWhen(Reactive::isEmpty)
                              .switchIfEmpty(Mono.error(new ResourceNotFoundException("quote.get.not_found.error", language, quoteId)))
                              .flatMap(Mono::just);
    }

    @Override
    public Mono<Void> likeQuote(String language, String quoteId) {
        return quoteRepository.likeQuote(language, quoteId)
                              .filterWhen(Mono::just)
                              .switchIfEmpty(Mono.error(new ResourceNotFoundException("quote.like.not_found.error", language, quoteId)))
                              .flatMap(x -> Mono.empty());
    }
}
