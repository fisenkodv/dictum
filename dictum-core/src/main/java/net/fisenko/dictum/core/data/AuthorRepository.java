package net.fisenko.dictum.core.data;

import net.fisenko.dictum.core.model.Author;
import org.jetbrains.annotations.Nullable;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

public interface AuthorRepository {

    Flux<Author> getAuthors(String language, @Nullable String query, int limit, int offset);

    Mono<Author> getAuthor(String language, String authorId);

    Mono<Long> getAuthorsCount(String language);
}
