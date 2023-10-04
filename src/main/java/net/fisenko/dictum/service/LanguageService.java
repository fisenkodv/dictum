package net.fisenko.dictum.service;

import net.fisenko.dictum.model.domain.Language;

import java.util.Collection;

public interface LanguageService {
    Collection<Language> getLanguages();
}
