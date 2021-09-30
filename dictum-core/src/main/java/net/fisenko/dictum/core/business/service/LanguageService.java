package net.fisenko.dictum.core.business.service;

import net.fisenko.dictum.core.data.LanguageRepository;
import net.fisenko.dictum.core.model.domain.Language;
import reactor.core.publisher.Flux;

public class LanguageService {

    private final LanguageRepository languageRepository;

    public LanguageService(LanguageRepository languageRepository) {
        this.languageRepository = languageRepository;
    }

    public Flux<Language> getLanguages() {
        return languageRepository.getLanguages();
    }
}
