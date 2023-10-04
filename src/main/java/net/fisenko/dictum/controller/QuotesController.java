package net.fisenko.dictum.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.http.annotation.QueryValue;
import io.micronaut.security.annotation.Secured;
import io.micronaut.security.rules.SecurityRule;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.model.domain.Quote;
import net.fisenko.dictum.model.dto.QuoteSummary;
import net.fisenko.dictum.service.MappingService;
import net.fisenko.dictum.service.QuotesService;

@Slf4j
@Validated
@Secured(SecurityRule.IS_ANONYMOUS)
@Controller("/${dictum.api.version}/quotes")
public class QuotesController {


    private final MappingService mappingService;
    private final QuotesService quotesService;

    public QuotesController(MappingService mappingService, QuotesService quotesService) {
        this.mappingService = mappingService;
        this.quotesService = quotesService;
    }

//    @Get("{?search*}")
//    public Flux<QuoteSummary> searchQuotes(@PathVariable @NonNull String language, @Nullable @Valid Search search) {
//        return quotesService.searchQuotes(language, Search.getQuery(search), Search.getLimit(search), Search.getOffset(search)).map(x -> mappingService.map(x, QuoteSummary.class));
//    }

    @Get("random")
    public QuoteSummary getRandomQuote(@NonNull @QueryValue Long language) {
        Quote quote = quotesService.getRandomQuote(language);
        return mappingService.map(quote, QuoteSummary.class);
    }

    @Get("{quoteId}")
    public QuoteSummary getQuote(@NonNull @PathVariable Long quoteId) {
        Quote quote = quotesService.getQuote(quoteId);
        return mappingService.map(quote, QuoteSummary.class);
    }

//    @Post()
//    @Secured({SecurityRoles.EDITOR})
//    public Mono<QuoteSummary> createQuote(@PathVariable @NonNull String language, @Valid CreateQuote quote) {
//        return quotesService.createQuote(language, quote.getAuthorId(), quote.getText()).map(x -> mappingService.map(x, QuoteSummary.class));
//    }
}
