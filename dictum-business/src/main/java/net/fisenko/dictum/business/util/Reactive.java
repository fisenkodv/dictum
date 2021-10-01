package net.fisenko.dictum.business.util;

import reactor.core.publisher.Mono;

public final class Reactive {

    private Reactive() {
    }

    public static <T> Mono<Boolean> isEmpty(T obj) {
        return obj == null ? Mono.empty() : Mono.just(true);
    }
}
