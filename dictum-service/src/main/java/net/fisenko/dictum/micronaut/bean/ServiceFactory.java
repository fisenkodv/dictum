package net.fisenko.dictum.micronaut.bean;

import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.business.mapping.impl.MappingServiceImpl;
import net.fisenko.dictum.core.business.service.AuthorService;
import net.fisenko.dictum.core.data.AuthorRepository;

@Factory
public final class ServiceFactory {

    @Singleton
    public AuthorService authorService(AuthorRepository authorRepository) {
        return new AuthorService(authorRepository);
    }

    @Singleton
    public MappingService mappingService() {
        return new MappingServiceImpl();
    }
}
