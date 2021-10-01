package net.fisenko.dictum.micronaut.controller;

import io.micronaut.core.annotation.NonNull;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.http.annotation.PathVariable;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.service.StatisticsService;
import net.fisenko.dictum.micronaut.dto.statistics.StatisticsDetail;
import reactor.core.publisher.Mono;

@Slf4j
@Validated
@Controller("/${dictum.api.version}/statistics/{language}")
public class StatisticsController {

    private final MappingService mappingService;
    private final StatisticsService statisticsService;

    public StatisticsController(MappingService mappingService, StatisticsService statisticsService) {
        this.mappingService = mappingService;
        this.statisticsService = statisticsService;
    }

    @Get
    public Mono<StatisticsDetail> searchQuotes(@PathVariable @NonNull String language) {
        return statisticsService.getStatistics(language)
                                .map(x -> mappingService.map(x, StatisticsDetail.class));
    }
}
