package net.fisenko.dictum.micronaut.bean;

import com.mongodb.reactivestreams.client.MongoClient;
import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.core.config.DatabaseConfiguration;
import net.fisenko.dictum.core.data.AuthorRepository;
import net.fisenko.dictum.core.data.LanguageRepository;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.data.RefreshTokenRepository;
import net.fisenko.dictum.core.data.UserRepository;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.data.mongo.repository.MongoAuthorRepository;
import net.fisenko.dictum.data.mongo.repository.MongoLanguageRepository;
import net.fisenko.dictum.data.mongo.repository.MongoQuoteRepository;
import net.fisenko.dictum.data.mongo.repository.MongoUserRepository;
import net.fisenko.dictum.data.mongo.repository.RefreshTokenRepositoryImpl;
import net.fisenko.dictum.micronaut.configuration.DictumApplicationConfiguration;

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
    public DatabaseConfiguration databaseConfiguration(DictumApplicationConfiguration applicationConfiguration) {
        return applicationConfiguration.getDB();
    }

    @Singleton
    public UserRepository userRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        return new MongoUserRepository(mappingService, databaseConfiguration, mongoClient);
    }

    @Singleton
    public RefreshTokenRepository refreshTokenRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        return new RefreshTokenRepositoryImpl(mappingService, databaseConfiguration, mongoClient);
    }
}
