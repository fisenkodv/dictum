package net.fisenko.dictum.micronaut.bean;

import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.business.service.AuthorServiceImpl;
import net.fisenko.dictum.business.service.LanguageServiceImpl;
import net.fisenko.dictum.business.service.MappingServiceImpl;
import net.fisenko.dictum.business.service.QuotesServiceImpl;
import net.fisenko.dictum.business.service.StatisticsServiceImpl;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.LanguageRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.service.AuthorService;
import net.fisenko.dictum.core.service.LanguageService;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.service.QuotesService;
import net.fisenko.dictum.core.service.StatisticsService;

@Factory
public final class ServiceFactory {

    @Singleton
    public MappingService mappingService() {
        return new MappingServiceImpl();
    }

    @Singleton
    public AuthorService authorService(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        return new AuthorServiceImpl(authorRepository, quoteRepository);
    }

    @Singleton
    public LanguageService languageService(LanguageRepository languageRepository) {
        return new LanguageServiceImpl(languageRepository);
    }

    @Singleton
    public QuotesService quotesService(QuoteRepository quoteRepository) {
        return new QuotesServiceImpl(quoteRepository);
    }

    @Singleton
    public StatisticsService quotesService(AuthorRepository authorRepository, QuoteRepository quoteRepository) {
        return new StatisticsServiceImpl(authorRepository, quoteRepository);
    }
}
