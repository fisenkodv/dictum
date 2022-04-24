package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Filters;
import com.mongodb.reactivestreams.client.FindPublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.config.DatabaseConfiguration;
import net.fisenko.dictum.core.data.RefreshTokenRepository;
import net.fisenko.dictum.core.model.RefreshToken;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.data.mongo.entity.RefreshTokenEntity;
import org.bson.types.ObjectId;
import reactor.core.publisher.Mono;

import java.util.Objects;

public class RefreshTokenRepositoryImpl implements RefreshTokenRepository {

    private final MappingService mappingService;
    private final DatabaseConfiguration databaseConfiguration;
    private final MongoClient mongoClient;

    public RefreshTokenRepositoryImpl(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {

        this.mappingService = mappingService;
        this.databaseConfiguration = databaseConfiguration;
        this.mongoClient = mongoClient;
    }

    @Override
    public Mono<RefreshToken> createRefreshToken(String userName, String refreshToken, boolean revoked) {
        final RefreshTokenEntity refreshTokenEntity = new RefreshTokenEntity();
        refreshTokenEntity.setUserName(userName);
        refreshTokenEntity.setRefreshToken(refreshToken);
        refreshTokenEntity.setRevoked(revoked);

        return Mono.from(getCollection().insertOne(refreshTokenEntity))
                   .map(insertOneResult -> {
                       final ObjectId insertedId = Objects.requireNonNull(insertOneResult.getInsertedId()).asObjectId().getValue();
                       refreshTokenEntity.setId(insertedId);
                       return mappingService.map(refreshTokenEntity, RefreshToken.class);
                   });
    }

    @Override
    public Mono<RefreshToken> getRefreshToken(String refreshToken) {
        final FindPublisher<RefreshTokenEntity> result = getCollection().find(Filters.eq(RefreshTokenEntity.REFRESH_TOKEN_FIELD_NAME, refreshToken))
                                                                        .limit(1);
        return Mono.from(result).map(x -> mappingService.map(x, RefreshToken.class));
    }

    private MongoCollection<RefreshTokenEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(RefreshTokenEntity.COLLECTION_NAME, RefreshTokenEntity.class);
    }
}
