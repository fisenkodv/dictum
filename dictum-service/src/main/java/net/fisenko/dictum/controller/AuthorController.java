package net.fisenko.dictum.controller;

import io.micronaut.http.MediaType;
import io.micronaut.http.annotation.Controller;
import io.micronaut.http.annotation.Get;
import lombok.extern.slf4j.Slf4j;
import net.fisenko.dictum.repository.AuthorRepository;

@Slf4j
@Controller("/authors")
public class AuthorController {
    private final AuthorRepository authorRepository;

    public AuthorController(AuthorRepository authorRepository) {
        this.authorRepository = authorRepository;
    }

    @Get(produces = MediaType.TEXT_PLAIN)
    public String index() {
        log.info("sdafdsafds");
        return authorRepository.getAuthors();
    }
}
