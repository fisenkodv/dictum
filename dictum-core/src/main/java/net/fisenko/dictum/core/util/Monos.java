package net.fisenko.dictum.core.util;

import reactor.core.publisher.Mono;

public final class Monos {

    private Monos() {
    }

    public static <T> Mono<Boolean> isEmpty(T obj) {
        return obj == null ? Mono.empty() : Mono.just(true);
    }
}
