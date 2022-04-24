package net.fisenko.dictum.core.data;

import net.fisenko.dictum.core.model.User;
import reactor.core.publisher.Mono;

public interface UserRepository {

    Mono<User> getUser(String username);
}
