package net.fisenko.dictum.core.data;

import net.fisenko.dictum.core.model.Quote;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public interface QuoteRepository {

    Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset);

    Mono<Quote> getRandomQuote(String language);

    Mono<Quote> getQuote(String language, String id);

    Flux<Quote> searchAuthorQuotes(String language, String id, @Nullable String query, int limit, int offset);

    Mono<Long> getQuotesCount(String language);
}
