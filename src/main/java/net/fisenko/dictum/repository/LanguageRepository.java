package net.fisenko.dictum.repository;

import net.fisenko.dictum.model.domain.Language;

import java.util.Collection;

public interface LanguageRepository {

    Collection<Language> getLanguages();
}
