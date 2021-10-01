package net.fisenko.dictum.micronaut.exception;

import io.micronaut.context.annotation.Requires;
import io.micronaut.http.HttpRequest;
import io.micronaut.http.HttpResponse;
import io.micronaut.http.annotation.Produces;
import io.micronaut.http.server.exceptions.ExceptionHandler;
import io.micronaut.http.server.exceptions.response.ErrorContext;
import io.micronaut.http.server.exceptions.response.ErrorResponseProcessor;
import jakarta.inject.Singleton;
import net.fisenko.dictum.business.exception.ResourceNotFoundException;

@Produces
@Singleton
@Requires(classes = {ResourceNotFoundException.class, ExceptionHandler.class})
public class ResourceNotFoundExceptionHandler implements ExceptionHandler<ResourceNotFoundException, HttpResponse<?>> {

    private final ErrorResponseProcessor<?> errorResponseProcessor;

    public ResourceNotFoundExceptionHandler(ErrorResponseProcessor<?> errorResponseProcessor) {
        this.errorResponseProcessor = errorResponseProcessor;
    }

    @Override
    public HttpResponse<?> handle(HttpRequest request, ResourceNotFoundException e) {
        return errorResponseProcessor.processResponse(ErrorContext.builder(request)
                                                                  .cause(e)
                                                                  .errorMessage(e.getMessage())
                                                                  .build(),
                                                      HttpResponse.notFound());
    }
}