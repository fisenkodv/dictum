package net.fisenko.dictum.micronaut.bean;

import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.business.mapping.impl.MappingServiceImpl;
import net.fisenko.dictum.core.business.service.AuthorService;
import net.fisenko.dictum.core.business.service.QuotesService;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.QuoteRepository;

@Factory
public final class ServiceFactory {

    @Singleton
    public MappingService mappingService() {
        return new MappingServiceImpl();
    }

    @Singleton
    public AuthorService authorService(AuthorRepository authorRepository) {
        return new AuthorService(authorRepository);
    }

    @Singleton
    public QuotesService quotesService(QuoteRepository quoteRepository) {
        return new QuotesService(quoteRepository);
    }
}
