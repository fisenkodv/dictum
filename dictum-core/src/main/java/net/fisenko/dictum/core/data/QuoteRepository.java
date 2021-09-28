package net.fisenko.dictum.core.data;

import net.fisenko.dictum.core.model.domain.Quote;
import reactor.core.publisher.Mono;

public interface QuoteRepository {

    Mono<Quote> getRandomQuote(String language);
}
