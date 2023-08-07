package net.fisenko.dictum.micronaut.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.core.annotation.Nullable;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.http.annotation.Post;
import io.micronaut.security.annotation.Secured;
import io.micronaut.security.rules.SecurityRule;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.business.annotation.Id;
import net.fisenko.dictum.core.security.SecurityRoles;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.service.QuotesService;
import net.fisenko.dictum.micronaut.binding.Search;
import net.fisenko.dictum.micronaut.dto.quote.CreateQuote;
import net.fisenko.dictum.micronaut.dto.quote.QuoteSummary;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import jakarta.validation.Valid;

@Slf4j
@Validated
@Secured(SecurityRule.IS_ANONYMOUS)
@Controller("/${dictum.api.version}/quotes/{language}")
public class QuotesController {

    private final MappingService mappingService;
    private final QuotesService quotesService;

    public QuotesController(MappingService mappingService, QuotesService quotesService) {
        this.mappingService = mappingService;
        this.quotesService = quotesService;
    }

    @Get("{?search*}")
    public Flux<QuoteSummary> searchQuotes(@PathVariable @NonNull String language, @Nullable @Valid Search search) {
        return quotesService.searchQuotes(language, Search.getQuery(search), Search.getLimit(search), Search.getOffset(search))
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }

    @Get("random")
    public Mono<QuoteSummary> getRandomQuote(@PathVariable @NonNull String language) {
        return quotesService.getRandomQuote(language)
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }

    @Get("{quoteId}")
    public Mono<QuoteSummary> getQuote(@PathVariable @NonNull String language, @PathVariable @NonNull @Id String quoteId) {
        return quotesService.getQuote(language, quoteId)
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }

    @Post()
    @Secured({SecurityRoles.EDITOR})
    public Mono<QuoteSummary> createQuote(@PathVariable @NonNull String language, @Valid CreateQuote quote) {
        return quotesService.createQuote(language, quote.getAuthorId(), quote.getText())
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }
}
