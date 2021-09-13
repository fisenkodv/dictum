package net.fisenko.dictum.service;

import net.fisenko.dictum.repository.AuthorRepository;

public class AuthorService {
    private final AuthorRepository authorRepository;

    public AuthorService(AuthorRepository authorRepository) {
        this.authorRepository = authorRepository;
    }
}
