package net.fisenko.dictum.micronaut.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.core.annotation.Nullable;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.security.annotation.Secured;
import io.micronaut.security.rules.SecurityRule;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.business.annotation.Id;
import net.fisenko.dictum.core.service.AuthorService;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.micronaut.binding.Search;
import net.fisenko.dictum.micronaut.dto.author.AuthorDetail;
import net.fisenko.dictum.micronaut.dto.author.AuthorSummary;
import net.fisenko.dictum.micronaut.dto.quote.QuoteSummary;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import jakarta.validation.Valid;

@Slf4j
@Validated
@Secured(SecurityRule.IS_ANONYMOUS)
@Controller("/${dictum.api.version}/authors/{language}")
public class AuthorsController {

    private final MappingService mappingService;
    private final AuthorService authorService;

    public AuthorsController(MappingService mappingService, AuthorService authorService) {
        this.mappingService = mappingService;
        this.authorService = authorService;
    }

    @Get("{?search*}")
    public Flux<AuthorSummary> searchAuthors(@PathVariable @NonNull String language, @Nullable @Valid Search search) {
        return authorService.searchAuthors(language, Search.getQuery(search), Search.getLimit(search), Search.getOffset(search))
                            .map(x -> mappingService.map(x, AuthorSummary.class));
    }

    @Get("{authorId}")
    public Mono<AuthorDetail> getAuthor(@PathVariable @NonNull String language, @PathVariable @NonNull @Id String authorId) {
        return authorService.getAuthor(language, authorId)
                            .map(x -> mappingService.map(x, AuthorDetail.class));
    }

    @Get("{authorId}/quotes{?search*}")
    public Flux<QuoteSummary> searchAuthorQuotes(@PathVariable @NonNull String language, @PathVariable @NonNull @Id String authorId, @Nullable @Valid Search search) {
        return authorService.searchAuthorQuotes(language, authorId, Search.getQuery(search), Search.getLimit(search), Search.getOffset(search))
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }
}