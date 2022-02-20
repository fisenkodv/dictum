package net.fisenko.dictum.data.mongo.repository;

import com.mongodb.client.model.Aggregates;
import com.mongodb.client.model.Field;
import com.mongodb.client.model.Filters;
import com.mongodb.client.model.Sorts;
import com.mongodb.client.model.TextSearchOptions;
import com.mongodb.client.model.UpdateOptions;
import com.mongodb.client.model.Updates;
import com.mongodb.reactivestreams.client.AggregatePublisher;
import com.mongodb.reactivestreams.client.MongoClient;
import com.mongodb.reactivestreams.client.MongoCollection;
import net.fisenko.dictum.core.config.DatabaseConfiguration;
import net.fisenko.dictum.core.data.QuoteRepository;
import net.fisenko.dictum.core.model.Quote;
import net.fisenko.dictum.core.service.MappingService;
import net.fisenko.dictum.core.util.Hash;
import net.fisenko.dictum.core.util.Strings;
import net.fisenko.dictum.data.mongo.entity.AuthorEntity;
import net.fisenko.dictum.data.mongo.entity.QuoteEntity;
import net.fisenko.dictum.data.mongo.util.Expressions;
import net.fisenko.dictum.data.mongo.util.Fields;
import org.bson.Document;
import org.bson.conversions.Bson;
import org.jetbrains.annotations.Nullable;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Objects;

public class MongoQuoteRepository implements QuoteRepository {

    private final static int RANDOM_SAMPLE_SIZE = 100;

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

        final AggregatePublisher<QuoteEntity> result = getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class).aggregate(aggregates);

        return Flux.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Mono<Quote> getRandomQuote(String language) {
        final List<Bson> topAuthorsAggregates = List.of(Aggregates.match(
                                                                Filters.and(
                                                                        Filters.eq(AuthorEntity.LANGUAGE_FIELD_NAME, language),
                                                                        Filters.ne(AuthorEntity.RANK_FIELD_NAME, null)
                                                                )
                                                        ),
                                                        Aggregates.sort(Sorts.descending(AuthorEntity.RANK_FIELD_NAME)),
                                                        Aggregates.limit(RANDOM_SAMPLE_SIZE),
                                                        Aggregates.sample(1)
        );

        final AggregatePublisher<AuthorEntity> topAuthorsAggregateResult = getCollection(AuthorEntity.COLLECTION_NAME, AuthorEntity.class).aggregate(topAuthorsAggregates);

        return Mono.from(topAuthorsAggregateResult)
                   .map(x -> x.getId().toString())
                   .flatMap(authorId -> {
                       final List<Bson> topAuthorQuotesAggregates = new ArrayList<>();
                       topAuthorQuotesAggregates.add(Aggregates.match(
                               Filters.and(
                                       Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language),
                                       Filters.eq(QuoteEntity.AUTHOR_ID_FIELD_NAME, authorId)
                               )
                       ));
                       topAuthorQuotesAggregates.add(Aggregates.sort(Sorts.descending(QuoteEntity.LIKES_FIELD_NAME)));
                       topAuthorQuotesAggregates.add(Aggregates.limit(RANDOM_SAMPLE_SIZE));
                       topAuthorQuotesAggregates.add(Aggregates.sample(1));
                       topAuthorQuotesAggregates.addAll(getAuthorLookupAggregationStages());

                       final AggregatePublisher<QuoteEntity> quoteResult = getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class).aggregate(topAuthorQuotesAggregates);

                       return Mono.from(quoteResult);
                   })
                   .map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Mono<Quote> getQuote(String language, String quoteId) {
        final List<Bson> aggregates = List.of(Aggregates.match(Filters.and(Filters.eq(Fields.UNDERSCORE_ID, Fields.getId(quoteId)), Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language))),
                                              Aggregates.addFields(new Field<>(QuoteEntity.AUTHOR_ID_FIELD_NAME, Expressions.toObjectId(Fields.getFieldPath(QuoteEntity.AUTHOR_ID_FIELD_NAME)))),
                                              Aggregates.lookup(AuthorEntity.COLLECTION_NAME, QuoteEntity.AUTHOR_ID_FIELD_NAME, Fields.UNDERSCORE_ID, QuoteEntity.AUTHOR_FIELD_NAME),
                                              Aggregates.unwind(Fields.getFieldPath(QuoteEntity.AUTHOR_FIELD_NAME)));

        final AggregatePublisher<QuoteEntity> result = getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class).aggregate(aggregates);

        return Mono.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Flux<Quote> searchAuthorQuotes(String language, String authorId, @Nullable String query, int limit, int offset) {
        final List<Bson> aggregates = new ArrayList<>();

        if (!Strings.isNullOrEmpty(query)) {
            aggregates.add(Aggregates.match(Filters.text(query, new TextSearchOptions().caseSensitive(false))));
        }

        aggregates.add(Aggregates.match(Filters.and(Filters.eq(QuoteEntity.AUTHOR_ID_FIELD_NAME, authorId), Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language))));
        aggregates.addAll(getPagingAggregationStages(limit, offset));
        aggregates.addAll(getAuthorLookupAggregationStages());

        final AggregatePublisher<QuoteEntity> result = getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class).aggregate(aggregates);

        return Flux.from(result).map(x -> mappingService.map(x, Quote.class));
    }

    @Override
    public Mono<Long> getQuotesCount(String language) {
        final Publisher<Long> result = getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class).countDocuments(Filters.eq(AuthorEntity.LANGUAGE_FIELD_NAME, language));

        return Mono.from(result);
    }

    @Override
    public Mono<Quote> createQuote(String language, String authorId, String text) {
        Document doc = new Document(QuoteEntity.TEXT_FIELD_NAME, text)
                .append(QuoteEntity.HASH_FIELD_NAME, Hash.sha256(text))
                .append(QuoteEntity.AUTHOR_ID_FIELD_NAME, Fields.getId(authorId))
                .append(QuoteEntity.ADDED_AT_FIELD_NAME, LocalDateTime.now())
                .append(QuoteEntity.LANGUAGE_FIELD_NAME, language)
                .append(QuoteEntity.LIKES_FIELD_NAME, 0);

        return Mono.from(getCollection(QuoteEntity.COLLECTION_NAME, Document.class).insertOne(doc))
                   .map(insertOneResult -> Objects.requireNonNull(insertOneResult.getInsertedId()).asObjectId().getValue().toString())
                   .flatMap(insertedId -> this.getQuote(language, insertedId));
    }

    @Override
    public Mono<Boolean> likeQuote(String language, String quoteId) {
        final Bson filter = Filters.and(Filters.eq(Fields.UNDERSCORE_ID, Fields.getId(quoteId)),
                                        Filters.eq(QuoteEntity.LANGUAGE_FIELD_NAME, language));

        final Bson likeIncrement = Updates.inc(QuoteEntity.LIKES_FIELD_NAME, 1);
        final UpdateOptions updateOptions = new UpdateOptions().upsert(false);

        return Mono.from(getCollection(QuoteEntity.COLLECTION_NAME, QuoteEntity.class)
                                 .updateOne(filter, likeIncrement, updateOptions))
                   .map(updateResult -> updateResult.getModifiedCount() != 0);
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

    private <T> MongoCollection<T> getCollection(String collectionName, Class<T> type) {
        return mongoClient
                .getDatabase(databaseConfiguration.getName())
                .getCollection(collectionName, type);
    }
}
