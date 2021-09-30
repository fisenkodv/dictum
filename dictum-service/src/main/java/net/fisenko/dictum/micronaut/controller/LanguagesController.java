package net.fisenko.dictum.micronaut.controller;

import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.business.service.LanguageService;
import net.fisenko.dictum.core.model.dto.language.LanguageSummary;
import reactor.core.publisher.Flux;

@Slf4j
@Validated
@Controller("/${dictum.api.version}/languages")
public class LanguagesController {

    private final MappingService mappingService;
    private final LanguageService languageService;

    public LanguagesController(MappingService mappingService, LanguageService languageService) {
        this.mappingService = mappingService;
        this.languageService = languageService;
    }

    @Get
    public Flux<LanguageSummary> getLanguages() {
        return languageService.getLanguages()
                              .map(x -> mappingService.map(x, LanguageSummary.class));
    }
}
