package net.fisenko.dictum.service;

import net.fisenko.dictum.model.domain.Quote;

public interface QuotesService {

//    Collection<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset);

    Quote getRandomQuote(Long languageId);

    Quote getQuote(Long quoteId);

//    Quote createQuote(String language, String authorId, String text);
}
