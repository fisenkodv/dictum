package net.fisenko.dictum.business.service;

import net.fisenko.dictum.core.data.LanguageRepository;
import net.fisenko.dictum.core.model.Language;
import net.fisenko.dictum.core.service.LanguageService;
import reactor.core.publisher.Flux;

public class LanguageServiceImpl implements LanguageService {

    private final LanguageRepository languageRepository;

    public LanguageServiceImpl(LanguageRepository languageRepository) {
        this.languageRepository = languageRepository;
    }

    @Override
    public Flux<Language> getLanguages() {
        return languageRepository.getLanguages();
    }
}
