package net.fisenko.dictum.micronaut.bean;

import com.mongodb.reactivestreams.client.MongoClient;
import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.configuration.DatabaseConfiguration;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.LanguageRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.data.mongo.repository.MongoAuthorRepository;
import net.fisenko.dictum.data.mongo.repository.MongoLanguageRepository;
import net.fisenko.dictum.data.mongo.repository.MongoQuoteRepository;
import net.fisenko.dictum.micronaut.configuration.MicronautApplicationConfiguration;

@Factory
public final class RepositoryFactory {

    @Singleton
    public AuthorRepository authorRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        return new MongoAuthorRepository(mappingService, databaseConfiguration, mongoClient);
    }

    @Singleton
    public QuoteRepository quoteRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        return new MongoQuoteRepository(mappingService, databaseConfiguration, mongoClient);
    }

    @Singleton
    public LanguageRepository languageRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        return new MongoLanguageRepository(mappingService, databaseConfiguration, mongoClient);
    }

    @Singleton
    public DatabaseConfiguration databaseConfiguration(MicronautApplicationConfiguration applicationConfiguration) {
        return applicationConfiguration.getDB();
    }
}
