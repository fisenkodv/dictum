package net.fisenko.dictum.controller;

import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import io.micronaut.security.annotation.Secured;
import io.micronaut.security.rules.SecurityRule;
import io.micronaut.validation.Validated;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.model.dto.LanguageSummary;
import net.fisenko.dictum.service.LanguageService;
import net.fisenko.dictum.service.MappingService;

import java.util.Collection;

@Slf4j
@Validated
@Secured(SecurityRule.IS_ANONYMOUS)
@Controller("/${dictum.api.version}/languages")
public class LanguagesController {

    private final MappingService mappingService;
    private final LanguageService languageService;

    public LanguagesController(MappingService mappingService, LanguageService languageService) {
        this.mappingService = mappingService;
        this.languageService = languageService;
    }

    @Get
    public Collection<LanguageSummary> getLanguages() {
        return mappingService.map(languageService.getLanguages(), LanguageSummary.class);
    }
}
