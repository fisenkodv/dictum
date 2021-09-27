package net.fisenko.dictum.micronaut.bean;

import com.mongodb.reactivestreams.client.MongoClient;
import io.micronaut.context.annotation.Factory;
import jakarta.inject.Singleton;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.data.mongo.repository.MongoAuthorRepository;
import net.fisenko.dictum.core.data.AuthorRepository;

@Factory
public final class RepositoryFactory {

    @Singleton
    public AuthorRepository authorRepository(MappingService mappingService, MongoClient mongoClient) {
        return new MongoAuthorRepository(mappingService, mongoClient);
    }
}
