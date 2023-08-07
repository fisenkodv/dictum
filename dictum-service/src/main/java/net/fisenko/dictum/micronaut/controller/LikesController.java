package net.fisenko.dictum.micronaut.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.http.annotation.Post;
import io.micronaut.security.annotation.Secured;
import io.micronaut.security.rules.SecurityRule;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.security.SecurityRoles;
import net.fisenko.dictum.core.service.QuotesService;
import net.fisenko.dictum.micronaut.dto.like.LikeQuote;
import reactor.core.publisher.Mono;

import jakarta.validation.Valid;

@Slf4j
@Validated
@Secured(SecurityRule.IS_ANONYMOUS)
@Controller("/${dictum.api.version}/likes/{language}")
public class LikesController {

    private final QuotesService quotesService;

    public LikesController(QuotesService quotesService) {
        this.quotesService = quotesService;
    }

    @Post
    @Secured({SecurityRoles.EDITOR})
    public Mono<Void> likeQuote(@PathVariable @NonNull String language, @Valid LikeQuote likeQuote) {
        return quotesService.likeQuote(language, likeQuote.getQuoteId());
    }
}
