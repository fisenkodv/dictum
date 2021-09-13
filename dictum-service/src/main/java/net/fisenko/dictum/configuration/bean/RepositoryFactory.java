package net.fisenko.dictum.configuration.bean;

import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.repository.AuthorRepository;

@Factory
public class RepositoryFactory {
    @Singleton
    public AuthorRepository authorRepository() {
        return new AuthorRepository() {
            @Override
            public String getAuthors() {
                return "Dmitry Fisenko";
            }
        };
    }
}
