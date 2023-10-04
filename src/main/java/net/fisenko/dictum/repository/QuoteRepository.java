package net.fisenko.dictum.repository;

import net.fisenko.dictum.model.domain.Quote;

public interface QuoteRepository {

//    Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset);

    Quote getRandomQuote(Long languageId);

    Quote getQuote(Long quoteId);
//
//    Flux<Quote> searchAuthorQuotes(String language, String authorId, @Nullable String query, int limit, int offset);
//
//    Mono<Long> getQuotesCount(String language);
//
//    Mono<Quote> createQuote(String language, String authorId, String text);
//
//    Mono<Boolean> likeQuote(String language, String quoteId);
}
