package net.fisenko.dictum.core.data;

import net.fisenko.dictum.core.model.RefreshToken;
import reactor.core.publisher.Mono;

public interface RefreshTokenRepository {

    Mono<RefreshToken> createRefreshToken(String userName, String refreshToken, boolean revoked);

    Mono<RefreshToken> getRefreshToken(String refreshToken);
}
