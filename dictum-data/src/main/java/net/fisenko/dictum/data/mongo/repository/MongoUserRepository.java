package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Filters;
import com.mongodb.reactivestreams.client.FindPublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.config.DatabaseConfiguration;
import net.fisenko.dictum.core.data.UserRepository;
import net.fisenko.dictum.core.model.User;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.data.mongo.entity.UserEntity;
import reactor.core.publisher.Mono;

public class MongoUserRepository implements UserRepository {

    private final MappingService mappingService;
    private final DatabaseConfiguration databaseConfiguration;
    private final MongoClient mongoClient;

    public MongoUserRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        this.mappingService = mappingService;
        this.databaseConfiguration = databaseConfiguration;
        this.mongoClient = mongoClient;
    }

    @Override
    public Mono<User> getUser(String username) {
        final FindPublisher<UserEntity> result = getCollection().find(Filters.eq(UserEntity.USER_NAME_FIELD_NAME, username))
                                                                .limit(1);

        return Mono.from(result).map(userEntity -> mappingService.map(userEntity, User.class));
    }

    private MongoCollection<UserEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(UserEntity.COLLECTION_NAME, UserEntity.class);
    }
}

