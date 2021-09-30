package net.fisenko.dictum.core.business.service;

import net.fisenko.dictum.core.ResourceNotFoundException;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.domain.Author;
import net.fisenko.dictum.core.model.domain.Quote;
import net.fisenko.dictum.core.util.Monos;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public class AuthorService {

    private final AuthorRepository authorRepository;
    private final QuoteRepository quoteRepository;

    public AuthorService(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        this.authorRepository = authorRepository;
        this.quoteRepository = quoteRepository;
    }

    public Flux<Author> searchAuthors(String language, @Nullable String query, int limit, int offset) {
        return authorRepository.getAuthors(language, query, limit, offset);
    }

    public Mono<Author> getAuthor(String language, String id) {
        return authorRepository.getAuthor(language, id)
                               .filterWhen(Monos::isEmpty)
                               .switchIfEmpty(Mono.error(new ResourceNotFoundException("author.get.not_found.error", language, id)))
                               .flatMap(Mono::just);
    }

    public Flux<Quote> searchAuthorQuotes(String language, String id, @Nullable String query, int limit, int offset) {
        return quoteRepository.searchAuthorQuotes(language, id, query, limit, offset);
    }
}
