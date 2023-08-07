package net.fisenko.dictum.micronaut.security;

import io.micronaut.core.annotation.Nullable;
import io.micronaut.security.authentication.AuthenticationProvider;
import io.micronaut.security.authentication.AuthenticationRequest;
import io.micronaut.security.authentication.AuthenticationResponse;
import jakarta.inject.Singleton;
import net.fisenko.dictum.business.util.Reactive;
import net.fisenko.dictum.core.data.UserRepository;
import net.fisenko.dictum.core.security.SecurityRoles;
import net.fisenko.dictum.core.service.SecurityService;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.FluxSink;
import reactor.core.publisher.Mono;

import java.util.List;

@Singleton
public class DictumAuthenticationProvider implements AuthenticationProvider {

    private final SecurityService securityService;
    private final UserRepository userRepository;

    public DictumAuthenticationProvider(SecurityService securityService, UserRepository userRepository) {
        this.securityService = securityService;
        this.userRepository = userRepository;
    }

    @Override
    public Publisher<AuthenticationResponse> authenticate(@Nullable Object httpRequest, AuthenticationRequest authenticationRequest) {
        final String username = authenticationRequest.getIdentity().toString();
        final String password = authenticationRequest.getSecret().toString();

        return userRepository
                .getUser(username)
                .filterWhen(Reactive::isEmpty)
                .switchIfEmpty(Mono.create(emitter -> emitter.error(AuthenticationResponse.exception())))
                .flux()
                .flatMap(user ->
                        Flux.create(emitter -> {
                                    if (securityService.verify(password, user.getPasswordHash())) {
                                        emitter.next(AuthenticationResponse.success((String) authenticationRequest.getIdentity(), List.of(SecurityRoles.EDITOR)));
                                        emitter.complete();
                                    } else {
                                        emitter.error(AuthenticationResponse.exception());
                                    }
                                },
                                FluxSink.OverflowStrategy.ERROR
                        ));
    }
}
