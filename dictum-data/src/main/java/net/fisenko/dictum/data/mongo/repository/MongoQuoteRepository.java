package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Aggregates;
import com.mongodb.client.model.Field;
import com.mongodb.client.model.Filters;
import com.mongodb.client.model.TextSearchOptions;
import com.mongodb.reactivestreams.client.AggregatePublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.config.DatabaseConfiguration;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.Quote;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.util.Strings;
import net.fisenko.dictum.data.mongo.entity.AuthorEntity;
import net.fisenko.dictum.data.mongo.entity.QuoteEntity;
import net.fisenko.dictum.data.mongo.util.Expressions;
import net.fisenko.dictum.data.mongo.util.Fields;
import org.bson.conversions.Bson;
import org.jetbrains.annotations.Nullable;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

public class MongoQuoteRepository implements QuoteRepository {

    private final static int RANDOM_SAMPLE_SIZE = 1000;

    private final MappingService mappingService;
    private final DatabaseConfiguration databaseConfiguration;
    private final MongoClient mongoClient;

    public MongoQuoteRepository(MappingService mappingService, DatabaseConfiguration databaseConfiguration, MongoClient mongoClient) {
        this.mappingService = mappingService;
        this.databaseConfiguration = databaseConfiguration;
        this.mongoClient = mongoClient;
    }

    @Override
    public Flux<Quote> searchQuotes(String language, @Nullable String query, int limit, int offset) {
        final List<Bson> aggregates = new ArrayList<>();

        if (!Strings.isNullOrEmpty(query)) {
            aggregates.add(Aggregates.match(Filters.text(query, new TextSearchOptions().caseSensitive(false))));
        }

        aggregates.add(Aggregates.match(Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language)));
        aggregates.addAll(getPagingAggregationStages(limit, offset));
        aggregates.addAll(getAuthorLookupAggregationStages());

        final AggregatePublisher<QuoteEntity> result = getCollection().aggregate(aggregates);

        return Flux.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Mono<Quote> getRandomQuote(String language) {
        final List<Bson> aggregates = new ArrayList<>();

        aggregates.add(Aggregates.sample(RANDOM_SAMPLE_SIZE));
        aggregates.add(Aggregates.match(Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language)));
        aggregates.addAll(getAuthorLookupAggregationStages());
        aggregates.add(Aggregates.limit(1));

        final AggregatePublisher<QuoteEntity> result = getCollection().aggregate(aggregates);

        return Mono.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Mono<Quote> getQuote(String language, String id) {
        final List<Bson> aggregates = List.of(Aggregates.match(Filters.and(Filters.eq(Fields.UNDERSCORE_ID, Fields.getId(id)), Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language))),
                                              Aggregates.addFields(new Field<>(QuoteEntity.AUTHOR_ID_FIELD_NAME, Expressions.toObjectId(Fields.getFieldPath(QuoteEntity.AUTHOR_ID_FIELD_NAME)))),
                                              Aggregates.lookup(AuthorEntity.COLLECTION_NAME, QuoteEntity.AUTHOR_ID_FIELD_NAME, Fields.UNDERSCORE_ID, QuoteEntity.AUTHOR_FIELD_NAME),
                                              Aggregates.unwind(Fields.getFieldPath(QuoteEntity.AUTHOR_FIELD_NAME)));

        final AggregatePublisher<QuoteEntity> result = getCollection().aggregate(aggregates);

        return Mono.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Flux<Quote> searchAuthorQuotes(String language, String id, @Nullable String query, int limit, int offset) {
        final List<Bson> aggregates = new ArrayList<>();

        if (!Strings.isNullOrEmpty(query)) {
            aggregates.add(Aggregates.match(Filters.text(query, new TextSearchOptions().caseSensitive(false))));
        }

        aggregates.add(Aggregates.match(Filters.and(Filters.eq(QuoteEntity.AUTHOR_ID_FIELD_NAME, id), Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language))));
        aggregates.addAll(getPagingAggregationStages(limit, offset));
        aggregates.addAll(getAuthorLookupAggregationStages());

        final AggregatePublisher<QuoteEntity> result = getCollection().aggregate(aggregates);

        return Flux.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Mono<Long> getQuotesCount(String language) {
        final Publisher<Long> result = getCollection().countDocuments(Filters.eq(AuthorEntity.LANGUAGE_FIELD_NAME, language));

        return Mono.from(result);
    }

    private Collection<Bson> getAuthorLookupAggregationStages() {
        return List.of(
                Aggregates.addFields(new Field<>(QuoteEntity.AUTHOR_ID_FIELD_NAME, Expressions.toObjectId(Fields.getFieldPath(QuoteEntity.AUTHOR_ID_FIELD_NAME)))),
                Aggregates.lookup(AuthorEntity.COLLECTION_NAME, QuoteEntity.AUTHOR_ID_FIELD_NAME, Fields.UNDERSCORE_ID, QuoteEntity.AUTHOR_FIELD_NAME),
                Aggregates.unwind(Fields.getFieldPath(QuoteEntity.AUTHOR_FIELD_NAME))
        );
    }

    private Collection<Bson> getPagingAggregationStages(int limit, int offset) {
        return List.of(Aggregates.limit(limit), Aggregates.skip(offset));
    }

    private MongoCollection<QuoteEntity> getCollection() {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class);
    }
}
