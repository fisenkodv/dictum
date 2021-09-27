package net.fisenko.dictum.core.business.service;

import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.model.domain.Author;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public class AuthorService {

    private final AuthorRepository authorRepository;

    public AuthorService(AuthorRepository authorRepository) {
        this.authorRepository = authorRepository;
    }

    public Flux<Author> searchAuthors(String language, @Nullable String query, int limit, int offset) {
        return authorRepository.getAuthors(language, query, limit, offset);
    }

    public Mono<Author> getAuthor(String language, String id) {
        return authorRepository.getAuthor(language, id);
    }
}
