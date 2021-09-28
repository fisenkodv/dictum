package net.fisenko.dictum.micronaut.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.core.annotation.Nullable;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.annotation.Id;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.business.service.AuthorService;
import net.fisenko.dictum.core.model.dto.author.AuthorDetail;
import net.fisenko.dictum.core.model.dto.author.AuthorSummary;
import net.fisenko.dictum.core.model.dto.quote.QuoteSummary;
import net.fisenko.dictum.micronaut.binding.Search;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import javax.validation.Valid;

@Slf4j
@Validated
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

    @Get("{id}")
    public Mono<AuthorDetail> getAuthor(@PathVariable @NonNull String language, @PathVariable @NonNull @Id String id) {
        return authorService.getAuthor(language, id)
                            .map(x -> mappingService.map(x, AuthorDetail.class));
    }

    @Get("{id}/quotes{?search*}")
    public Flux<QuoteSummary> searchAuthorQuotes(@PathVariable @NonNull String language, @PathVariable @NonNull @Id String id, @Nullable @Valid Search search) {
        return authorService.searchAuthorQuotes(language, id, Search.getQuery(search), Search.getLimit(search), Search.getOffset(search))
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }
}