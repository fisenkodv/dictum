package net.fisenko.dictum.core.service;

import net.fisenko.dictum.core.model.Author;
import net.fisenko.dictum.core.model.Quote;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public interface AuthorService {

    Flux<Author> searchAuthors(String language, @Nullable String query, int limit, int offset);

    Mono<Author> getAuthor(String language, String id);

    Flux<Quote> searchAuthorQuotes(String language, String id, @Nullable String query, int limit, int offset);
}
