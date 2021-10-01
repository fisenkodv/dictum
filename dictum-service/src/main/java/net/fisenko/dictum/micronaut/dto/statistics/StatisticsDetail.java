package net.fisenko.dictum.micronaut.dto.statistics;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public final class StatisticsDetail {

    private long authors;
    private long quotes;
}
