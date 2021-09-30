package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Filters;
import com.mongodb.reactivestreams.client.FindPublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.business.mapping.MappingService;
import net.fisenko.dictum.core.configuration.DatabaseConfiguration;
import net.fisenko.dictum.core.data.LanguageRepository;
import net.fisenko.dictum.core.model.domain.Language;
import net.fisenko.dictum.data.mongo.entity.LanguageEntity;
import reactor.core.publisher.Flux;

public class MongoLanguageRepository implements LanguageRepository {

    private final MappingService mappingService;
    private final DatabaseConfiguration databaseConfiguration;
    private final MongoClient mongoClient;

    public MongoLanguageRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        this.mappingService = mappingService;
        this.databaseConfiguration = databaseConfiguration;
        this.mongoClient = mongoClient;
    }

    @Override
    public Flux<Language> getLanguages() {
        final FindPublisher<LanguageEntity> result = getCollection().find(Filters.empty());

        return Flux.from(result).map(x -> mappingService.map(x, Language.class));
    }

    private MongoCollection<LanguageEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(LanguageEntity.COLLECTION_NAME, LanguageEntity.class);
    }
}
