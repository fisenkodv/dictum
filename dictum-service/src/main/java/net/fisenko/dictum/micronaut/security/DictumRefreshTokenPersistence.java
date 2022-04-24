package net.fisenko.dictum.micronaut.security;

import io.micronaut.security.authentication.Authentication;
import io.micronaut.security.errors.OauthErrorResponseException;
import io.micronaut.security.token.event.RefreshTokenGeneratedEvent;
import io.micronaut.security.token.refresh.RefreshTokenPersistence;
import jakarta.inject.Singleton;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.business.service.LocalizationService;
import net.fisenko.dictum.business.util.Reactive;
import net.fisenko.dictum.core.data.RefreshTokenRepository;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.FluxSink;
import reactor.core.publisher.Mono;

import static io.micronaut.security.errors.IssuingAnAccessTokenErrorCode.INVALID_GRANT;

@Slf4j
@Singleton
public class DictumRefreshTokenPersistence implements RefreshTokenPersistence {

    private final RefreshTokenRepository refreshTokenRepository;

    public DictumRefreshTokenPersistence(RefreshTokenRepository refreshTokenRepository) {
        this.refreshTokenRepository = refreshTokenRepository;
    }

    @Override
    public void persistToken(RefreshTokenGeneratedEvent event) {
        if (event != null &&
                event.getRefreshToken() != null &&
                event.getAuthentication() != null &&
                event.getAuthentication().getName() != null) {
            final String refreshToken = event.getRefreshToken();
            refreshTokenRepository.createRefreshToken(event.getAuthentication().getName(), refreshToken, false).block();
        }
    }

    public Publisher<Authentication> getAuthentication(String refreshToken) {
        return refreshTokenRepository
                .getRefreshToken(refreshToken)
                .filterWhen(Reactive::isEmpty)
                .switchIfEmpty(Mono.create(emitter ->
                                           {
                                               final String message = LocalizationService.getMessage("refresh_token.not_found.error");
                                               final OauthErrorResponseException exception = new OauthErrorResponseException(INVALID_GRANT, message, null);
                                               emitter.error(exception);
                                           }
                ))
                .flux()
                .flatMap(foundRefreshToken ->
                                 Flux.create(emitter -> {
                                                 if (foundRefreshToken.isRevoked()) {
                                                     final String message = LocalizationService.getMessage("refresh_token.revoked.error");
                                                     final OauthErrorResponseException exception = new OauthErrorResponseException(INVALID_GRANT, message, null);
                                                     emitter.error(exception);
                                                 } else {
                                                     emitter.next(Authentication.build(foundRefreshToken.getUserName()));
                                                     emitter.complete();
                                                 }
                                             },
                                             FluxSink.OverflowStrategy.ERROR));
    }
}
