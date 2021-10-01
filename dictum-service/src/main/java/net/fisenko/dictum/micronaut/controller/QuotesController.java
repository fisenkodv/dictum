package net.fisenko.dictum.micronaut.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.core.annotation.Nullable;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.business.annotation.Id;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.service.QuotesService;
import net.fisenko.dictum.micronaut.binding.Search;
import net.fisenko.dictum.micronaut.dto.quote.QuoteSummary;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import javax.validation.Valid;

@Slf4j
@Validated
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

    @Get("{id}")
    public Mono<QuoteSummary> getQuote(@PathVariable @NonNull String language, @PathVariable @NonNull @Id String id) {
        return quotesService.getQuote(language, id)
                            .map(x -> mappingService.map(x, QuoteSummary.class));
    }
}
