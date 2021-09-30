package net.fisenko.dictum.micronaut.binding;

import io.micronaut.core.annotation.Introspected;
import io.micronaut.core.annotation.Nullable;
import io.micronaut.http.annotation.QueryValue;
import lombok.extern.slf4j.Slf4j;

@Slf4j
@Introspected
public record Search(@Nullable @QueryValue String query,
                     @Nullable @QueryValue Integer offset,
                     @Nullable @QueryValue Integer limit) {

    private final static int DEFAULT_LIMIT = 50;
    private final static int DEFAULT_OFFSET = 0;

    public static String getQuery(Search search) {
        return search == null ? null : search.query();
    }

    public static int getLimit(Search search) {
        int limit = search == null || search.limit() == null ? DEFAULT_LIMIT : search.limit();

        if (limit > DEFAULT_LIMIT || limit <= 0) {
            log.info("Limit was {}, setting to default value {}", limit, DEFAULT_LIMIT);
            limit = DEFAULT_LIMIT;
        }

        return limit;
    }

    public static int getOffset(Search search) {
        int offset = search == null || search.offset() == null ? DEFAULT_OFFSET : search.offset();

        if (offset < 0) {
            log.info("Offset was {}, setting to default value {}", offset, DEFAULT_OFFSET);
        }

        return offset;
    }
}
