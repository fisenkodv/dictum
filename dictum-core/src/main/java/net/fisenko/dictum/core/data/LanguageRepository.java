package net.fisenko.dictum.core.data;

import net.fisenko.dictum.core.model.domain.Language;
import reactor.core.publisher.Flux;

public interface LanguageRepository {

    Flux<Language> getLanguages();
}
