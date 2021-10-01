package net.fisenko.dictum.core.service;

import net.fisenko.dictum.core.model.Statistics;
import reactor.core.publisher.Mono;

public interface StatisticsService {

    Mono<Statistics> getStatistics(String language);
}
