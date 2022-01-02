package net.fisenko.dictum.business.service;

import net.fisenko.dictum.business.exception.ResourceNotFoundException;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.Author;
import net.fisenko.dictum.core.model.Quote;
import net.fisenko.dictum.core.service.AuthorService;
import net.fisenko.dictum.business.util.Reactive;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public class AuthorServiceImpl implements AuthorService {

    private final AuthorRepository authorRepository;
    private final QuoteRepository quoteRepository;

    public AuthorServiceImpl(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        this.authorRepository = authorRepository;
        this.quoteRepository = quoteRepository;
    }

    @Override
    public Flux<Author> searchAuthors(String language, @Nullable String query, int limit, int offset) {
        return authorRepository.getAuthors(language, query, limit, offset);
    }

    @Override
    public Mono<Author> getAuthor(String language, String authorId) {
        return authorRepository.getAuthor(language, authorId)
                               .filterWhen(Reactive::isEmpty)
                               .switchIfEmpty(Mono.error(new ResourceNotFoundException("author.get.not_found.error", language, authorId)))
                               .flatMap(Mono::just);
    }

    @Override
    public Flux<Quote> searchAuthorQuotes(String language, String authorId, @Nullable String query, int limit, int offset) {
        return quoteRepository.searchAuthorQuotes(language, authorId, query, limit, offset);
    }
}
