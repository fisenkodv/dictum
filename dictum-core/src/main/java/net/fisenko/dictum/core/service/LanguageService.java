package net.fisenko.dictum.core.service;

import net.fisenko.dictum.core.model.Language;
import reactor.core.publisher.Flux;

public interface LanguageService {

    Flux<Language> getLanguages();
}
