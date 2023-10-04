package net.fisenko.dictum.service.impl;

import jakarta.inject.Singleton;
import net.fisenko.dictum.model.domain.Language;
import net.fisenko.dictum.repository.LanguageRepository;
import net.fisenko.dictum.service.LanguageService;

import java.util.Collection;

@Singleton
public class LanguageServiceImpl implements LanguageService {

    private final LanguageRepository languageRepository;

    public LanguageServiceImpl(LanguageRepository languageRepository) {
        this.languageRepository = languageRepository;
    }

    @Override
    public Collection<Language> getLanguages() {
        return languageRepository.getLanguages();
    }
}
