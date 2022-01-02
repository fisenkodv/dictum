package net.fisenko.dictum.core.service;

import net.fisenko.dictum.core.model.Quote;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public interface QuotesService {

    Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset);

    Mono<Quote> getRandomQuote(String language);

    Mono<Quote> getQuote(String language, String quoteId);

    Mono<Void> likeQuote(String language, String quoteId);
}
