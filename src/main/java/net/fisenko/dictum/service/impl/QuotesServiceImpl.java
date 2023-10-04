package net.fisenko.dictum.service.impl;

import jakarta.inject.Singleton;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.model.domain.Quote;
import net.fisenko.dictum.repository.AuthorRepository;
import net.fisenko.dictum.repository.QuoteRepository;
import net.fisenko.dictum.service.QuotesService;

@Slf4j
@Singleton
public class QuotesServiceImpl implements QuotesService {

    private final QuoteRepository quoteRepository;
    private final AuthorRepository authorRepository;

    public QuotesServiceImpl(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        this.quoteRepository = quoteRepository;
        this.authorRepository = authorRepository;
    }

//    @Override
//    public Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset) {
//        return quoteRepository.searchQuotes(language, query, limit, offset);
//    }
//
//    @Override
//    public Mono<Quote> getRandomQuote(String language) {
//        return quoteRepository.getRandomQuote(language);
//    }
//
//    @Override
//    public Mono<Quote> getQuote(String language, String quoteId) {
//        return quoteRepository.getQuote(language, quoteId)
//                .filterWhen(Reactive::isEmpty)
//                .switchIfEmpty(Mono.error(new ResourceNotFoundException("quote.get.not_found.error", language, quoteId)))
//                .flatMap(Mono::just);
//    }
//
//    @Override
//    public Mono<Quote> createQuote(String language, String authorId, String text) {
//        return authorRepository.getAuthor(language, authorId)
//                .filterWhen(Reactive::isEmpty)
//                .switchIfEmpty(Mono.error(new ResourceNotFoundException("author.get.not_found.error", language, authorId)))
//                .flatMap(x -> quoteRepository.createQuote(language, authorId, text));
//    }
//
//    @Override
//    public Mono<Void> likeQuote(String language, String quoteId) {
//        return quoteRepository.likeQuote(language, quoteId)
//                .filterWhen(Mono::just)
//                .switchIfEmpty(Mono.error(new ResourceNotFoundException("quote.like.not_found.error", language, quoteId)))
//                .flatMap(x -> Mono.empty());
//    }

    @Override
    public Quote getRandomQuote(Long languageId) {
        return quoteRepository.getRandomQuote(languageId);
    }

    @Override
    public Quote getQuote(Long quoteId) {
        return quoteRepository.getQuote(quoteId);
    }
}
